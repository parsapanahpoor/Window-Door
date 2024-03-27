using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using Window.Application.CQRS.SellerPanel.SellerChequeInfo.Command.AddOrEditSellerChequeInfo;
using Window.Application.CQRS.SellerPanel.SellerChequeInfo.Query.AddOrEditSellerChequeInfo;
using Window.Application.Extensions;
using Window.Domain.ViewModels.Seller.SellerChequeInfo;
namespace Window.Web.Areas.Seller.Controllers;

public class SellerChequeInfoController : SellerBaseController
{
    #region Add Or Edit Seller Cheque Info

    [HttpGet]
    public async Task<IActionResult> AddOrEditSellerChequeInfo(bool FillData = false, 
                                                               CancellationToken cancellation = default)
    {
        #region Notification

        if (FillData)
        {
            TempData[WarningMessage] = "برای ثبت محصولات خود می بایست ابتدا اطلاعات چک های دریافتی خود را وارد نمایید.";
        }

        #endregion

        #region Fill Model 

        var model = await Mediator.Send(new AddOrEditSellerChequeInfoQuery()
        {
            SellerUserId = User.GetUserId(),
        },
        cancellation);

        if (model == null)
        {
            return View(new SellerChequeInfoSellerSideDTO()
            {
                SellerUserId = User.GetUserId(),
            });
        }

        #endregion

        return View(model);
    }

    [HttpPost , ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEditSellerChequeInfo(SellerChequeInfoSellerSideDTO model , CancellationToken cancellation = default)
    {
        #region Add Or Edit Seller Cheque Info

        var res = await Mediator.Send(new AddOrEditSellerChqueInfoCommand()
        {
            CountOfCheque = model.CountOfCheque,
            HasLimitation = model.HasLimitation,
            SellerMaximumDays = model.SellerMaximumDays,
            SellerUserId = model.SellerUserId
        },
        cancellation) ;

        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction("Index" , "Home" , new { area = "Seller" });
        }

        #endregion
         
        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(model);
    }

    #endregion
}
