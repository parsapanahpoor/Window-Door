using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SellerPanel.OrderCheque.Command;
using Window.Application.CQRS.SellerPanel.OrderCheque.Query;
using Window.Application.CQRS.SellerPanel.ShopOrder.Commands;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.FilterShopOrders;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.FilterShopOrdersAsCustomer;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.ShowFactor;
using Window.Application.Extensions;
using Window.Domain.ViewModels.Seller.OrderCheque;
using Window.Domain.ViewModels.Seller.ShopOrder;
using Window.Web.HttpManager;
namespace Window.Web.Areas.Seller.Controllers;

public class ShopOrderController : SellerBaseController
{
    #region Manage Order Pages 

    public IActionResult ManageOrderPage()
    {
        return View();
    }

    #endregion

    #region List OF User Orders As Seller

    public async Task<IActionResult> ListOFUserOrdersAsSeller(FilterShopOrdersSellerSideDTO filter , 
                                                              CancellationToken cancellationToken)
    {
        filter.SellerUserId = User.GetUserId();

        return View(await Mediator.Send(new FilterShopOrdersQuery()
        {
            Filter = filter,
        } , 
        cancellationToken)); ;
    }

    #endregion

    #region List OF User Orders As Customer

    public async Task<IActionResult> ListOFUserOrdersAsCustomer(FilterShopOrdersSellerAsCustomerSideDTO filter,
                                                                CancellationToken cancellationToken)
    {
        filter.CustomerId = User.GetUserId();

        return View(await Mediator.Send(new FilterShopOrdersAsCustomerQuery()
        {
            Filter = filter,
        },
        cancellationToken)); ;
    }

    #endregion

    #region Manage Shop Order

    public async Task<IActionResult> ManageShopOrder(ulong? orderId,
                                                     CancellationToken cancellationToken = default)
    {
        #region Initial Model 

        var model = await Mediator.Send(new ManageShopOrderDetailQuery()
        {
            userId = User.GetUserId(),
            orderId = orderId,
        },
        cancellationToken);
        if (model == null) return NotFound();

        if (model.SellerInformations.SellerId == User.GetUserId())
        {
            return View("ShowWithdrawOrderCheque", model);
        }

        #endregion

        return View(model);
    }

    #endregion

    #region Show Order Cheque Detail

    [HttpGet]
    public async Task<IActionResult> ChequeDetail(ulong orderChequeId,
                                                  CancellationToken cancellationToken)
    {
        #region View Bag Model

        var orderChequeDetail = await Mediator.Send(new ShowChequeDetailForSellerReviewQuery()
        {
            ChequeId = orderChequeId,
            SellerUserId = User.GetUserId(),
        },
        cancellationToken);
        if (orderChequeDetail.CustomerNationalId == null) return NotFound();

        ViewBag.OrderCheque = orderChequeDetail;

        #endregion

        return View(new SellerChequeDetailChangeStateDTO()
        {
            OrderChequeId = orderChequeId,
            SellerRejectDescription = orderChequeDetail.SellerRejectDescription,
            OrderChequeSellerState = orderChequeDetail.OrderChequeSellerState,
            OrderId = orderChequeDetail.OrderId,
        });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChequeDetail(SellerChequeDetailChangeStateDTO command,
                                                  CancellationToken cancellationToken)
    {
        #region Update Cheque Info

        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(new ChangeOrderChequeStateSellerCommand()
            {
                SellerRejectDescription = command.SellerRejectDescription,
                OrderChequeSellerState = command.OrderChequeSellerState,
                OrderChequeId = command.OrderChequeId,
                SellerUserId = User.GetUserId()
            },
            cancellationToken);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(ManageShopOrder), new { orderId = command.OrderId });
            }
        }

        #endregion

        #region View Bag Model

        var orderChequeDetail = await Mediator.Send(new ShowChequeDetailForSellerReviewQuery()
        {
            ChequeId = command.OrderChequeId,
            SellerUserId = User.GetUserId(),
        },
        cancellationToken);
        if (orderChequeDetail.CustomerNationalId == null) return NotFound();

        ViewBag.OrderCheque = orderChequeDetail;

        #endregion

        return View(new SellerChequeDetailChangeStateDTO()
        {
            OrderChequeId = command.OrderChequeId,
            SellerRejectDescription = orderChequeDetail.SellerRejectDescription,
            OrderChequeSellerState = orderChequeDetail.OrderChequeSellerState,
            OrderId = orderChequeDetail.OrderId
        });
    }

    #endregion

    #region Order Finalization 

    [HttpGet]
    public async Task<IActionResult> OrderFinalization(ulong orderId,
                                                       CancellationToken cancellation)
    {
        var res = await Mediator.Send(new OrderFinalizationSellerSideCommand()
        {
            OrderId = orderId,
            SellerUserId = User.GetUserId()
        },
        cancellation);

        if (res)
        {
            return ApiResponse.SetResponse(ApiResponseStatus.Success, null, "عملیات باموفقیت انجام شده است.");
        }

        return ApiResponse.SetResponse(ApiResponseStatus.Danger, null, "عملیات باشکست مواجه شده است.");
    }

    #endregion
}
