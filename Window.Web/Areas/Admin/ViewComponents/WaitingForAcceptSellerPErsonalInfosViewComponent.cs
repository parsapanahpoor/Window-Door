using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;

namespace Window.Web.Areas.Admin.ViewComponents
{
    public class WaitingForAcceptSellerPErsonalInfosViewComponent : ViewComponent
    {
        private readonly ISellerService _sellerService;

        public WaitingForAcceptSellerPErsonalInfosViewComponent(ISellerService sellerService)
        {
            _sellerService= sellerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("WaitingForAcceptSellerPErsonalInfos" , await _sellerService.WaitingForAcceptSellerPErsonalInfos());
        }
    }
}
