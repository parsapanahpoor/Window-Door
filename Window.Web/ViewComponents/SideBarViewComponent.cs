using Microsoft.AspNetCore.Mvc;

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
