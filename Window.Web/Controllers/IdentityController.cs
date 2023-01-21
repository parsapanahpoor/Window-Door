using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Localization;
using NuGet.Protocol.Plugins;
using Window.Application.Interfaces;
using Window.Domain.ViewModels.Account;
using Window.Domain.ViewModels.Site.Account;
using Window.Domain.ViewModels.User.Authentication;
using Window.Web.HttpManager;

namespace Window.Web.Controllers
{
    public class IdentityController : Controller
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

            if (await _userService.IsExistUserByMobile(Mobile) == false)
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

            #region Get User By User ID

            var user = await _userService.GetUserByEmail(Mobile);

            #endregion

            #region Time Counter Initilize


            var SiteSettingSMSTimer = 2;

            DateTime expireMinut = user.ExpireMobileSMSDateTime.Value.AddMinutes(SiteSettingSMSTimer);

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
                        TempData["success"] = _localizer["ثبت نام شما باموفقیت انجام شده است ."].Value;
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
        public IActionResult Login(string? returnUrl = "/")
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        [RedirectIfLoggedInActionFilter]
        public async Task<IActionResult> Login(LoginUserViewModel model, string? returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var canUserLogin = await _userService.LoginUserAsync(model);

            if (canUserLogin != LoginUserResponse.Success)
            {
                switch (canUserLogin)
                {
                    case LoginUserResponse.EmailNotFound:
                        ModelState.AddModelError("Email", _localizer["موبایل وارد شده صحیح نمی باشد."]);
                        break;
                    case LoginUserResponse.UserNotActive:
                        ModelState.AddModelError("Error", _localizer["Your Account Is Not Active"]);
                        return RedirectToAction("ActiveUserByMobileActivationCode", new { Mobile = model.Mobile, Resend = true });

                    case LoginUserResponse.WrongPassword:
                        ModelState.AddModelError("Password", _localizer["Wrong Password"]);
                        break;
                    default:
                        ModelState.AddModelError("Error", _localizer["An error occurred in the login process"]);
                        break;
                }

                return View(model);
            }

            var user = await _userService.GetUserByEmail(model.Mobile);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.MobilePhone, user.Mobile),
                new (ClaimTypes.Name, user.Username),
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimIdentity);

            var authProps = new AuthenticationProperties();
            authProps.IsPersistent = model.RememberMe;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
            return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
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
        public async Task<IActionResult> ResetPassword(string mobile, bool resend)
        {
            #region Send Model To View

            ViewBag.Mobile = mobile;

            var user = await _userService.GetUserByEmail(mobile);

            if (user == null) return NotFound();

            if (resend)
            {
                await _userService.ResendActivationCodeSMS(mobile);
            }

            var SiteSettingSMSTimer = 2;

            DateTime expireMinut = user.ExpireMobileSMSDateTime.Value.AddMinutes(SiteSettingSMSTimer);

            var TimerMinut = expireMinut - DateTime.Now;

            ViewBag.Time = TimerMinut.TotalMinutes * 60;

            #endregion

            return View();
        }

        [HttpPost("reset-pass/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string mobile, ResetPasswordViewModel reset)
        {
            #region Get User By Mobile 

            ViewBag.Mobile = mobile;

            var user = await _userService.GetUserByEmail(mobile);

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

            #region Resend Timer

            var SiteSettingSMSTimer = 2;

            if (SiteSettingSMSTimer == null)
            {
                ModelState.AddModelError("Mobile", _localizer["لطفا با پشتیبان تماس بگیرید ."]);
                return RedirectToAction(nameof(Login));
            }

            DateTime expireMinut = user.ExpireMobileSMSDateTime.Value.AddMinutes(SiteSettingSMSTimer);

            var TimerMinut = expireMinut - DateTime.Now;

            ViewBag.Time = TimerMinut.TotalMinutes * 60;

            #endregion

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
