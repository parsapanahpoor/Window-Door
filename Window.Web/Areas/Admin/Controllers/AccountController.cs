using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Domain.ViewModels.User.Account;
using Window.Web.Areas.User.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Window.Web.Areas.Seller.Controllers;
using Window.Application.Security;

namespace Window.Web.Areas.User.Controllers;

[PermissionChecker("ManageUsers")]
public class AccountController : SellerBaseController
{
    #region ctor

    private readonly IUserService _userService;
    private readonly IStringLocalizer<AccountController> _localizer;
    private readonly IStringLocalizer<SharedLocalizer.SharedLocalizer> _sharedLocalizer;

    public AccountController(IUserService userService, IStringLocalizer<AccountController> localizer, IStringLocalizer<SharedLocalizer.SharedLocalizer> sharedLocalizer)
    {
        _userService = userService;
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer;
    }

    #endregion

    #region Edit Profile

    public async Task<IActionResult> EditProfile()
    {
        var userProfile = (await _userService.GetUserProfileForEditAsync(User.GetUserId()))!;
        return View(userProfile);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _userService.EditProfileAsync(User.GetUserId(), model);

        switch (result)
        {
            case EditProfileResponse.Success:
                TempData[SwalSuccess] = _sharedLocalizer["Operation Successfully"].Value;
                return RedirectToAction("Index", "Home");
                break;

            case EditProfileResponse.UserNotFound:
                TempData[SwalError] = _localizer["Your account not found, please re login"].Value;
                break;

            case EditProfileResponse.ImageIsNotValid:
                TempData[SwalError] = _localizer["Uploaded image is not valid, please select another image"].Value;
                break;

            default:
                TempData[SwalError] = _localizer["An error occurred in the edit profile process"].Value;
                break;
        }


        return RedirectToAction("EditProfile");
    }

    #endregion

    #region Change Password

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _userService.ChangeUserPasswordAsync(User.GetUserId(), model);

        switch (result)
        {
            case ChangeUserPasswordResponse.Success:
                TempData[SwalSuccess] = _sharedLocalizer["Operation Successfully"].Value;
                return RedirectToAction("Index", "Home");
                break;

            case ChangeUserPasswordResponse.UserNotFound:
                TempData[SwalError] = _localizer["Your account not found, please re login"].Value;
                break;

            case ChangeUserPasswordResponse.WrongPassword:
                TempData[SwalError] = _localizer["Current password is invalid"].Value;
                break;

            default:
                TempData[SwalError] = _localizer["An error occurred in the change password process"].Value;
                break;
        }


        return View(model);
    }

    #endregion
}
