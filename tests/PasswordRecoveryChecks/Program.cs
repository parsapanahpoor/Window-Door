using System.Net;
using System.Reflection;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Window.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Window.Application.Services;
using Window.Application.Services.Implementation;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Account;
using Window.Web.Controllers;

const string Mobile = "09100000001";
var model = new ForgetPasswordViewModel { Mobile = Mobile };
var user = new User
{
    Mobile = Mobile,
    MobileActivationCode = "12345",
    ExpireMobileSMSDateTime = DateTime.Now.AddMinutes(-3)
};
var previousTime = user.ExpireMobileSMSDateTime;

var repository = DispatchProxy.Create<IUserRepository, RecoveryRepository>();
var repositoryDouble = (RecoveryRepository)(object)repository;
repositoryDouble.User = user;
await using var database = new RecoveryDatabase();
using var transport = new RecoveryTransport();
using var http = new HttpClient(transport) { BaseAddress = new Uri("https://example.invalid/v1/") };
var settings = new ConfigurationBuilder().AddInMemoryCollection().Build();
settings["kavenegar:apikey"] = "legacy-offline-test-key";
var sms = new SMSService(http, settings, NullLogger<SMSService>.Instance);
var service = new UserService(settings, repository, null!, database, null!, null!, sms);
var localizer = new StringLocalizer<IdentityController>(
    new ResourceManagerStringLocalizerFactory(
        Microsoft.Extensions.Options.Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance));
var controller = new IdentityController(service, localizer);

async Task CheckFailure()
{
    Check(await controller.ForgotPassword(model) is ViewResult, "Recovery failure must render the form.");
    Check(controller.ModelState.ErrorCount > 0, "Recovery failure must show a validation message.");
    Check(database.SaveCount == 0 && user.MobileActivationCode == "12345" &&
        user.ExpireMobileSMSDateTime == previousTime, "Failure must preserve the previous code and timestamp.");
    controller.ModelState.Clear();
}

await CheckFailure();
Check(transport.Calls == 0, "Missing configuration must not call the provider.");

settings["kavenegar:recoveryApiKey"] = "offline-test-key";
transport.Reply = () => Response(416, """{"return":{"status":416},"entries":[]}""");
await CheckFailure();
transport.Reply = () => Response(426, """{"return":{"status":426},"entries":[]}""");
await CheckFailure();
transport.Reply = () => Response(200, """{"return":{"status":424},"entries":[]}""");
await CheckFailure();
transport.Reply = () => Response(200, """{"return":{"status":200},"entries":[{"messageid":42,"status":6}]}""");
await CheckFailure();
transport.Reply = () => Response(502, "Invalid gateway response");
await CheckFailure();
transport.Reply = () => throw new HttpRequestException("Simulated network failure");
await CheckFailure();
transport.Reply = () => throw new TaskCanceledException("Simulated timeout");
await CheckFailure();

int callsBeforeAccountChecks = transport.Calls;
user.IsBan = true;
Check(await service.RecoverUserPassword(model) == ForgotPasswordResult.UserIsBlocked, "Blocked account must fail.");
user.IsBan = false;
user.IsDelete = true;
Check(await service.RecoverUserPassword(model) == ForgotPasswordResult.NotFound, "Deleted account must fail.");
user.IsDelete = false;
repositoryDouble.User = null;
Check(await service.RecoverUserPassword(model) == ForgotPasswordResult.NotFound, "Missing account must fail.");
repositoryDouble.User = user;
Check(transport.Calls == callsBeforeAccountChecks && database.SaveCount == 0,
    "Ineligible accounts must not call the provider or save a code.");

var providerResponse = new TaskCompletionSource<HttpResponseMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
transport.ReplyAsync = () => providerResponse.Task;
var request = controller.ForgotPassword(model);
Check(!request.IsCompleted && database.SaveCount == 0, "Recovery must wait for provider acceptance.");
providerResponse.SetResult(Response(200, """{"return":{"status":200},"entries":[{"messageid":42,"status":1}]}"""));
var result = await request;
Check(result is RedirectToActionResult { ActionName: nameof(IdentityController.ResetPassword) },
    "An accepted and saved SMS must redirect to reset.");
Check(database.SaveCount == 1 && user.ExpireMobileSMSDateTime > previousTime,
    "Success must persist a new issuance time.");
var fields = System.Web.HttpUtility.ParseQueryString(transport.LastForm!);
Check(fields["receptor"] == Mobile && fields["template"] == "Register" &&
    fields["token"] == user.MobileActivationCode, "The submitted code must match the saved code.");
Check(transport.LastUri?.AbsoluteUri == "https://example.invalid/v1/offline-test-key/verify/lookup.json",
    "The lookup request must use the expected HTTPS route.");
Check(await controller.ResetPassword(Mobile) is ViewResult && transport.Calls == callsBeforeAccountChecks + 1,
    "Loading the reset page must not send another SMS.");
Check(typeof(IdentityController).GetMethod(nameof(IdentityController.ForgotPassword), [typeof(ForgetPasswordViewModel)])!
    .IsDefined(typeof(ValidateAntiForgeryTokenAttribute)), "Recovery POST must require an antiforgery token.");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(IdentityController).Assembly.GetName().Name
});
builder.WebHost.UseUrls("http://127.0.0.1:0");
builder.Logging.ClearProviders();
builder.Services.AddSingleton<IUserService>(service);
builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
builder.Services.AddControllersWithViews().AddViewLocalization();
await using var app = builder.Build();
app.MapControllers();
await app.StartAsync();

using var handler = new HttpClientHandler { AllowAutoRedirect = false, UseProxy = false };
using var browser = new HttpClient(handler) { BaseAddress = new Uri(app.Urls.Single()) };
var page = new HtmlDocument();
page.LoadHtml(await browser.GetStringAsync($"/reset-pass/{Mobile}"));
var retry = page.DocumentNode.SelectSingleNode("//form[@id='resendPasswordCode']");
var action = retry?.GetAttributeValue("action", "");
Check(action == "/ForgotPassword" && retry?.GetAttributeValue("method", "") == "post",
    "The rendered retry form must post to the recovery endpoint.");
var button = page.DocumentNode.SelectSingleNode("//button[@id='SMS']");
Check(button?.GetAttributeValue("form", "") == "resendPasswordCode", "The retry button must submit the retry form.");
var retryMobile = retry!.SelectSingleNode("input[@name='Mobile']").GetAttributeValue("value", "");
var csrf = retry.SelectSingleNode("input[@name='__RequestVerificationToken']")?.GetAttributeValue("value", "");
Check(retryMobile == Mobile && !string.IsNullOrEmpty(csrf), "The retry form must include the mobile and antiforgery token.");
Check(transport.Calls == callsBeforeAccountChecks + 1, "Rendering the reset page must not call the provider.");

using var missingToken = new FormUrlEncodedContent(new Dictionary<string, string> { ["Mobile"] = retryMobile });
using var rejected = await browser.PostAsync(action, missingToken);
Check(rejected.StatusCode == HttpStatusCode.BadRequest && transport.Calls == callsBeforeAccountChecks + 1,
    "A retry without the antiforgery token must be rejected before sending.");

transport.ReplyAsync = null;
transport.Reply = () => Response(200, """{"return":{"status":200},"entries":[{"messageid":43,"status":1}]}""");
using var retryForm = new FormUrlEncodedContent(new Dictionary<string, string>
{
    ["Mobile"] = retryMobile,
    ["__RequestVerificationToken"] = csrf!
});
using var accepted = await browser.PostAsync(action, retryForm);
Check(accepted.StatusCode == HttpStatusCode.Redirect &&
    accepted.Headers.Location?.OriginalString == $"/reset-pass/{Mobile}" &&
    transport.Calls == callsBeforeAccountChecks + 2 && database.SaveCount == 2,
    "A valid retry must save an accepted code and redirect back to the reset page.");
await app.StopAsync();

Console.WriteLine("PASS: recovery acceptance, rejection, timeout, account checks, and reset form retry with antiforgery.");

static HttpResponseMessage Response(int status, string json) => new((HttpStatusCode)status)
{
    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
};

static void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

public class RecoveryRepository : DispatchProxy
{
    public User? User { get; set; }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod?.Name == nameof(IUserRepository.GetUserByEmail)) return Task.FromResult(User);
        throw new NotSupportedException(targetMethod?.Name);
    }
}

sealed class RecoveryDatabase() : WindowDbContext(new DbContextOptions<WindowDbContext>())
{
    public int SaveCount { get; private set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveCount++;
        return Task.FromResult(1);
    }
}

sealed class RecoveryTransport : HttpMessageHandler
{
    public int Calls { get; private set; }
    public string? LastForm { get; private set; }
    public Uri? LastUri { get; private set; }
    public Func<HttpResponseMessage>? Reply { get; set; }
    public Func<Task<HttpResponseMessage>>? ReplyAsync { get; set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        Calls++;
        LastUri = request.RequestUri;
        LastForm = await request.Content!.ReadAsStringAsync(token);
        return ReplyAsync == null ? Reply!() : await ReplyAsync();
    }
}
