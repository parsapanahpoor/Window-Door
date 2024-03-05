using Microsoft.AspNetCore.Mvc;
namespace Window.Web.Areas.Seller.Controllers;

public class ShopOrderController : SellerBaseController
{
    #region Manage Order Pages 



    #endregion

    #region List OF User Orders 

    #endregion

    #region Manage Shop Order

    public async Task<IActionResult> ManageShopOrder(CancellationToken cancellationToken = default)
    {
        return View();
    }

    #endregion
}
