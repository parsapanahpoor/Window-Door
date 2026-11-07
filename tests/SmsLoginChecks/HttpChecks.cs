using System.Net;
using CRM.Data.Repository;
using System.Security.Claims;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Application.Services;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.User.Authentication;
using Window.Web.Controllers;

internal static class HttpChecks
{
    public static async Task RunAsync(DbContextOptions<WindowDbContext> options, OfflineSms sender, TimeProvider clock)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(IdentityController).Assembly.GetName().Name,
            ContentRootPath = Path.GetFullPath("Window.Web")
        });
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        builder.Logging.ClearProviders();
        builder.Services.AddSingleton(options);
        builder.Services.AddScoped<WindowDbContext>();
        builder.Services.AddScoped<SmsLoginService>();
        builder.Services.AddSingleton<ISMSService>(sender);
        builder.Services.AddSingleton(clock);
        builder.Services.AddScoped<IUserService>(services =>
        {
            var db = services.GetRequiredService<WindowDbContext>();
            return new UserService(services.GetRequiredService<IConfiguration>(), new UserRepository(db),
                null!, db, null!, null!, services.GetRequiredService<ISMSService>());
        });
        builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(cookie => cookie.LoginPath = "/Login");
        builder.Services.AddAuthorization();
        builder.Services.AddControllersWithViews().AddViewLocalization();
        await using var app = builder.Build();
        app.UseStaticFiles();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/session-check", (ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.AuthenticationMethod) ?? "password").RequireAuthorization();
        app.MapGet("/session-account", (ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier)!).RequireAuthorization();
        await app.StartAsync();

        using var browser = Browser(app.Urls.Single());
        foreach (var path in new[] { "/login", "/login/password", "/register", "/ForgotPassword" })
        {
            var html = await browser.GetStringAsync(path);
            Check(html.Contains("dir=\"rtl\"") && html.Contains("/account/account.css") &&
                !html.Contains("fi-google") && !html.Contains("fi-facebook"), "Render only working controls in the shared RTL layout");
        }
        Check((await browser.GetStringAsync("/account/window.svg")).Contains("<svg") &&
            (await browser.GetStringAsync("/account/door.svg")).Contains("<svg"), "Serve both original illustrations");

        const string phone = "09100000031";
        const string start = "/login?returnUrl=https%3A%2F%2Fexample.invalid";
        var defaultForm = new HtmlDocument();
        defaultForm.LoadHtml(await browser.GetStringAsync(start));
        Check(defaultForm.DocumentNode.SelectSingleNode("//nav//a[@aria-current='page']")?.InnerText == "ورود با کد پیامکی" &&
            defaultForm.DocumentNode.SelectSingleNode("//nav//a[contains(@href,'/login/password')]")?.InnerText == "ورود با کلمهٔ عبور" &&
            defaultForm.DocumentNode.SelectSingleNode("//input[@name='Password']") is null,
            "The canonical entry defaults to SMS and names both methods clearly");
        using (var unauthorized = await browser.GetAsync("/session-check"))
            Check(unauthorized.Headers.Location?.AbsolutePath == "/Login", "Protected routes point to the default SMS entry");
        using (var forged = await Submit(browser, "/login/sms", new() { ["Mobile"] = phone }))
            Check(forged.StatusCode == HttpStatusCode.BadRequest && sender.Count(phone) == 0, "Reject SMS POST without antiforgery token");

        var token = Token(await browser.GetStringAsync(start));
        var smsAction = HtmlEntity.DeEntitize(defaultForm.DocumentNode.SelectSingleNode("//form[@method='post']").GetAttributeValue("action", ""));
        Check(smsAction == "/login/sms?returnUrl=%2F", "Default form posts to SMS with a safe return URL");
        using (var issued = await Submit(browser, smsAction, new() { ["Mobile"] = "۰۹۱۰۰۰۰۰۰۳۱", ["__RequestVerificationToken"] = token }))
            Check(issued.StatusCode == HttpStatusCode.Redirect && issued.Headers.Location?.OriginalString == "/login/sms/code",
                "Start passwordless flow with a normalized phone");

        var codePage = await browser.GetStringAsync("/login/sms/code");
        var view = new HtmlDocument();
        view.LoadHtml(codePage);
        Check(view.DocumentNode.SelectSingleNode("//form[@data-resend-seconds='120']") is not null &&
            !codePage.Contains(phone), "Render a two-minute timer without exposing the full mobile");

        using var otherBrowser = Browser(app.Urls.Single());
        var otherToken = Token(await otherBrowser.GetStringAsync(start));
        using (var stolen = await Submit(otherBrowser, "/login/sms/code", new()
        {
            ["Code"] = sender.Code(phone), ["__RequestVerificationToken"] = otherToken
        }))
            Check(stolen.Headers.Location?.OriginalString == "/login/sms", "A different browser cannot submit the code");

        token = Token(codePage);
        using (var early = await Submit(browser, "/login/sms/resend", new() { ["__RequestVerificationToken"] = token }))
            Check(early.StatusCode == HttpStatusCode.Redirect && sender.Count(phone) == 1, "The resend endpoint enforces cooldown");

        using (var verified = await Submit(browser, "/login/sms/code", new()
        {
            ["Code"] = sender.Code(phone), ["__RequestVerificationToken"] = token
        }))
            Check(verified.StatusCode == HttpStatusCode.Redirect && verified.Headers.Location?.OriginalString == "/",
                "Successful verification rejects the external return URL");

        Check(await browser.GetStringAsync("/session-check") == SmsLoginService.AuthenticationMethod,
            "SMS verification issues the normal authentication cookie");

        const string sharedPhone = "09100000032";
        var first = new User
        {
            Mobile = sharedPhone, Username = "first shared account",
            Password = PasswordHelper.EncodePasswordMd5("first-password"), IsMobileConfirm = true,
            EmailActivationCode = Guid.NewGuid().ToString("N"), MobileActivationCode = "654321",
            ExpireMobileSMSDateTime = DateTime.Now
        };
        var second = new User
        {
            Mobile = sharedPhone, Username = "second shared account",
            Password = PasswordHelper.EncodePasswordMd5("second-password"), IsMobileConfirm = true,
            EmailActivationCode = Guid.NewGuid().ToString("N"), MobileActivationCode = Guid.NewGuid().ToString("N")
        };
        await using (var db = new WindowDbContext(options))
        {
            foreach (var account in new[] { first, second })
            {
                db.Users.Add(account);
                db.Entry(account).Property(MobileLoginKey.Excluded).CurrentValue = true;
            }
            await db.SaveChangesAsync();
        }

        using var recoveryBrowser = Browser(app.Urls.Single());
        var recoveryToken = Token(await recoveryBrowser.GetStringAsync("/ForgotPassword"));
        using var recovery = await Submit(recoveryBrowser, "/ForgotPassword", new()
        {
            ["Mobile"] = sharedPhone, ["__RequestVerificationToken"] = recoveryToken
        });
        Check(recovery.StatusCode == HttpStatusCode.OK && sender.Count(sharedPhone) == 0,
            "Password recovery cannot send a code to a shared number");
        using var reset = await Submit(recoveryBrowser, $"/reset-pass/{sharedPhone}", new()
        {
            ["ActiveCode"] = "654321", ["Password"] = "new-fixture-password",
            ["ConfirmPassword"] = "new-fixture-password", ["__RequestVerificationToken"] = recoveryToken
        });
        Check(reset.StatusCode == HttpStatusCode.NotFound, "A previously issued code cannot reset a shared account");
        using var activation = await recoveryBrowser.GetAsync($"/Active-mobile/{sharedPhone}/true");
        Check(activation.StatusCode == HttpStatusCode.NotFound && sender.Count(sharedPhone) == 0,
            "Legacy activation cannot send a code to a shared number");

        using var passwordBrowser = Browser(app.Urls.Single());
        var fallbackToken = Token(await passwordBrowser.GetStringAsync("/login?returnUrl=%2Fsession-account"));
        using var fallback = await Submit(passwordBrowser, "/login/sms?returnUrl=%2Fsession-account", new()
        {
            ["Mobile"] = sharedPhone, ["__RequestVerificationToken"] = fallbackToken
        });
        const string passwordPath = "/login/password?returnUrl=%2Fsession-account";
        Check(fallback.Headers.Location?.OriginalString == passwordPath && sender.Count(sharedPhone) == 0,
            "A shared mobile goes directly to password sign-in without sending SMS and keeps the return destination");
        var fallbackPage = await passwordBrowser.GetStringAsync(passwordPath);
        const string fallbackMessage = "ورود پیامکی با این شمارهٔ موبایل امکان‌پذیر نیست.";
        Check(WebUtility.HtmlDecode(fallbackPage).Contains(fallbackMessage) &&
            !WebUtility.HtmlDecode(await passwordBrowser.GetStringAsync(passwordPath)).Contains(fallbackMessage),
            "Show the fallback message once on the password form");

        var passwordToken = Token(fallbackPage);
        using (var passwordSignIn = await Submit(passwordBrowser, passwordPath, new()
        {
            ["Mobile"] = sharedPhone, ["Password"] = "second-password",
            ["RememberMe"] = "true", ["__RequestVerificationToken"] = passwordToken
        }))
            Check(passwordSignIn.Headers.Location?.OriginalString == "/session-account", "Password flow keeps a local return URL");
        Check(await passwordBrowser.GetStringAsync("/session-account") == second.Id.ToString() &&
            await passwordBrowser.GetStringAsync("/session-check") == "password",
            "Password fallback signs into the matching account, even when the raw number is identical");

        await using (var db = new WindowDbContext(options))
            await db.Users.Where(u => u.Id == first.Id).ExecuteUpdateAsync(s => s.SetProperty(u => u.Password, second.Password));
        using var ambiguousBrowser = Browser(app.Urls.Single());
        var ambiguousToken = Token(await ambiguousBrowser.GetStringAsync(passwordPath));
        using var ambiguous = await Submit(ambiguousBrowser, "/login?returnUrl=%2Fsession-account", new()
        {
            ["Mobile"] = sharedPhone, ["Password"] = "second-password",
            ["__RequestVerificationToken"] = ambiguousToken
        });
        Check(ambiguous.StatusCode == HttpStatusCode.OK &&
            WebUtility.HtmlDecode(await ambiguous.Content.ReadAsStringAsync()).Contains("شمارهٔ موبایل یا کلمهٔ عبور درست نیست."),
            "A legacy password POST cannot sign in when the same credentials match more than one account");

        await app.StopAsync();
    }

    private static HttpClient Browser(string url) => new(new HttpClientHandler
    {
        AllowAutoRedirect = false,
        UseProxy = false
    }) { BaseAddress = new Uri(url) };

    private static async Task<HttpResponseMessage> Submit(HttpClient client, string url, Dictionary<string, string> fields)
    {
        using var body = new FormUrlEncodedContent(fields);
        return await client.PostAsync(url, body);
    }

    private static string Token(string html)
    {
        var page = new HtmlDocument();
        page.LoadHtml(html);
        return page.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']")
            .GetAttributeValue("value", "");
    }

    private static void Check(bool result, string description)
    {
        if (!result)
            throw new InvalidOperationException(description);
    }
}
