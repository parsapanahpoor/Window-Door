using Microsoft.AspNetCore.Mvc;

namespace Window.Web.Areas.Admin.ViewComponents
{
    public class AdminSideBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminSideBar");
        }
    }
}
