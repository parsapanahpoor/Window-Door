﻿using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.OrderCheques.Command;
using Window.Application.CQRS.AdminPanel.OrderCheques.Query;
using Window.Application.CQRS.SellerPanel.OrderCheque.Command;
using Window.Application.CQRS.SellerPanel.OrderCheque.Query;
using Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;
using Window.Application.Extensions;
using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;
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
}
