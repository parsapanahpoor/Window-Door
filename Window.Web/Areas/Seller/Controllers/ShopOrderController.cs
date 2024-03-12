using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;
using Window.Application.Extensions;
namespace Window.Web.Areas.Seller.Controllers;

public class ShopOrderController : SellerBaseController
{
    #region Manage Order Pages 



    #endregion

    #region List OF User Orders 

    

    #endregion

    #region Manage Shop Order

    public async Task<IActionResult> ManageShopOrder(ulong? orderId , 
                                                     CancellationToken cancellationToken = default)
    {
        #region Initial Model 

        var model = await Mediator.Send(new ManageShopOrderDetailQuery()
        {
            userId = User.GetUserId(),
            orderId = orderId,
        } , 
        cancellationToken);

        if(model == null) return NotFound();

        #endregion

        return View(model);
    }

    #endregion
}
