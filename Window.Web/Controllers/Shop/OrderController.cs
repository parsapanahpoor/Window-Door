using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Window.Application.CQRS.SiteSide.ShopOrder.Command;
using Window.Application.CQRS.SiteSide.ShopOrder.Query;
using Window.Application.Extensions;
using Window.Domain.Interfaces.Order;
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

    public async Task<IActionResult> ShopCart(CancellationToken cancellationToken)
    {
        var model = await Mediator.Send(new ShopCartQuery()
        {
            UserId = User.GetUserId(),
        },
        cancellationToken
        );

        if (model == null || model.ProductItems == null)
        {
            return View("_EmptyShopCart");
        }

        return View(model);
    }

    #endregion

    #region Plus Product In Shop Cart

    public async Task<IActionResult> PlusProductInShopCart(PlusProductInShopCartQuery plus , 
                                                           CancellationToken cancellation = default)
    {
        plus.UserId = User.GetUserId();

        var res = await Mediator.Send(plus , cancellation);
        if (!res)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction("ShopLanding", "Shop");
        }

        TempData[SuccessMessage] = "محصول مورد نظر باموفقیت به سبد خرید شما اضافه شده است.";
        return RedirectToAction(nameof(ShopCart));
    }

    #endregion

    #region Minus Product In Shop Cart

    public async Task<IActionResult> MinusProductInShopCart(MinusProductInShopCartQuery minus,
                                                           CancellationToken cancellation = default)
    {
        minus.UserId = User.GetUserId();

        var res = await Mediator.Send(minus, cancellation);
        if (!res)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction("ShopLanding", "Shop");
        }

        TempData[SuccessMessage] = "محصول مورد نظر باموفقیت به سبد خرید شما اضافه شده است.";
        return RedirectToAction(nameof(ShopCart));
    }

    #endregion

    #region Delete Product From Shop Order

    public async Task<IActionResult> DeleteProductFromShopOrder(DeleteProductFromShopCartCommand command , 
                                                                CancellationToken cancellationToken)
    {
        command.UserId = User.GetUserId();

        var res = await Mediator.Send(command, cancellationToken);
        if (!res)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction("ShopLanding", "Shop");
        }

        TempData[SuccessMessage] = "محصول مورد نظر از سبد خرید شما پاک شده است.";
        return RedirectToAction(nameof(ShopCart));
    }

    #endregion
}
