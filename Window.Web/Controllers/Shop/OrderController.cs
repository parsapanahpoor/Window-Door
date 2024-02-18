using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.ShopOrder;
using Window.Application.Extensions;
namespace Window.Web.Controllers.Shop;

[Authorize]
public class OrderController : SiteBaseController
{
    #region Ctor

    #endregion

    #region Add To Shop Cart

    public async Task<IActionResult> AddToShopCart(ShopOrderQuery request ,
                                                   CancellationToken cancellationToken = default)
    {
        request.userId = User.GetUserId();

        var res = await Mediator.Send(request , cancellationToken);
        switch (res)
        {
            case AddToShopOrderRes.Success:
                TempData[SuccessMessage] = "محصول مورد نظر باموفقیت به سبد خرید شما اضافه شده است.";
                return RedirectToAction(nameof(ShopCart));

            case AddToShopOrderRes.Faild:
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return RedirectToAction("ShopLanding", "Shop");

            default:
                break;
        }

        return View("_EmptyShopCart");
    }

    #endregion

    #region Shop Cart 

    public async Task<IActionResult> ShopCart()
    {


        return View();
    }

    #endregion
}
