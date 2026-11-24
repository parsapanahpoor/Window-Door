using System.Net;
using System.Reflection;
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
using Window.Application.Services.Implementation;
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
                null!, db, null!, null!, services.GetRequiredService<ISMSService>(),
                new Window.Application.Services.Services.MarketService(db));
        });
        builder.Services.AddScoped<IPermissionService, PermissionService>();
        builder.Services.AddScoped<IMarketService, Window.Application.Services.Services.MarketService>();
        builder.Services.AddScoped<ISellerService>(_ => DispatchProxy.Create<ISellerService, SellerServiceStub>());
        builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(cookie => cookie.LoginPath = "/Login");
        builder.Services.AddAuthorization();
        builder.Services.AddControllersWithViews().AddViewLocalization();
        await using var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
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
        using (var roleStep = await Submit(browser, smsAction, new() { ["Mobile"] = "۰۹۱۰۰۰۰۰۰۳۱", ["__RequestVerificationToken"] = token }))
        {
            Check(roleStep.StatusCode == HttpStatusCode.OK, "A new number opens the account-type step");
            Check(sender.Count(phone) == 0, "No SMS is sent before the account type is chosen");
        }
        var accountTypeToken = Token(await browser.GetStringAsync("/login/sms/register"));
        using (var issued = await Submit(browser, "/login/sms/register", new() { ["accountType"] = "Customer", ["__RequestVerificationToken"] = accountTypeToken }))
            Check(issued.StatusCode == HttpStatusCode.Redirect && issued.Headers.Location?.OriginalString == "/login/sms/code",
                "Customer registration starts the passwordless challenge");

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
            Check(verified.StatusCode == HttpStatusCode.Redirect && verified.Headers.Location?.OriginalString == "/UserPanel",
                "Successful verification rejects the external return URL and opens the personal panel");

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

        ulong sellerAssignmentId;
        await using (var db = new WindowDbContext(options))
        {
            var adminRole = new Role { Title = "Administrator", RoleUniqueName = "Admin" };
            var sellerRole = new Role { Title = "Seller owner", RoleUniqueName = "SellerMaster" };
            var sellerAssignment = new UserRole { UserId = second.Id, Role = sellerRole };
            db.UserRoles.AddRange(new UserRole { UserId = second.Id, Role = adminRole }, sellerAssignment);
            await db.SaveChangesAsync();
            sellerAssignmentId = sellerAssignment.Id;
        }

        foreach (var expectedPanel in new[] { "/Seller", "/Admin" })
        {
            using var roleBrowser = Browser(app.Urls.Single());
            var roleToken = Token(await roleBrowser.GetStringAsync("/login/password"));
            using var signedIn = await Submit(roleBrowser, "/login/password", new()
            {
                ["Mobile"] = sharedPhone, ["Password"] = "second-password", ["__RequestVerificationToken"] = roleToken
            });
            var expectedDestination = expectedPanel == "/Seller"
                ? "/register/store"
                : expectedPanel;
            var failureBody = signedIn.StatusCode == HttpStatusCode.Redirect ? "" : (await signedIn.Content.ReadAsStringAsync());
            Check(signedIn.StatusCode == HttpStatusCode.Redirect && signedIn.Headers.Location?.OriginalString == expectedDestination,
                $"Password sign-in opens {expectedDestination} for the matching account roles (got {(int)signedIn.StatusCode} {signedIn.Headers.Location} {failureBody[..Math.Min(400, failureBody.Length)]})");
            using var existingSession = await roleBrowser.GetAsync("/login?returnUrl=%2Flogin");
            Check(existingSession.Headers.Location?.OriginalString == expectedDestination, "Signed-in login entry opens the role panel");

            if (expectedPanel == "/Seller")
            {
                // The seller lost the registration session before the store step. After signing
                // in with the verified phone, the dedicated store form must finish the flow.
                var resumeToken = Token(await roleBrowser.GetStringAsync("/register/store"));
                using var resumed = await Submit(roleBrowser, "/register/store", new()
                {
                    ["marketName"] = "فروشگاه بازیابی‌شده", ["__RequestVerificationToken"] = resumeToken
                });
                Check(resumed.StatusCode == HttpStatusCode.Redirect &&
                    resumed.Headers.Location?.OriginalString == "/Seller/SellerPersonalInfo/PageOfManageSellerInformation?welcome=1",
                    "A seller who lost the session completes the store after sign-in");

                await using var resumeDb = new WindowDbContext(options);
                Check(await resumeDb.Market.AnyAsync(m => m.UserId == second.Id && !m.IsDelete),
                    "The resumed store belongs to the signed-in seller");

                await using var db = new WindowDbContext(options);
                await db.UserRoles.Where(assignment => assignment.Id == sellerAssignmentId)
                    .ExecuteUpdateAsync(update => update.SetProperty(assignment => assignment.IsDelete, true));
                // Completing the store added the project's owner role (id 4); remove it too so the
                // next iteration verifies the admin destination for a pure admin account.
                await db.UserRoles.Where(assignment => assignment.UserId == second.Id &&
                        assignment.RoleId == 4 && !assignment.IsDelete)
                    .ExecuteUpdateAsync(update => update.SetProperty(assignment => assignment.IsDelete, true));
            }
        }

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

        // New seller through the SMS flow: role -> store -> code -> verified -> store information.
        const string sellerPhone = "09100000071";
        var sellerBrowser = Browser(app.Urls.Single());
        var sellerEntryToken = Token(await sellerBrowser.GetStringAsync("/login/sms"));
        using (var sellerStep = await Submit(sellerBrowser, "/login/sms", new() { ["Mobile"] = sellerPhone, ["__RequestVerificationToken"] = sellerEntryToken }))
            Check(sellerStep.StatusCode == HttpStatusCode.OK && sender.Count(sellerPhone) == 0,
                "A new seller number asks for the account type without sending an SMS");
        var sellerRoleToken = Token(await sellerBrowser.GetStringAsync("/login/sms/register"));
        using (var storePage = await Submit(sellerBrowser, "/login/sms/register", new() { ["accountType"] = "Seller", ["__RequestVerificationToken"] = sellerRoleToken }))
            Check(storePage.StatusCode == HttpStatusCode.OK && sender.Count(sellerPhone) == 0,
                "The store name is requested before any SMS");
        var storeToken = Token(await sellerBrowser.GetStringAsync("/login/sms/register/store"));
        using (var store = await Submit(sellerBrowser, "/login/sms/register/store", new() { ["marketName"] = "فروشگاه تست پیامکی", ["__RequestVerificationToken"] = storeToken }))
            Check(store.StatusCode == HttpStatusCode.Redirect && store.Headers.Location?.OriginalString == "/login/sms/code",
                "A valid store name starts the SMS challenge");
        Check(sender.Count(sellerPhone) == 1, "Exactly one activation SMS is sent after the store");
        var sellerCodeToken = Token(await sellerBrowser.GetStringAsync("/login/sms/code"));
        using (var sellerVerified = await Submit(sellerBrowser, "/login/sms/code", new() { ["Code"] = sender.Code(sellerPhone), ["__RequestVerificationToken"] = sellerCodeToken }))
            Check(sellerVerified.StatusCode == HttpStatusCode.Redirect &&
                sellerVerified.Headers.Location?.OriginalString == "/Seller/SellerPersonalInfo/PageOfManageSellerInformation?welcome=1",
                "The verified seller is signed in and sent to store information");

        await using (var db = new WindowDbContext(options))
        {
            var seller = await db.Users.AsNoTracking().SingleAsync(u => u.Mobile == sellerPhone);
            var market = await db.Market.AsNoTracking().SingleAsync(m => m.UserId == seller.Id && !m.IsDelete);
            Check(seller.IsMobileConfirm, "The seller mobile is confirmed only after the code");
            Check(market.MarketPersonalsInfoState == Window.Domain.Entities.Market.MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller,
                "A new store starts in the incomplete state");
            Check(await db.UserRoles.AnyAsync(r => r.UserId == seller.Id && r.RoleId == 4 && !r.IsDelete),
                "The seller receives the owner role");
            Check(await db.MarketUser.AnyAsync(mu => mu.MarketId == market.Id && mu.UserId == seller.Id && mu.RoleId == 4),
                "The store owner link is created");
        }

        // Password registration for a seller: account + store are committed before activation.
        const string passwordSellerPhone = "09100000072";
        var passwordSeller = Browser(app.Urls.Single());
        var registerToken = Token(await passwordSeller.GetStringAsync("/register"));
        using (var registered = await Submit(passwordSeller, "/register", new()
        {
            ["Mobile"] = passwordSellerPhone, ["Password"] = "seller-password", ["RePassword"] = "seller-password",
            ["AccountType"] = "Seller", ["MarketName"] = "فروشگاه ثبت‌نام رمز", ["__RequestVerificationToken"] = registerToken
        }))
            Check(registered.StatusCode == HttpStatusCode.Redirect &&
                registered.Headers.Location?.OriginalString == $"/Active-mobile/{passwordSellerPhone}",
                "Seller password registration redirects to mobile activation");

        string passwordActivationCode;
        await using (var db = new WindowDbContext(options))
        {
            var seller = await db.Users.AsNoTracking().SingleAsync(u => u.Mobile == passwordSellerPhone);
            passwordActivationCode = seller.MobileActivationCode;
            Check(!seller.IsMobileConfirm, "The password account is unconfirmed before activation");
            Check(await db.Market.AnyAsync(m => m.UserId == seller.Id && !m.IsDelete),
                "The store is created in the same registration transaction");
        }

        var activationToken = Token(await passwordSeller.GetStringAsync($"/Active-mobile/{passwordSellerPhone}"));
        using (var activated = await Submit(passwordSeller, $"/Active-mobile/{passwordSellerPhone}", new()
        {
            ["Mobile"] = passwordSellerPhone, ["MobileActiveCode"] = passwordActivationCode,
            ["__RequestVerificationToken"] = activationToken
        }))
            Check(activated.StatusCode == HttpStatusCode.Redirect &&
                activated.Headers.Location?.OriginalString == "/Seller/SellerPersonalInfo/PageOfManageSellerInformation?welcome=1",
                "Activation signs the seller in and opens store information");

        // Password registration for a customer: no store and no seller role.
        const string passwordCustomerPhone = "09100000073";
        var passwordCustomer = Browser(app.Urls.Single());
        var customerToken = Token(await passwordCustomer.GetStringAsync("/register"));
        using (var registeredCustomer = await Submit(passwordCustomer, "/register", new()
        {
            ["Mobile"] = passwordCustomerPhone, ["Password"] = "customer-password", ["RePassword"] = "customer-password",
            ["AccountType"] = "Customer", ["__RequestVerificationToken"] = customerToken
        }))
            Check(registeredCustomer.StatusCode == HttpStatusCode.Redirect &&
                registeredCustomer.Headers.Location?.OriginalString == $"/Active-mobile/{passwordCustomerPhone}",
                "Customer password registration redirects to mobile activation");

        string customerActivationCode;
        await using (var db = new WindowDbContext(options))
        {
            var customer = await db.Users.AsNoTracking().SingleAsync(u => u.Mobile == passwordCustomerPhone);
            customerActivationCode = customer.MobileActivationCode;
            Check(!await db.Market.AnyAsync(m => m.UserId == customer.Id && !m.IsDelete), "A customer registration creates no store");
            Check(!await db.UserRoles.AnyAsync(r => r.UserId == customer.Id && r.RoleId == 4 && !r.IsDelete), "A customer receives no seller role");
        }

        var customerActivationToken = Token(await passwordCustomer.GetStringAsync($"/Active-mobile/{passwordCustomerPhone}"));
        using (var customerActivated = await Submit(passwordCustomer, $"/Active-mobile/{passwordCustomerPhone}", new()
        {
            ["Mobile"] = passwordCustomerPhone, ["MobileActiveCode"] = customerActivationCode,
            ["__RequestVerificationToken"] = customerActivationToken
        }))
            Check(customerActivated.StatusCode == HttpStatusCode.Redirect &&
                customerActivated.Headers.Location?.OriginalString == "/UserPanel",
                "Activated customer opens the personal panel");

        // HTTP edge cases. GET helpers assert status and print the Location so a redirect is
        // diagnosed instead of being swallowed by GetStringAsync.
        async Task<string> GetPage(HttpClient client, string path)
        {
            using var response = await client.GetAsync(path);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GET {path} -> {(int)response.StatusCode} {response.Headers.Location}");
            Check(response.StatusCode == HttpStatusCode.OK, $"GET {path} must return 200");
            return body;
        }

        // 1a. Register rejects a forged POST.
        const string forgedPhone = "09100000301";
        using (var forgedRegister = await Submit(browser, "/register", new()
        { ["Mobile"] = forgedPhone, ["Password"] = "pass1234", ["RePassword"] = "pass1234", ["AccountType"] = "Customer" }))
            Check(forgedRegister.StatusCode == HttpStatusCode.BadRequest, "Register rejects a POST without antiforgery");
        await using (var db = new WindowDbContext(options))
            Check(!await db.Users.AnyAsync(u => u.Mobile == forgedPhone), "A forged register POST creates no account");

        // 1b. Activation rejects a forged POST and does not confirm the account.
        const string activationPhone = "09100000302";
        var activationBrowser = Browser(app.Urls.Single());
        var activationRegisterToken = Token(await GetPage(activationBrowser, "/register"));
        using (var registered = await Submit(activationBrowser, "/register", new()
        {
            ["Mobile"] = activationPhone, ["Password"] = "pass1234", ["RePassword"] = "pass1234",
            ["AccountType"] = "Customer", ["__RequestVerificationToken"] = activationRegisterToken
        }))
            Console.WriteLine($"POST /register (activation fixture) -> {(int)registered.StatusCode} {registered.Headers.Location}");
        string activationCode;
        await using (var db = new WindowDbContext(options))
            activationCode = (await db.Users.AsNoTracking().SingleAsync(u => u.Mobile == activationPhone)).MobileActivationCode;
        using (var forgedActivation = await Submit(activationBrowser, $"/Active-mobile/{activationPhone}", new()
        { ["Mobile"] = activationPhone, ["MobileActiveCode"] = activationCode }))
            Check(forgedActivation.StatusCode == HttpStatusCode.BadRequest, "Activation rejects a POST without antiforgery");
        await using (var db = new WindowDbContext(options))
            Check(!(await db.Users.AsNoTracking().SingleAsync(u => u.Mobile == activationPhone)).IsMobileConfirm,
                "A forged activation does not confirm the mobile");

        // 2. Missing or unknown account type creates nothing.
        const string invalidRolePhone = "09100000303";
        var invalidBrowser = Browser(app.Urls.Single());
        var invalidEntryToken = Token(await GetPage(invalidBrowser, "/login/sms"));
        using (var entry = await Submit(invalidBrowser, "/login/sms", new() { ["Mobile"] = invalidRolePhone, ["__RequestVerificationToken"] = invalidEntryToken }))
            Console.WriteLine($"POST /login/sms (new number) -> {(int)entry.StatusCode} {entry.Headers.Location}");
        var invalidRoleToken = Token(await GetPage(invalidBrowser, "/login/sms/register"));
        using (var invalidRole = await Submit(invalidBrowser, "/login/sms/register", new() { ["accountType"] = "NotARole", ["__RequestVerificationToken"] = invalidRoleToken }))
            Check(invalidRole.StatusCode == HttpStatusCode.OK, "The SMS role step rejects an unknown account type");
        await using (var db = new WindowDbContext(options))
            Check(!await db.Users.AnyAsync(u => u.Mobile == invalidRolePhone), "An unknown SMS role creates no account");

        // 3. Space-only and over-long store names are rejected.
        const string spacesPhone = "09100000304";
        var spacesBrowser = Browser(app.Urls.Single());
        var spacesEntryToken = Token(await GetPage(spacesBrowser, "/login/sms"));
        await Submit(spacesBrowser, "/login/sms", new() { ["Mobile"] = spacesPhone, ["__RequestVerificationToken"] = spacesEntryToken });
        var spacesRoleToken = Token(await GetPage(spacesBrowser, "/login/sms/register"));
        using (var role = await Submit(spacesBrowser, "/login/sms/register", new() { ["accountType"] = "Seller", ["__RequestVerificationToken"] = spacesRoleToken }))
            Console.WriteLine($"POST /login/sms/register (seller) -> {(int)role.StatusCode} {role.Headers.Location}");
        var spacesStoreToken = Token(await GetPage(spacesBrowser, "/login/sms/register/store"));
        using (var spacesStore = await Submit(spacesBrowser, "/login/sms/register/store", new() { ["marketName"] = "   ", ["__RequestVerificationToken"] = spacesStoreToken }))
            Check(spacesStore.StatusCode == HttpStatusCode.OK, "A space-only store name is rejected in the SMS flow");
        Check(sender.Count(spacesPhone) == 0, "A rejected store name sends no SMS");
        var longStoreToken = Token(await GetPage(spacesBrowser, "/login/sms/register/store"));
        using (var longStore = await Submit(spacesBrowser, "/login/sms/register/store", new() { ["marketName"] = new string('a', 101), ["__RequestVerificationToken"] = longStoreToken }))
            Check(longStore.StatusCode == HttpStatusCode.OK, "A store name over 100 characters is rejected in the SMS flow");

        // 4. A validation error preserves the mobile, role and store value.
        var preserveBrowser = Browser(app.Urls.Single());
        var preserveToken = Token(await GetPage(preserveBrowser, "/register"));
        using (var preserve = await Submit(preserveBrowser, "/register", new()
        {
            ["Mobile"] = "09100000305", ["Password"] = "pass1234", ["RePassword"] = "pass1234",
            ["AccountType"] = "Seller", ["MarketName"] = "ab", ["__RequestVerificationToken"] = preserveToken
        }))
        {
            var preserved = WebUtility.HtmlDecode(await preserve.Content.ReadAsStringAsync());
            Check(preserve.StatusCode == HttpStatusCode.OK && preserved.Contains("value=\"09100000305\"") &&
                preserved.Contains("value=\"ab\"") && preserved.Contains("value=\"Seller\""),
                "A validation error preserves the mobile, role and store name");
        }

        // 5. A provider failure does not duplicate records; resend continues the flow.
        const string retryPhone = "09100000306";
        sender.RejectedMobile = retryPhone;
        var retryBrowser = Browser(app.Urls.Single());
        var retryEntryToken = Token(await GetPage(retryBrowser, "/login/sms"));
        await Submit(retryBrowser, "/login/sms", new() { ["Mobile"] = retryPhone, ["__RequestVerificationToken"] = retryEntryToken });
        var retryRoleToken = Token(await GetPage(retryBrowser, "/login/sms/register"));
        await Submit(retryBrowser, "/login/sms/register", new() { ["accountType"] = "Seller", ["__RequestVerificationToken"] = retryRoleToken });
        var retryStoreToken = Token(await GetPage(retryBrowser, "/login/sms/register/store"));
        using (var failedSend = await Submit(retryBrowser, "/login/sms/register/store", new() { ["marketName"] = "فروشگاه تلاش مجدد", ["__RequestVerificationToken"] = retryStoreToken }))
            Check(failedSend.StatusCode == HttpStatusCode.Redirect, "A provider failure continues the flow without a server error");
        await using (var db = new WindowDbContext(options))
        {
            var retryUser = await db.Users.AsNoTracking().SingleAsync(u => u.Mobile == retryPhone);
            Check(await db.Market.AsNoTracking().CountAsync(m => m.UserId == retryUser.Id) == 1, "One store exists after a provider failure");
            Check(await db.UserRoles.AsNoTracking().CountAsync(r => r.UserId == retryUser.Id && r.RoleId == 4) == 1, "One owner role exists after a provider failure");
        }
        var sendsAfterFailure = sender.Count(retryPhone);
        ((TestClock)clock).Advance(TimeSpan.FromMinutes(3));
        sender.RejectedMobile = null;
        var retryCodeToken = Token(await GetPage(retryBrowser, "/login/sms/code"));
        using (var resent = await Submit(retryBrowser, "/login/sms/resend", new() { ["__RequestVerificationToken"] = retryCodeToken }))
            Check(resent.StatusCode == HttpStatusCode.Redirect, "Resend continues the same registration flow");
        Check(sender.Count(retryPhone) == sendsAfterFailure + 1, "The retry sends exactly one more activation code");

        // 6. An unconfirmed account cannot obtain a panel session.
        var unconfirmedBrowser = Browser(app.Urls.Single());
        var unconfirmedToken = Token(await GetPage(unconfirmedBrowser, "/login/password"));
        using (var unconfirmedLogin = await Submit(unconfirmedBrowser, "/login/password", new()
        { ["Mobile"] = activationPhone, ["Password"] = "pass1234", ["__RequestVerificationToken"] = unconfirmedToken }))
            Check(unconfirmedLogin.StatusCode == HttpStatusCode.Redirect &&
                unconfirmedLogin.Headers.Location?.OriginalString.StartsWith($"/Active-mobile/{activationPhone}") == true,
                "An unconfirmed account is sent to activation instead of the panel");
        using (var panel = await unconfirmedBrowser.GetAsync("/Seller"))
            Check(panel.StatusCode == HttpStatusCode.Redirect && panel.Headers.Location?.AbsolutePath == "/Login",
                "An unconfirmed account has no panel session after password sign-in");
        using var anonymousPanel = Browser(app.Urls.Single());
        using (var directPanel = await anonymousPanel.GetAsync("/Seller"))
            Check(directPanel.StatusCode == HttpStatusCode.Redirect && directPanel.Headers.Location?.AbsolutePath == "/Login",
                "Direct panel access without a session is redirected to login");

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

public class SellerServiceStub : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        => targetMethod!.Name == nameof(ISellerService.GetMarketByUserId)
            ? Task.FromResult<Window.Domain.Entities.Market.Market?>(null)
            : throw new NotSupportedException(targetMethod.Name);
}
