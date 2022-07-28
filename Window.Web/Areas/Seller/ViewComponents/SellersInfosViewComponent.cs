using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Window.Web.Areas.Seller.ViewComponents
{
    public class SellersInfosViewComponent : ViewComponent
    {
        #region Ctor

        private readonly ISellerService _sellerservice;

        public SellersInfosViewComponent(ISellerService sellerservice)
        {
            _sellerservice = sellerservice;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _sellerservice.GetSellersInfosState(User.GetUserId());
            return View("SellersInfos" , model );
        }
    }

    public class SellersInfosBadgeViewComponent : ViewComponent
    {
        #region Ctor

        private readonly ISellerService _sellerservice;

        public SellersInfosBadgeViewComponent(ISellerService sellerservice)
        {
            _sellerservice = sellerservice;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _sellerservice.GetSellersInfosState(User.GetUserId());
            return View("SellersInfosBadge", model);
        }
    }
}
