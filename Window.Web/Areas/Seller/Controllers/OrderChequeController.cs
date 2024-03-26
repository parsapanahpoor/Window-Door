using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SellerPanel.ChequeReceipt.Command;
using Window.Application.CQRS.SellerPanel.ChequeReceipt.Query;
using Window.Application.CQRS.SellerPanel.OrderCheque.Command;
using Window.Application.Extensions;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Seller.ChequeReceipt;
using Window.Domain.ViewModels.Seller.OrderCheque;
namespace Window.Web.Areas.Seller.Controllers;

public class OrderChequeController : SellerBaseController
{
    #region Manage Pages

    public async Task<IActionResult> ManageOrderChequesPages(CancellationToken cancellationToken = default)
    {
        return View();
    }

    #endregion

    #region As Seller 

    #region List Of Recive Cheques

    public async Task<IActionResult> ListOfWithdrawCheques(FilterReciveOrderChequesSellerSideDTO filter ,
                                                           CancellationToken cancellation = default) 
    {
        filter.UserId = User.GetUserId();

        return View(await Mediator.Send(new FilterReciveOrderChequesSellerSideQuery()
        {
            FilterOrderChequesDTO = filter
        },
        cancellation));
    }

    #endregion

    #endregion

    #region As Customer 

    #region List Of Deposit Cheques

    public async Task<IActionResult> ListOfDepositCheques(FilterReciveOrderChequesSellerSideDTO filter,
                                                           CancellationToken cancellation = default)
    {
        filter.UserId = User.GetUserId();

        return View(await Mediator.Send(new FilterDepositOrderChequesSellerSideQuery()
        {
            FilterOrderChequesDTO = filter
        },
        cancellation));
    }

    #endregion

    #region Upload Order Cheque 

    [HttpGet]
    public IActionResult UploadOrderCheque(ulong orderId)
    {
        return View(new UploadOrderChequeDTO()
        {
            OrderId = orderId,
        });
    }

    [HttpPost]
    public async Task<IActionResult> UploadOrderCheque(UploadOrderChequeDTO model,
                                                       CancellationToken cancellationToken = default)
    {
        #region Add Cheque 

        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(new UploadOrderChequeCommand()
            {
                ChequeDateTime = model.ChequeDateTime,
                ChequeImage = model.ChequeImage,
                ChequePrice = model.ChequePrice,
                CustomerNationalId = model.CustomerNationalId,
                CustomerUserId = User.GetUserId(),
                OrderId = model.OrderId
            },
            cancellationToken);

            switch (res)
            {
                case UploadOrderChequeResult.SellerDosentSupportCheque:
                    TempData[ErrorMessage] = "درحال حاضر فروشنده ی مورد نظر فروش قسطی و چکی ندارد.";
                    break;

                case UploadOrderChequeResult.SellerHasAnLimitationOFDay:
                    TempData[ErrorMessage] = "تاریخ چک وارد شده از بازه ی زمانی قابل قبول فروشنده بیشتر است.";
                    break;

                case UploadOrderChequeResult.SellerHasAnLimitationOfChequeCount:
                    TempData[ErrorMessage] = "تعداد چک آپلود شده ی شما از تعداد چک تعیین شده توسط فروشنده بیشتر شده است.";
                    break;

                case UploadOrderChequeResult.ThisIsNotYourOrder:
                    TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                    break;

                case UploadOrderChequeResult.Success:
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                    return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller", orderId = model.OrderId });

                case UploadOrderChequeResult.Faild:
                    TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                    break;

                default:
                    break;
            }
        }

        #endregion

        return View(model);
    }

    #endregion

    #region Upload Cheque Receipt

    [HttpGet]
    public async Task<IActionResult> UploadChequeReceipt(ulong chequeId , 
                                                         ulong orderId,
                                                         CancellationToken cancellation = default)
    {
        #region Model 

        var res = await Mediator.Send(new ShowOrderChequeReceiptQuery()
        {
            CustomerUserId = User.GetUserId(),
            OrderChequeId = chequeId
        } , 
        cancellation);

        switch (res.EnumResult)
        {
            case ShowOrderChequeReceiptQueryResultEnum.Success:
                break;

            case ShowOrderChequeReceiptQueryResultEnum.SellerDosentAccept:
                TempData[ErrorMessage] = "چک آپلود شده ی شما ابتدا باید توسط فروشنده ی مربوطه تایید گردد.";
                return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller", orderId = orderId });

            case ShowOrderChequeReceiptQueryResultEnum.OrderChequeNotFound:
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller", orderId = orderId });

            default:
                break;
        }

        #endregion

        return View(res.ChequeReceipt);
    }

    [HttpPost , ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadChequeReceipt(ShowOrderChequeReceiptDTO model,
                                                         CancellationToken cancellation = default)
    {
        #region Update Cheque

        var res = await Mediator.Send(new UploadChequeReceiptCommand()
        {
            ChequeReceipt = model,
            CustomerUserId = User.GetUserId(),
        } , 
        cancellation);

        switch (res)
        {
            case UploadChequeReceiptResult.Success:
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller", orderId = model.OrderId });

            case UploadChequeReceiptResult.SellerDosentAccept:
                TempData[ErrorMessage] = "چک آپلود شده ی شما ابتدا باید توسط فروشنده ی مربوطه تایید گردد.";
                return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller", orderId = model.OrderId });

            case UploadChequeReceiptResult.OrderChequeNotfound:
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller", orderId = model.OrderId });

            default:
                break;
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(model);
    }

    #endregion

    #endregion
}
