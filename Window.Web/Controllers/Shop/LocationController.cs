using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.Location.Command;
using Window.Application.CQRS.SiteSide.Location.Query;
using Window.Application.Extensions;
using Window.Domain.ViewModels.Site.Shop.Location;
using Window.Domain.ViewModels.Site.Shop.SellerDetail;

namespace Window.Web.Controllers.Shop;

[Authorize]
public class LocationController : SiteBaseController
{
    #region List Of User Location

    public async Task<IActionResult> ListOfUserLocations(CancellationToken cancellation = default)
    {
        return View(await Mediator.Send(new ListOfUserLocationsQuery()
        {
            UserId = User.GetUserId()
        },
        cancellation));
    }

    #endregion

    #region Add Or Edit Location

    [HttpPost]
    public async Task<IActionResult> AddOrEditLocation(AddOrEditLocationDTO model,
                                                       CancellationToken cancellation = default)
    {
        #region Add Or Edit Location

        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(new AddOrEditLocationCommand()
            {
                Address = model.Address,
                City = model.City,
                State = model.State,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mobile = model.Mobile,
                PostalCode = model.PostalCode,
                UserId = User.GetUserId()
            },
       cancellation);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(ListOfUserLocations));
            }
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction(nameof(ListOfUserLocations));
    }

    #endregion

    #region Delete Location

    public async Task<IActionResult> DeleteLocation(DeleteLocationCommand command ,
                                                    CancellationToken cancellationToken = default)
    {
        var res = await Mediator.Send(command, cancellationToken);
        if (!res)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction(nameof(ListOfUserLocations));
        }

        TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
        return RedirectToAction(nameof(ListOfUserLocations));
    }

    #endregion

    #region Add LocationId To Order

    [HttpGet]
    public async Task<IActionResult> AddLocationIdToOrder(ulong locationId , 
                                                          CancellationToken cancellationToken = default)
    {
        var res = await Mediator.Send(new AddLocationIdToOrderCommand()
        {
            LocationId = locationId,
            UserId = User.GetUserId(),
        },
        cancellationToken);

        switch (res)
        {
            case AddLocationIdToOrderResult.Success:
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction("ShowInvoice", "Order");

            case AddLocationIdToOrderResult.OrderNotFound:
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return RedirectToAction("ShopCart", "Order");

            case AddLocationIdToOrderResult.WaitingForPaymentOrder:
                TempData[WarningMessage] = "شما یک فاکتور درانتظار پرداخت دارید . لطفا در ابتدا آن را بررسی بفرمایید";
                return RedirectToAction("ShowInvoice", "Order");

            default:
                break;
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction("ShopCart", "Order");
    }

    #endregion
}
