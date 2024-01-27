using Microsoft.AspNetCore.Mvc;
namespace Window.Web.Controllers.Shop;

public class ShopSellerController : SiteBaseController
{
    #region Ctor



    #endregion

    #region Seller Detail

    public async Task<IActionResult> SellerDetail(ulong sellerId , CancellationToken cancellation)
    {
        return View();
    }

    #endregion
}
