using Window.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Application.Extensions;

namespace Window.Web.Areas.User.ViewComponents
{
    public class SellerSideBarViewComponent : ViewComponent
    {
        #region Ctor

        private readonly ISellerService _sellerService;

        public SellerSideBarViewComponent(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }
    
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SellersInfosState = await _sellerService.GetSellersInfosState(User.GetUserId());

            if (await _sellerService.IsSellerMaster(User.GetUserId()))
            {
                ViewBag.SellerMastert = true;
            }

            return View("SellerSideBar");
        }
    }
}
