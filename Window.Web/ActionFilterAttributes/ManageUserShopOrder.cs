using Microsoft.AspNetCore.Mvc.Filters;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.Order;

namespace Window.Web.ActionFilterAttributes;

public class ManageUserShopOrder : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var service = (IOrderQueryRepository)context.HttpContext.RequestServices.GetService(typeof(IOrderQueryRepository))!;

        base.OnActionExecuting(context);

        var order = service.GetLastest_NotFinally_Order(context.HttpContext.User.GetUserId()).Result;

        if (order != null)
        {
            if (order.LocationId.HasValue)
            {
                if (order.PaymentWay.HasValue)
                {
                    if (order.PaymentWay.Value == Domain.Enums.Order.OrderPaymentWay.InstallmentPayment)
                    {
                        context.HttpContext.Response.Redirect("/Order/ShowInvoice");
                    }
                    else
                    {
                        context.HttpContext.Response.Redirect("/Order/ShowInvoice");
                    }
                }
                else
                {
                    context.HttpContext.Response.Redirect("/Order/ShowInvoice?continueOrder=true");
                }
            }
        }
    }
}
