using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Window.Data.Context;
using Window.Domain.Enums.SellerType;

internal static class InquiryHttpChecks
{
    public static async Task RunAsync(DbContextOptions<WindowDbContext> options, ulong state, ulong city, ulong glass, string mobile)
    {
        await using var context = new WindowDbContext(options);
        var web = Path.GetFullPath("Window.Web");
        var configuration = new DirectoryInfo(AppContext.BaseDirectory).Parent!.Name;
        var dll = Path.Combine(web, "bin", configuration, "net8.0", "Window.Web.dll");
        var portPicker = new TcpListener(IPAddress.Loopback, 0);
        portPicker.Start();
        var port = ((IPEndPoint)portPicker.LocalEndpoint).Port;
        portPicker.Stop();
        var origin = new Uri($"http://127.0.0.1:{port}");
        var launch = new ProcessStartInfo("dotnet") { WorkingDirectory = web, UseShellExecute = false,
            RedirectStandardOutput = true, RedirectStandardError = true };
        launch.ArgumentList.Add(dll);
        launch.Environment["ASPNETCORE_URLS"] = origin.ToString().TrimEnd('/');
        launch.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        launch.Environment["ConnectionStrings__WindowConnection"] = context.Database.GetConnectionString();
        using var app = Process.Start(launch) ?? throw new InvalidOperationException("Cannot launch the fixture web app.");
        var stdout = app.StandardOutput.ReadToEndAsync();
        var stderr = app.StandardError.ReadToEndAsync();

        try
        {
            using var guest = Client(origin);
            using var other = Client(origin);
            var ready = false;
            for (var attempt = 0; attempt < 50; attempt++)
            {
                if (app.HasExited) break;
                try
                {
                    using var health = await guest.GetAsync("/");
                    ready = health.IsSuccessStatusCode;
                    if (ready) break;
                }
                catch (HttpRequestException) { }
                await Task.Delay(150);
            }
            if (!ready)
            {
                if (!app.HasExited) app.Kill(entireProcessTree: true);
                await app.WaitForExitAsync();
                var output = (await stdout + await stderr).Split((char)10)
                    .Where(line => line.Contains("Exception", StringComparison.OrdinalIgnoreCase)
                        || line.Contains("fail:", StringComparison.OrdinalIgnoreCase)).TakeLast(10);
                throw new InvalidOperationException("Fixture web app failed to start. " + string.Join(Environment.NewLine, output));
            }

            var first = await Start(guest);
            var second = await Start(guest);
            var foreign = await Start(other);

            await Expect(guest, Step(first), HttpStatusCode.OK);
            await Expect(guest, Step(second), HttpStatusCode.OK);
            await Expect(other, Step(first), HttpStatusCode.Redirect);
            await Expect(guest, Step(foreign), HttpStatusCode.Redirect);
            await Expect(guest, "/InquiryProduct/InquiryStep2", HttpStatusCode.MethodNotAllowed);
            using (var rejected = await guest.PostAsync("/InquiryProduct/CompleteInquiry", new FormUrlEncodedContent([])))
                Check(rejected.StatusCode == HttpStatusCode.BadRequest, "Completion requires antiforgery.");

            var login = await guest.GetStringAsync("/login/password");
            using var signedIn = await guest.PostAsync("/login/password", Form(
                ("__RequestVerificationToken", ReadToken(login)), ("Mobile", mobile), ("Password", "fixture-password")));
            Check(signedIn.StatusCode == HttpStatusCode.Redirect, $"Password login returned {signedIn.StatusCode}.");
            var owner = await context.Users.Where(user => user.Mobile == mobile).Select(user => user.Id).SingleAsync();
            Check(await context.LogInquiryForUsers.CountAsync(draft => draft.OwnerUserId == owner
                && (draft.UserMAcAddress == first || draft.UserMAcAddress == second)) == 2, "Login must claim both drafts in the current session.");
            Check(await context.LogInquiryForUsers.AnyAsync(draft => draft.UserMAcAddress == foreign && draft.OwnerUserId == null),
                "Login must not claim the other session's draft.");
            await Expect(guest, Step(first), HttpStatusCode.OK);
            await Expect(guest, Step(foreign), HttpStatusCode.Redirect);
            await Expect(guest, "/Admin/LogForVisitSellerProfile/FilterForLogInquiry", HttpStatusCode.Redirect);
            await Expect(guest, "/Seller/Product/FilterProducts", HttpStatusCode.Redirect);
            await Expect(guest, "/logout", HttpStatusCode.Redirect);
            await Expect(guest, Step(first), HttpStatusCode.Redirect);

            const string api = "/api/v1/InquiryAPI";
            await Expect(other, api + "/get-step1/1/fixture/fixture/legacy-fixture/fixture/fixture", HttpStatusCode.Gone);
            await Expect(other, api + "/get-step3/10/100/100/0/legacy-fixture", HttpStatusCode.Gone);
            await Expect(other, api + "/get-step4/legacy-fixture/all/1/1/1", HttpStatusCode.Gone);
            await Expect(other, api + "/get-brands", HttpStatusCode.OK);
            using var fresh = await other.GetAsync(Step(foreign));
            Check(fresh.Headers.CacheControl?.NoStore == true, "Inquiry pages must not be stored by intermediaries or browsers.");

            await InquiryBrowserChecks.RunAsync(origin, state, city, glass);
            Console.WriteLine("PASS: HTTP two sessions/tabs, login claim, foreign draft rejection, logout, role redirects, antiforgery, no-store and retired API.");
        }
        finally
        {
            if (!app.HasExited) app.Kill(entireProcessTree: true);
            await app.WaitForExitAsync();
            await Task.WhenAll(stdout, stderr);
        }

        async Task<string> Start(HttpClient browser)
        {
            var html = await browser.GetStringAsync("/InquiryProduct/InquiryStep1");
            using var response = await browser.PostAsync("/InquiryProduct/InquiryStep2", Form(
                ("__RequestVerificationToken", ReadToken(html)),
                ("StateId", state.ToString(CultureInfo.InvariantCulture)),
                ("CityId", city.ToString(CultureInfo.InvariantCulture)),
                ("GlassId", glass.ToString(CultureInfo.InvariantCulture)),
                ("SellerType", ((int)SellerType.Aluminium).ToString(CultureInfo.InvariantCulture))));
            Check(response.IsSuccessStatusCode, $"Create draft returned {response.StatusCode}.");
            var result = await response.Content.ReadAsStringAsync();
            var token = Regex.Match(result, "web_[a-f0-9]{32}").Value;
            Check(token.Length > 0, "Brand screen must carry the created draft.");
            return token;
        }
    }

    private static HttpClient Client(Uri origin) => new(new HttpClientHandler { AllowAutoRedirect = false })
    {
        BaseAddress = origin, Timeout = TimeSpan.FromSeconds(20)
    };

    private static string Step(string token) => "/InquiryProduct/InquiryStep3?userMacAddress=" + token;

    private static string ReadToken(string html)
    {
        var token = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value;
        Check(token.Length > 0, "Missing antiforgery field.");
        return WebUtility.HtmlDecode(token);
    }

    private static FormUrlEncodedContent Form(params (string Name, string Value)[] fields) =>
        new(fields.Select(field => new KeyValuePair<string, string>(field.Name, field.Value)));

    private static async Task Expect(HttpClient client, string path, HttpStatusCode status)
    {
        using var response = await client.GetAsync(path);
        Check(response.StatusCode == status, $"Expected {status}, got {response.StatusCode} for {path.Split('?')[0]}.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
