using Microsoft.AspNetCore.Mvc;

namespace Window.Web.Areas.Admin.ViewComponents
{
    public class AdminNavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminNavbar");
        }
    }
}
