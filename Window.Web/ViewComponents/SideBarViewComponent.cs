using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Interfaces;

namespace Window.Web.Areas.ViewComponents
{
    public class SideBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SideBar");
        }
    }
}

namespace Window.Web.Areas.ViewComponents
{
    public class NewSideBarViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IUserService _userService;

        public NewSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserById(User.GetUserId());

            return View("NewSideBar", user);
        }
    }
}

