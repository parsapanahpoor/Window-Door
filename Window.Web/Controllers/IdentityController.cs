using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Localization;
using Window.Application.Interfaces;
using Window.Domain.ViewModels.User.Authentication;
using Window.Web.HttpManager;

namespace Window.Web.Controllers
{
    public class IdentityController : Controller
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly IStringLocalizer<IdentityController> _localizer;

        public IdentityController( IUserService userService, IStringLocalizer<IdentityController> localizer)
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
                return RedirectToAction("Login");
            }

            switch (result)
            {
                case RegisterUserResponse.EmailExist:
                    ModelState.AddModelError("Email", _localizer["Email Exist"]);
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
                        ModelState.AddModelError("Email", _localizer["Email Not Found"]);
                        break;
                    case LoginUserResponse.UserNotActive:
                        ModelState.AddModelError("Error", _localizer["Your Account Is Not Active"]);
                        break;
                    case LoginUserResponse.WrongPassword:
                        ModelState.AddModelError("Password", _localizer["Wrong Password"]);
                        break;
                    default:
                        ModelState.AddModelError("Error", _localizer["An error occurred in the login process"]);
                        break;
                }

                return View(model);
            }

            var user = await _userService.GetUserByEmail(model.Email);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email),
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
