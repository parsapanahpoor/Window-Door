using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Domain.ViewModels;
using Window.Domain.ViewModels.User;
using Window.Web.Areas.Seller.ActionFilterAttributes;

namespace Window.Web.Areas.Seller.Controllers
{
    public class UserController : SellerBaseController
    {
        #region Ctor

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region usersList

        public async Task<IActionResult> Index(FilterUserViewModel filter)
        {
            return View(await _userService.FilterUsersForSellerPanel(filter , User.GetUserId()));
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

            AddNewUserResult result = await _userService.CreateUserFromSellerPanel(user, avatar ,  User.GetUserId());

            switch (result)
            {
                case AddNewUserResult.DuplicateEmail:
                    TempData[ErrorMessage] = "ایمیل وارد شده تکراری است .";

                    break;
                case AddNewUserResult.DuplicateMobileNumber:
                    TempData[ErrorMessage] = "موبایل وارد شده تکراری است .";

                    break;
                case AddNewUserResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";

                    return RedirectToAction("Index");
            }

            return View();
        }

        #endregion
    }
}
