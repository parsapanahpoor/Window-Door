using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Domain.ViewModels;
using Window.Domain.ViewModels.Admin.Wallet;
using Window.Domain.ViewModels.User;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageUsers")]
    public class UserController : AdminBaseController
    {
        #region cunstructor
        private readonly IUserService _userService;
        private readonly IStringLocalizer<UserController> _localizer;
        private readonly IStringLocalizer<SharedLocalizer.SharedLocalizer> _sharedLocalizer;

        public UserController(IUserService userService, IStringLocalizer<UserController> localizer, IStringLocalizer<SharedLocalizer.SharedLocalizer> sharedLocalizer)
        {
            _userService = userService;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        #endregion

        #region User

        #region usersList

        public async Task<IActionResult> Index(FilterUserViewModel filter)
        {
            var date = DateTime.Now;
            var a = await _userService.FilterUsers(filter);
            return View(a);
        }

        #endregion

        #region User Profile

        public async Task<IActionResult> Profile(ulong id)
        {

            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                TempData[ErrorMessage] = _sharedLocalizer["User not found"].Value;
                return RedirectToAction("Index");
            }

            return View(user);
        }

        #endregion

        #region create new user

        public async Task<IActionResult> AddNewUser()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewUser(AddUserViewModel user, IFormFile avatar)
        {

            AddNewUserResult result = await _userService.CreateUser(user, avatar);

            switch (result)
            {
                case AddNewUserResult.DuplicateEmail:
                    TempData[ErrorMessage] = _localizer["Duplicated Email"];

                    break;
                case AddNewUserResult.DuplicateMobileNumber:
                    TempData[ErrorMessage] = _localizer["Duplicated Mobile Number"];

                    break;
                case AddNewUserResult.Success:
                    TempData[SuccessMessage] = _localizer["User added successFully"].Value;

                    return RedirectToAction("Index");
            }

            return View();
        }

        #endregion

        #region edit user

        public async Task<IActionResult> EditUser(ulong id)
        {
            var user = await _userService.GetUserForEdit(id);
            if (user == null)
            {
                return NotFound();
            }

            #region Page Data

            ViewData["Roles"] = await _userService.GetSelectRolesList();

            #endregion

            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel userViewModel, IFormFile avatar)
        {
            #region Page Data

            ViewData["Roles"] = await _userService.GetSelectRolesList();

            #endregion

            var res = await _userService.EditUser(userViewModel, avatar);
            switch (res)
            {
                case EditUserResult.DuplicateEmail:
                    TempData[ErrorMessage] = _localizer["Duplicated Email"];

                    break;
                case EditUserResult.DuplicateMobileNumber:
                    TempData[ErrorMessage] = _localizer["Duplicated Mobile Number"];

                    break;
                case EditUserResult.Success:
                    return RedirectToAction("Index");
            }

            return View();
        }

        #endregion

        #region remove user

        public async Task<IActionResult> RemoveUser(ulong userId)
        {
            var res = await _userService.RemoveUserById(userId);

            if (res)
            {
                return ApiResponse.SetResponse(ApiResponseStatus.Success, null, _localizer["Mission Accomplished"].Value);
            }
            return ApiResponse.SetResponse(ApiResponseStatus.Danger, null, _localizer["The operation failed"].Value);
        }

        #endregion

        #endregion
    }
}
