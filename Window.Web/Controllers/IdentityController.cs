using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Window.Application.Interfaces;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.Account;
using Window.Domain.ViewModels.Site.Account;
using Window.Domain.ViewModels.User.Authentication;
using Window.Web.HttpManager;

namespace Window.Web.Controllers
{
    public partial class IdentityController : Controller
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly IStringLocalizer<IdentityController> _localizer;

        public IdentityController(IUserService userService, IStringLocalizer<IdentityController> localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

        #endregion

        #region Register

        [HttpGet("register")]
        [RedirectIfLoggedInActionFilter]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        [RedirectIfLoggedInActionFilter]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var mobile = MobileNumber.Normalize(model.Mobile);
            if (mobile is null)
            {
                ModelState.AddModelError(nameof(model.Mobile), "شمارهٔ موبایل ایران را به‌درستی وارد کن.");
                return View(model);
            }

            model.Mobile = mobile;
            var result = await _userService.RegisterUserAsync(model);

            if (result == RegisterUserResponse.Success)
            {
                TempData["success"] = _localizer["ثبت نام شما باموفقیت انجام شده است ."].Value;
                return RedirectToAction("ActiveUserByMobileActivationCode", new { Mobile = model.Mobile });
            }

            switch (result)
            {
                case RegisterUserResponse.EmailExist:
                    ModelState.AddModelError("Mobile", _localizer["Mobile Exist"]);
                    break;
                case RegisterUserResponse.MobileExist:
                    ModelState.AddModelError("Mobile", _localizer["Mobile Exist"]);
                    break;
                default:
                    ModelState.AddModelError("Error", _localizer["An error occurred in the register process"]);
                    break;
            }

            return View(model);
        }

        #endregion

        #region Active mobile user

        [HttpGet("Active-mobile/{Mobile}/{Resend?}")]
        public async Task<IActionResult> ActiveUserByMobileActivationCode(string Mobile, bool Resend = false)
        {
            #region Model State Validation 

            if (Mobile == null)
            {
                return NotFound();
            }

            #endregion

            #region Is Exist User 

            var user = await _userService.GetMobileCodeAccountAsync(Mobile);
            if (user is null || user.IsDelete || user.IsBan)
            {
                return NotFound();
            }

            #endregion

            #region Resend SMS

            if (Resend)
            {
                await _userService.ResendActivationCodeSMS(Mobile);
            }

            #endregion


            #region Time Counter Initilize


            var SiteSettingSMSTimer = 2;

            DateTime expireMinut = user.ExpireMobileSMSDateTime?.AddMinutes(SiteSettingSMSTimer) ?? DateTime.Now;

            var TimerMinut = expireMinut - DateTime.Now;

            ViewBag.Time = TimerMinut.TotalMinutes * 60;

            ViewBag.Mobile = Mobile;

            #endregion

            return View();
        }

        [HttpPost("Active-mobile/{Mobile}/{Resend?}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveUserByMobileActivationCode(ActiveMobileByActivationCodeViewModel activeMobileByActivationCodeViewModel)
        {
            #region Active User Mobile

            if (ModelState.IsValid)
            {
                var result = await _userService.ActiveUserMobile(activeMobileByActivationCodeViewModel);
                switch (result)
                {
                    case ActiveMobileByActivationCodeResult.Success:
                        TempData["success"] = _localizer["فعال سازی حساب کاربری شما با موفقیت انجام شده است."].Value;
                        return RedirectToAction(nameof(Login));

                    case ActiveMobileByActivationCodeResult.AccountNotFound:
                        ModelState.AddModelError("Error", _localizer["کاربر مورد نظر یافت نشده است."]);
                        return RedirectToAction("ActiveUserByMobileActivationCode", new { Mobile = activeMobileByActivationCodeViewModel.Mobile, Resend = false });
                }
            }

            #endregion

            return View(activeMobileByActivationCodeViewModel);
        }

        #endregion

        #region Login

        [HttpGet("login")]
        [RedirectIfLoggedInActionFilter]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Login(string? returnUrl = null) => ShowSmsLogin(returnUrl);

        [HttpGet("login/password")]
        [RedirectIfLoggedInActionFilter]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult PasswordLogin(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = LocalReturnUrl(returnUrl);
            if (TempData[PasswordFallbackKey] is string message)
                ModelState.AddModelError(string.Empty, message);

            return View("Login");
        }

        [HttpPost("login/password")]
        [HttpPost("login", Order = 1)]
        [ValidateAntiForgeryToken]
        [RedirectIfLoggedInActionFilter]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> PasswordLogin(
            LoginUserViewModel model, string? returnUrl, CancellationToken cancellationToken)
        {
            ViewData["ReturnUrl"] = LocalReturnUrl(returnUrl);
            if (!ModelState.IsValid)
                return View("Login", model);

            var (status, account) = await _userService.LoginUserAsync(model, cancellationToken);
            if (status == LoginUserResponse.Success && account is not null)
                return await SignInUserAsync(account, model.RememberMe, returnUrl);

            if (status == LoginUserResponse.UserNotActive && account is not null)
                return RedirectToAction(nameof(ActiveUserByMobileActivationCode), new { Mobile = account.Mobile, Resend = true });

            ModelState.AddModelError(string.Empty, status == LoginUserResponse.Unavailable
                ? "ورود به این حساب امکان‌پذیر نیست."
                : "شمارهٔ موبایل یا کلمهٔ عبور درست نیست.");

            return View("Login", model);
        }

        private string LocalReturnUrl(string? returnUrl) =>
            Url.IsLocalUrl(returnUrl) ? returnUrl! : "/";

        private async Task<IActionResult> SignInUserAsync(
            User user, bool rememberMe, string? returnUrl, string? authenticationMethod = null)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.MobilePhone, user.Mobile),
                new (ClaimTypes.Name, user.Username),
            };

            if (authenticationMethod is not null)
                claims.Add(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = rememberMe });
            return LocalRedirect(LocalReturnUrl(returnUrl));
        }

        #endregion

        #region Forgot Password

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordResult.VerificationSmsFaildFromParsGreen:
                        ModelState.AddModelError("Mobile", _localizer[" کد تایید جدید برای شما ارسال نشد!"]);
                        ModelState.AddModelError("Mobile", _localizer[" لطفا با پشتیبانی سایت تماس بگیرید!"]);
                        break;
                    case ForgotPasswordResult.NotFound:
                        ModelState.AddModelError("Mobile", _localizer["کاربر مورد نظر یافت نشد"]);
                        break;
                    case ForgotPasswordResult.FailSendEmail:
                        ModelState.AddModelError("Mobile", _localizer["در ارسال موبایل مشکلی رخ داد"]);
                        break;
                    case ForgotPasswordResult.UserIsBlocked:
                        ModelState.AddModelError("Mobile", _localizer["حساب کاربری شما بسته شده است!"]);
                        break;
                    case ForgotPasswordResult.SuccessSendEmail:
                        ModelState.AddModelError("Mobile", _localizer["کد جدید برای شما ارسال شد"]);
                        return RedirectToAction("ResetPassword", "Identity", new { mobile = forgot.Mobile });
                    case ForgotPasswordResult.Success:
                        ModelState.AddModelError("Mobile", _localizer["کد تایید جدید برای شما ارسال شد"]);
                        return RedirectToAction("ResetPassword", "Identity", new { mobile = forgot.Mobile });
                }
            }

            return View(forgot);
        }

        #endregion

        #region Reset Password

        [HttpGet("reset-pass/{mobile}")]
        public async Task<IActionResult> ResetPassword(string mobile)
        {
            #region Send Model To View

            var user = await _userService.GetMobileCodeAccountAsync(mobile);

            if (user == null) return NotFound();

            var expiresAt = user.ExpireMobileSMSDateTime?.AddMinutes(2) ?? DateTime.Now;
            ViewBag.Time = Math.Max(0, (expiresAt - DateTime.Now).TotalSeconds);

            ViewBag.Mobile = mobile;

            #endregion

            return View();
        }

        [HttpPost("reset-pass/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string mobile, ResetPasswordViewModel reset)
        {
            #region Get User By Mobile 

            ViewBag.Mobile = mobile;

            var user = await _userService.GetMobileCodeAccountAsync(mobile);
            if (user is null)
                return NotFound();

            #endregion

            if (ModelState.IsValid)
            {
                var res = await _userService.ResetUserPassword(reset, mobile);
                switch (res)
                {
                    case ResetPasswordResult.NotFound:
                        ModelState.AddModelError("ActiveCode", _localizer["کاربری با مشخصات وارد شده یافت نشد"]);
                        return Redirect("/");
                    case ResetPasswordResult.WrongActiveCode:
                        ModelState.AddModelError("ActiveCode", _localizer["کد تایید وارد شده صحیح نمی باشد"]);
                        break;
                    case ResetPasswordResult.Success:
                        ModelState.AddModelError("Mobile", _localizer["کلمه عبور شما با موفقیت تغییر پیدا کرد"]);
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Identity", new { area = "" });
                }
            }

            var expiresAt = user.ExpireMobileSMSDateTime?.AddMinutes(2) ?? DateTime.Now;
            ViewBag.Time = Math.Max(0, (expiresAt - DateTime.Now).TotalSeconds);

            return View(reset);
        }

        #endregion

        #region Logout

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion
    }
}
