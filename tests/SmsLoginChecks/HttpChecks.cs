using System.Net;
using System.Reflection;
using System.Security.Claims;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Window.Application.Interfaces;
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
        builder.Services.AddSingleton(DispatchProxy.Create<IUserService, PasswordAccountStub>());
        builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
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
        await app.StartAsync();

        using var browser = Browser(app.Urls.Single());
        foreach (var path in new[] { "/login", "/register", "/ForgotPassword" })
        {
            var html = await browser.GetStringAsync(path);
            Check(html.Contains("dir=\"rtl\"") && html.Contains("/account/account.css") &&
                !html.Contains("fi-google") && !html.Contains("fi-facebook"), "Render only working controls in the shared RTL layout");
        }
        Check((await browser.GetStringAsync("/account/window.svg")).Contains("<svg") &&
            (await browser.GetStringAsync("/account/door.svg")).Contains("<svg"), "Serve both original illustrations");

        const string phone = "09100000031";
        const string start = "/login/sms?returnUrl=https%3A%2F%2Fexample.invalid";
        using (var forged = await Submit(browser, start, new() { ["Mobile"] = phone }))
            Check(forged.StatusCode == HttpStatusCode.BadRequest && sender.Count(phone) == 0, "Reject SMS POST without antiforgery token");

        var token = Token(await browser.GetStringAsync(start));
        using (var issued = await Submit(browser, start, new() { ["Mobile"] = "۰۹۱۰۰۰۰۰۰۳۱", ["__RequestVerificationToken"] = token }))
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

        using var passwordBrowser = Browser(app.Urls.Single());
        var passwordToken = Token(await passwordBrowser.GetStringAsync("/login?returnUrl=%2Fsession-check"));
        using (var passwordSignIn = await Submit(passwordBrowser, "/login?returnUrl=%2Fsession-check", new()
        {
            ["Mobile"] = "09100000032", ["Password"] = "fixture-password",
            ["RememberMe"] = "true", ["__RequestVerificationToken"] = passwordToken
        }))
            Check(passwordSignIn.Headers.Location?.OriginalString == "/session-check", "Password flow keeps a local return URL");
        Check(await passwordBrowser.GetStringAsync("/session-check") == "password", "Password sign-in still issues its cookie");

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

public class PasswordAccountStub : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => targetMethod?.Name switch
    {
        nameof(IUserService.LoginUserAsync) => Task.FromResult(LoginUserResponse.Success),
        nameof(IUserService.GetUserByEmail) => Task.FromResult<User?>(new User
        {
            Id = 42,
            Mobile = "09100000032",
            Username = "Fixture user",
            IsMobileConfirm = true
        }),
        _ => throw new NotSupportedException(targetMethod?.Name)
    };
}
