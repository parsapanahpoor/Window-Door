using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Interfaces;

namespace Window.Web.Areas.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IUserService _userService;

        public NavbarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserById(User.GetUserId());

            return View("Navbar", user);
        }
    }
}
