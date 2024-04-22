using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.ShowFactor;
using Window.Application.CQRS.SiteSide.ShopOrder.Command;
using Window.Application.CQRS.SiteSide.ShopOrder.Query;
using Window.Application.Extensions;
using Window.Web.ActionFilterAttributes;
namespace Window.Web.Controllers.Shop;

[Authorize]
public class OrderController : SiteBaseController
{
    #region Ctor

    #endregion

    #region Check Is Exist Any Waiting For Payment Order

    public async Task<bool> CheckIsExistAny_WaitingForPayment_Order(CancellationToken cancellation)
    {
        return await Mediator.Send(new IsExistAnyWaitingForPaymentOrderQuery()
        {
            UserId = User.GetUserId()
        }, cancellation);
    }

    #endregion

    #region Add To Shop Cart

    [ManageUserShopOrder]
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

            case AddToShopOrderRes.WaitingForPaymentOrderExist:
                TempData[WarningMessage] = "شما یک فاکتور درانتظار پرداخت دارید . لطفا در ابتدا آن را بررسی بفرمایید";
                return RedirectToAction(nameof(ShowInvoice));

            default:
                break;
        }

        return View("_EmptyShopCart");
    }

    #endregion

    #region Shop Cart 

    [ManageUserShopOrder]
    public async Task<IActionResult> ShopCart(CancellationToken cancellationToken = default)
    {
        var model = await Mediator.Send(new ShopCartQuery()
        {
            UserId = User.GetUserId(),
        },
        cancellationToken
        );

        if (model == null || model.ProductItems == null)return View("_EmptyShopCart");

        return View(model);
    }

    #endregion

    #region Plus Product In Shop Cart

    [ManageUserShopOrder]
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

    [ManageUserShopOrder]
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

    [ManageUserShopOrder]
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

    #region Delete Order

    public async Task<IActionResult> DeleteOrder(CancellationToken cancellation)
    {
        var res = await Mediator.Send(new DeleteOrderCommand()
        {
            UserId = User.GetUserId(),
        } , 
        cancellation);

        if (res)
        {
            TempData[SuccessMessage] = "سبد خرید شما خالی شده و میتوانید مجددا محصولات موردنظرتان را انتخاب کنید.";
            return RedirectToAction("ShopLanding", "Shop");
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction(nameof(ShopCart));
    }

    #endregion

    #region Show Invoice

    public async Task<IActionResult> ShowInvoice(bool continueOrder = false,
                                                 CancellationToken cancellation = default)
    {
        if (continueOrder) 
        {
            TempData[WarningMessage] = "شما یک فاکتور درانتظار پرداخت دارید . لطفا در ابتدا آن را بررسی بفرمایید";
        }

        var invoice = await Mediator.Send(new ShowInvoiceQuery()
        {
            UserId = User.GetUserId(),
        } ,
        cancellation);

        if (invoice == null) return NotFound();

        return View(invoice);
    }

    #endregion

    #region Show Factor 

    public async Task<IActionResult> ShowFactor(ulong orderId,
                                                CancellationToken cancellation)
    {
        var res = await Mediator.Send(new ShowFactorSellerSideQuery()
        {
            OrderId = orderId,
        },
        cancellation);
        if (res == null) return NotFound();

        return View(res);
    }

    #endregion
}
