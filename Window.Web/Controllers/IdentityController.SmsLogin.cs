using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Window.Application.Services;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.User.Authentication;
using Window.Web.HttpManager;

namespace Window.Web.Controllers;

public partial class IdentityController
{
    public const string SmsLoginRateLimit = "sms-login";
    private const string SmsLoginSessionKey = "Window.SmsLogin";
    private const string SmsLoginErrorKey = "SmsLoginError";
    private const string SendError = "در حال حاضر امکان ارسال کد نیست. بعد از پایان زمان انتظار دوباره تلاش کن.";

    [HttpGet("login/sms")]
    [RedirectIfLoggedInActionFilter]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult SmsLogin(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = LocalReturnUrl(returnUrl);
        return View();
    }

    [HttpPost("login/sms")]
    [ValidateAntiForgeryToken]
    [RedirectIfLoggedInActionFilter]
    [EnableRateLimiting(SmsLoginRateLimit)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> SmsLogin(
        SmsLoginRequest model, [FromServices] SmsLoginService service, string? returnUrl, CancellationToken cancellationToken)
    {
        ViewData["ReturnUrl"] = LocalReturnUrl(returnUrl);
        if (!ModelState.IsValid)
            return View(model);

        var mobile = MobileNumber.Normalize(model.Mobile);
        if (mobile is null)
        {
            ModelState.AddModelError(nameof(model.Mobile), "شمارهٔ موبایل ایران را به‌درستی وارد کن.");
            return View(model);
        }

        var existing = ReadSmsLoginFlow();
        var previous = existing?.Mobile == mobile ? existing.ChallengeId : (Guid?)null;
        var result = await service.SendAsync(mobile, previous, cancellationToken);
        if (result.ChallengeId is not Guid id)
        {
            ModelState.AddModelError(string.Empty, SendError);
            return View(model);
        }

        WriteSmsLoginFlow(new SmsLoginFlow(mobile, id, LocalReturnUrl(returnUrl)));
        if (!result.Sent)
            TempData[SmsLoginErrorKey] = SendError;

        return RedirectToAction(nameof(SmsLoginCode));
    }

    [HttpGet("login/sms/code")]
    [RedirectIfLoggedInActionFilter]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> SmsLoginCode([FromServices] SmsLoginService service, CancellationToken cancellationToken)
    {
        var flow = ReadSmsLoginFlow();
        if (flow is null)
            return RedirectToAction(nameof(SmsLogin));

        if (TempData[SmsLoginErrorKey] is string error)
            ModelState.AddModelError(string.Empty, error);

        return View(await CreateCodeViewAsync(service, flow, cancellationToken));
    }

    [HttpPost("login/sms/code")]
    [ValidateAntiForgeryToken]
    [RedirectIfLoggedInActionFilter]
    [EnableRateLimiting(SmsLoginRateLimit)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> SmsLoginCode(
        SmsLoginCodeInput input, [FromServices] SmsLoginService service, CancellationToken cancellationToken)
    {
        var flow = ReadSmsLoginFlow();
        if (flow is null)
            return RedirectToAction(nameof(SmsLogin));

        if (ModelState.IsValid)
        {
            var user = await service.VerifyAsync(flow.Mobile, flow.ChallengeId, MobileNumber.ToAsciiDigits(input.Code), cancellationToken);
            if (user is not null)
            {
                HttpContext.Session.Remove(SmsLoginSessionKey);
                return await SignInUserAsync(user, false, flow.ReturnUrl, SmsLoginService.AuthenticationMethod);
            }

            ModelState.AddModelError(nameof(input.Code), "کد معتبر نیست یا مهلت آن تمام شده است.");
        }

        return View(await CreateCodeViewAsync(service, flow, cancellationToken));
    }

    [HttpPost("login/sms/resend")]
    [ValidateAntiForgeryToken]
    [RedirectIfLoggedInActionFilter]
    [EnableRateLimiting(SmsLoginRateLimit)]
    public async Task<IActionResult> ResendSmsLoginCode([FromServices] SmsLoginService service, CancellationToken cancellationToken)
    {
        var flow = ReadSmsLoginFlow();
        if (flow is null)
            return RedirectToAction(nameof(SmsLogin));

        var result = await service.SendAsync(flow.Mobile, flow.ChallengeId, cancellationToken);
        if (result.ChallengeId is Guid id && id != flow.ChallengeId)
            WriteSmsLoginFlow(flow with { ChallengeId = id });
        if (!result.Sent)
            TempData[SmsLoginErrorKey] = SendError;

        return RedirectToAction(nameof(SmsLoginCode));
    }

    private async Task<SmsLoginCodeViewModel> CreateCodeViewAsync(
        SmsLoginService service, SmsLoginFlow flow, CancellationToken cancellationToken)
    {
        var seconds = await service.SecondsUntilResendAsync(flow.Mobile, flow.ChallengeId, cancellationToken);
        return new SmsLoginCodeViewModel(flow.Mobile[..4] + "••••" + flow.Mobile[^3..], seconds, flow.ReturnUrl);
    }

    private SmsLoginFlow? ReadSmsLoginFlow() =>
        HttpContext.Session.GetString(SmsLoginSessionKey) is { } json
            ? JsonSerializer.Deserialize<SmsLoginFlow>(json)
            : null;

    private void WriteSmsLoginFlow(SmsLoginFlow flow) =>
        HttpContext.Session.SetString(SmsLoginSessionKey, JsonSerializer.Serialize(flow));

    private sealed record SmsLoginFlow(string Mobile, Guid ChallengeId, string? ReturnUrl);
}
