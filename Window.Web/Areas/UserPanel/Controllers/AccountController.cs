
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Domain.ViewModels.User.Account;

namespace Window.Web.Areas.UserPanel.Controllers
{
    public class AccountController : UserPanelBaseController
    {
        #region ctor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Edit User

        [HttpGet]
        public async Task<IActionResult> EditProfile(bool FillInfo)
        {
            #region Fill View Model

            var result = await _userService.FillUserPanelEditUserInfoViewModel(User.GetUserId());

            if (result == null) return NotFound();

            #endregion

            #region Fill Personal Information

            if (FillInfo == true)
            {
                ViewBag.FillInfo = true;
            }

            #endregion

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserPanelEditUserInfoViewModel edit, IFormFile? UserAvatar)
        {
            #region Model State Validation

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";
                return View(edit);
            }

            if (edit.UserId != User.GetUserId())
            {
                TempData[ErrorMessage] = "کاربر مورد نظر یافت نشده است .";
                return View(edit);
            }

            #endregion

            #region Edit User Method

            var result = await _userService.EditUserInfoInUserPanel(edit, UserAvatar);

            switch (result)
            {
                case UserPanelEditUserInfoResult.NotValidImage:
                    TempData[ErrorMessage] = "تصویر وارد شده صحیح نمی باشد.";
                    break;
                case UserPanelEditUserInfoResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشده است.";
                    return RedirectToAction("EditProfile", "Account", new { area = "UserPanel" });
                case UserPanelEditUserInfoResult.Success:
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                    return RedirectToAction("Index", "Home", new { area = "UserPanel" });
                case UserPanelEditUserInfoResult.NotValidEmail:
                    TempData[ErrorMessage] = "ایمیل وارد شده صحیح نمی باشد.";
                    break;
                case UserPanelEditUserInfoResult.NationalId:
                    TempData[ErrorMessage] = "کدملی وارد شده صحیح نمی باشد.";
                    break;
                case UserPanelEditUserInfoResult.NotValidNationalId:
                    TempData[ErrorMessage] = "کدملی وارد شده در سامانه موجود است.";
                    break;
            }

            #endregion

            return View(edit);
        }

        #endregion

        #region Change Password

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordViewModel model)
        {
            #region Model State Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            #region change Password

            var result = await _userService.ChangeUserPasswordAsync(User.GetUserId(), model);

            switch (result)
            {
                case ChangeUserPasswordResponse.Success:
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                    return RedirectToAction("Index", "Home");
                    break;

                case ChangeUserPasswordResponse.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشده است.";
                    break;

                case ChangeUserPasswordResponse.WrongPassword:
                    TempData[ErrorMessage] = "کلمه ی عبور وارد شده صحیح نمی باشد.";
                    break;

                default:
                    TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
                    break;
            }

            #endregion

            return View(model);
        }

        #endregion
    }
}
