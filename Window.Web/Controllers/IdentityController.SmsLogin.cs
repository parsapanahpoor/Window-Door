using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Window.Application.Services;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.User.Authentication;
using Window.Web.HttpManager;

namespace Window.Web.Controllers;

public partial class IdentityController
{
    public const string SmsLoginRateLimit = "sms-login";
    private const string SmsLoginSessionKey = "Window.SmsLogin";
    private const string SmsRegistrationSessionKey = "Window.SmsRegistration";
    private const string SmsLoginErrorKey = "SmsLoginError";
    private const string PasswordFallbackKey = "PasswordFallback";
    private const string SendError = "در حال حاضر امکان ارسال کد نیست. بعد از پایان زمان انتظار دوباره تلاش کن.";

    [HttpGet("login/sms")]
    [RedirectIfLoggedInActionFilter]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult SmsLogin(string? returnUrl = null) => ShowSmsLogin(returnUrl);

    private IActionResult ShowSmsLogin(string? returnUrl)
    {
        ViewData["ReturnUrl"] = LocalReturnUrl(returnUrl);
        return View(nameof(SmsLogin));
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

        // A new number must choose customer or seller before any message is sent.
        if (!await service.AccountExistsAsync(mobile, cancellationToken))
        {
            WriteSmsRegistrationFlow(new SmsRegistrationFlow(mobile, "role", null, LocalReturnUrl(returnUrl)));
            return View("SmsRegisterRole", new SmsRegisterViewModel
            {
                Mobile = mobile,
                ReturnUrl = LocalReturnUrl(returnUrl)
            });
        }

        var existing = ReadSmsLoginFlow();
        var previous = existing?.Mobile == mobile ? existing.ChallengeId : (Guid?)null;
        var result = await service.SendAsync(mobile, previous, cancellationToken);
        if (result.RequiresPassword)
            return UsePasswordInstead(returnUrl);
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
            var result = await service.VerifyAsync(flow.Mobile, flow.ChallengeId, MobileNumber.ToAsciiDigits(input.Code), cancellationToken);
            if (result.RequiresPassword)
                return UsePasswordInstead(flow.ReturnUrl);

            if (result.Account is { } user)
            {
                HttpContext.Session.Remove(SmsLoginSessionKey);
                ClearSmsRegistrationFlow();
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
        if (result.RequiresPassword)
            return UsePasswordInstead(flow.ReturnUrl);
        if (result.ChallengeId is Guid id && id != flow.ChallengeId)
            WriteSmsLoginFlow(flow with { ChallengeId = id });
        if (!result.Sent)
            TempData[SmsLoginErrorKey] = SendError;

        return RedirectToAction(nameof(SmsLoginCode));
    }

    private IActionResult UsePasswordInstead(string? returnUrl)
    {
        HttpContext.Session.Remove(SmsLoginSessionKey);
        TempData.Remove(SmsLoginErrorKey);
        TempData[PasswordFallbackKey] = "ورود پیامکی با این شمارهٔ موبایل امکان‌پذیر نیست. اگر حساب فعال داری، با کلمهٔ عبور وارد شو.";
        return RedirectToAction(nameof(PasswordLogin), new { returnUrl = LocalReturnUrl(returnUrl) });
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

    #region Sms registration

    [HttpGet("login/sms/register")]
    [RedirectIfLoggedInActionFilter]
    public IActionResult SmsRegistrationRole()
    {
        var flow = ReadSmsRegistrationFlow();
        if (flow is null) return RedirectToAction(nameof(SmsLogin));

        return View("SmsRegisterRole", new SmsRegisterViewModel
        {
            Mobile = flow.Mobile,
            ReturnUrl = flow.ReturnUrl
        });
    }

    [HttpPost("login/sms/register")]
    [ValidateAntiForgeryToken]
    [RedirectIfLoggedInActionFilter]
    [EnableRateLimiting(SmsLoginRateLimit)]
    public async Task<IActionResult> SmsRegistrationRole(
        AccountType accountType, [FromServices] SmsLoginService service, CancellationToken cancellationToken)
    {
        var flow = ReadSmsRegistrationFlow();
        if (flow is null) return RedirectToAction(nameof(SmsLogin));

        if (accountType is not (AccountType.Customer or AccountType.Seller))
        {
            ModelState.AddModelError(nameof(accountType), "نوع حساب را انتخاب کنید.");
            return View("SmsRegisterRole", new SmsRegisterViewModel { Mobile = flow.Mobile, ReturnUrl = flow.ReturnUrl });
        }

        var account = await _userService.CreatePendingSmsAccountAsync(flow.Mobile, accountType == AccountType.Seller, cancellationToken);
        if (account is null || account.IsMobileConfirm)
            return RedirectToAction(nameof(SmsLogin));

        if (accountType == AccountType.Seller)
        {
            WriteSmsRegistrationFlow(flow with { Step = "store", UserId = account.Id });
            return View("SmsRegisterStore", new SmsRegisterViewModel { Mobile = flow.Mobile, ReturnUrl = flow.ReturnUrl });
        }

        var result = await service.SendAsync(flow.Mobile, null, cancellationToken);
        if (result.ChallengeId is Guid challengeId)
        {
            WriteSmsLoginFlow(new SmsLoginFlow(flow.Mobile, challengeId, flow.ReturnUrl));
            ClearSmsRegistrationFlow();
            return RedirectToAction(nameof(SmsLoginCode));
        }

        ModelState.AddModelError(string.Empty, SendError);
        return View("SmsRegisterRole", new SmsRegisterViewModel { Mobile = flow.Mobile, ReturnUrl = flow.ReturnUrl });
    }

    [HttpGet("login/sms/register/store")]
    [RedirectIfLoggedInActionFilter]
    public IActionResult SmsRegistrationStore()
    {
        var flow = ReadSmsRegistrationFlow();
        if (flow is null || flow.Step != "store" || flow.UserId is null)
            return RedirectToAction(nameof(SmsLogin));

        return View("SmsRegisterStore", new SmsRegisterViewModel { Mobile = flow.Mobile, ReturnUrl = flow.ReturnUrl });
    }

    [HttpPost("login/sms/register/store")]
    [ValidateAntiForgeryToken]
    [RedirectIfLoggedInActionFilter]
    [EnableRateLimiting(SmsLoginRateLimit)]
    public async Task<IActionResult> SmsRegistrationStore(
        string? marketName, [FromServices] SmsLoginService service,
        [FromServices] IMarketService markets, CancellationToken cancellationToken)
    {
        var flow = ReadSmsRegistrationFlow();
        if (flow is null || flow.Step != "store" || flow.UserId is not ulong userId)
            return RedirectToAction(nameof(SmsLogin));

        if (!markets.IsValidMarketName(marketName))
        {
            ModelState.AddModelError(nameof(marketName), "نام فروشگاه باید بین ۳ تا ۱۰۰ کاراکتر باشد.");
            return View("SmsRegisterStore", new SmsRegisterViewModel
            {
                Mobile = flow.Mobile,
                ReturnUrl = flow.ReturnUrl,
                MarketName = marketName
            });
        }

        var account = await _userService.GetUserById(userId);
        if (account is null || account.IsDelete || account.IsBan || account.IsMobileConfirm)
            return RedirectToAction(nameof(SmsLogin));

        var created = await markets.AddMarket(userId, marketName!);
        if (!created)
        {
            // A retry after a partial failure must continue with the store this user already owns.
            var existing = await markets.GetMarketByUserId(userId);
            if (existing is null)
            {
                ModelState.AddModelError(nameof(marketName), "ثبت فروشگاه انجام نشد. دوباره تلاش کنید.");
                return View("SmsRegisterStore", new SmsRegisterViewModel
                {
                    Mobile = flow.Mobile,
                    ReturnUrl = flow.ReturnUrl,
                    MarketName = marketName
                });
            }
        }

        var result = await service.SendAsync(flow.Mobile, null, cancellationToken);
        if (result.ChallengeId is Guid challengeId)
        {
            WriteSmsLoginFlow(new SmsLoginFlow(flow.Mobile, challengeId, flow.ReturnUrl));
            ClearSmsRegistrationFlow();
            return RedirectToAction(nameof(SmsLoginCode));
        }

        ModelState.AddModelError(string.Empty, SendError);
        return View("SmsRegisterStore", new SmsRegisterViewModel
        {
            Mobile = flow.Mobile,
            ReturnUrl = flow.ReturnUrl,
            MarketName = marketName
        });
    }

    private SmsRegistrationFlow? ReadSmsRegistrationFlow() =>
        HttpContext.Session.GetString(SmsRegistrationSessionKey) is { } json
            ? JsonSerializer.Deserialize<SmsRegistrationFlow>(json)
            : null;

    private void WriteSmsRegistrationFlow(SmsRegistrationFlow flow) =>
        HttpContext.Session.SetString(SmsRegistrationSessionKey, JsonSerializer.Serialize(flow));

    private void ClearSmsRegistrationFlow() =>
        HttpContext.Session.Remove(SmsRegistrationSessionKey);

    #endregion

    private sealed record SmsLoginFlow(string Mobile, Guid ChallengeId, string? ReturnUrl);

    private sealed record SmsRegistrationFlow(string Mobile, string Step, ulong? UserId, string? ReturnUrl);
}
