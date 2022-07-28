using Window.Application.Extensions;
using Window.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Window.Web.Areas.User.ViewComponents
{
    public class SellerNavbarViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IUserService _userService;

        public SellerNavbarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserById(User.GetUserId());

            return View("SellerNavbar", user);
        }
    }
}
