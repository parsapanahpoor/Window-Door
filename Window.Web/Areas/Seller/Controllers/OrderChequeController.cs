using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SellerPanel.OrderCheque.Command;
using Window.Application.Extensions;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Seller.OrderCheque;
namespace Window.Web.Areas.Seller.Controllers;

public class OrderChequeController : SellerBaseController
{
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
    public async Task<IActionResult> UploadOrderCheque(UploadOrderChequeDTO model , 
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
                    return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller" , orderId = model.OrderId }) ;
                
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
}
