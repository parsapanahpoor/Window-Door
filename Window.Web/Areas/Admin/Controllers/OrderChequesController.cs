using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.OrderCheques.Command;
using Window.Application.CQRS.AdminPanel.OrderCheques.Query;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Admin.OrderCheque;
namespace Window.Web.Areas.Admin.Controllers;

public class OrderChequesController : AdminBaseController
{
    #region Filter Order Cheques

    public async Task<IActionResult> FilterOrderCheques(FilterOrderChequesDTO filter,
                                                        CancellationToken cancellationToken = default)
    {
        return View(await Mediator.Send(new FilterOrderChequesQuery()
        {
            FilterOrderChequesDTO = filter,
        },
        cancellationToken));
    }

    #endregion

    #region Show Order Cheque Detail

    public async Task<IActionResult> ShowOrderChequeDetail(ShowOrderChequeDetailQuery query , 
                                                           CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(query , cancellationToken);
        if (res == null) return NotFound();

        return View(res);
    }

    #endregion

    #region Show Order Cheque Detail

    [HttpGet]
    public async Task<IActionResult> ChequeDetail(ulong orderChequeId , 
                                                  CancellationToken cancellationToken)
    {
        #region View Bag Model

        var orderChequeDetail = await Mediator.Send(new ShowChequeDetailForAdminReviewQuery()
        {
            ChequeId = orderChequeId
        }, 
        cancellationToken);
        if (orderChequeDetail.CustomerNationalId == null) return NotFound();

        ViewBag.OrderCheque = orderChequeDetail;

        #endregion

        return View(new AdminChequeDetailChangeStateDTO()
        {
            OrderChequeId = orderChequeId,
            AdminRejectDescription = orderChequeDetail.AdminRejectDescription,
            OrderChequeAdminState = orderChequeDetail.OrderChequeAdminState,
            OrderId = orderChequeDetail.OrderId,
        });
    }
    [HttpPost , ValidateAntiForgeryToken]
    public async Task<IActionResult> ChequeDetail(AdminChequeDetailChangeStateDTO command,
                                                  CancellationToken cancellationToken)
    {
        #region Update Cheque Info

        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(new ChangeOrderChequeStateAdminCommand()
            {
                AdminRejectDescription= command.AdminRejectDescription,
                OrderChequeAdminState= command.OrderChequeAdminState,
                OrderChequeId = command.OrderChequeId,  
            } , 
            cancellationToken);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(ShowOrderChequeDetail) , new { orderId = command.OrderId });
            }
        }

        #endregion

        #region View Bag Model

        var orderChequeDetail = await Mediator.Send(new ShowChequeDetailForAdminReviewQuery()
        {
            ChequeId = command.OrderChequeId
        },
        cancellationToken);
        if (orderChequeDetail.CustomerNationalId == null) return NotFound();

        ViewBag.OrderCheque = orderChequeDetail;

        #endregion

        return View(new AdminChequeDetailChangeStateDTO()
        {
            OrderChequeId = command.OrderChequeId,
            AdminRejectDescription = orderChequeDetail.AdminRejectDescription,
            OrderChequeAdminState = orderChequeDetail.OrderChequeAdminState,
            OrderId = orderChequeDetail.OrderId
        });
    }

    #endregion
}
