using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.ShopSeller.ShopSellerDetail;
namespace Window.Web.Controllers.Shop;

public class ShopSellerController : SiteBaseController
{
    #region Ctor



    #endregion

    #region Seller Detail

    public async Task<IActionResult> SellerDetail(ShopSellerDetailQuery shopSellerDetail ,
                                                  CancellationToken cancellation = default)
    {
        var model = await Mediator.Send(shopSellerDetail , cancellation);
        if (ModelState == null) return NotFound(); 

        return View(model);
    }

    #endregion
}
