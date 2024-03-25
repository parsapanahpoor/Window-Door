using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.ShopOrder.Command;
using Window.Application.Extensions;
using Window.Domain.Enums.Order;
namespace Window.Web.Controllers.Shop;

[Authorize]
public class OrderPaymentController : SiteBaseController
{
	#region Select Order Payment Way

	[HttpPost]
	public async Task<IActionResult> SelectOrderPaymentWay(OrderPaymentWay OrderPaymentWay, 
														   CancellationToken cancellationToken = default)
	{
		#region Select Order Payment Way

		if (ModelState.IsValid)
		{
			var res = await Mediator.Send(new SelectOrderPaymentWayCommand()
			{
				OrderPaymentWay = OrderPaymentWay,
				UserId = User.GetUserId(),
			} , 
			cancellationToken);

            switch (res)
            {
                case SelectOrderPaymentWayResult.SuccessInstallerPayment:
                    TempData[SuccessMessage] = "درخواست با موفقیت ثبت شده است.";
                    TempData[SuccessMessage] = "درقدم بعدی شما باید اطلاعات چک های خود را ثبت کنید.";
                    return RedirectToAction("ManageShopOrder", "ShopOrder" , new { area = "Seller" });

                case SelectOrderPaymentWayResult.ChoseInstallerInPass:
                    TempData[WarningMessage] = "شما میتوانید در این بخش به ادامه ی تکمیل اطلاعات خرید خود بپردازید.";
                    return RedirectToAction("ManageShopOrder", "ShopOrder", new { area = "Seller" });

                case SelectOrderPaymentWayResult.SuccessCashPayment:
                    TempData[SuccessMessage] = "درخواست با موفقیت ثبت شده است.";
                    return RedirectToAction(nameof(CashPayment));

                case SelectOrderPaymentWayResult.Faild:
                    TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                    return RedirectToAction("ShopLanding", "Shop");

                default:
                    break;
            }
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction("ShopLanding", "Shop");
    }

    #endregion

    #region Cash Payment 

    public async Task<IActionResult> CashPayment()
    {
        return View();
    }

    #endregion
}
