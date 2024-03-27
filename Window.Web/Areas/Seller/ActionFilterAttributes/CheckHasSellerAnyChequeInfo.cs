using Microsoft.AspNetCore.Mvc.Filters;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Web.Areas.Seller.ActionFilterAttributes;

public class CheckHasSellerAnyChequeInfo : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var service = (IOrderChequeQueryRepository)context.HttpContext.RequestServices.GetService(typeof(IOrderChequeQueryRepository))!;

        base.OnActionExecuting(context);

        var hasAnyChequeInfo = service.Get_SellerChequeInfo_BySellerUserId_Sync(context.HttpContext.User.GetUserId()).Result;

        if (hasAnyChequeInfo == null)
        {
            context.HttpContext.Response.Redirect("/Seller/SellerChequeInfo/AddOrEditSellerChequeInfo?FillData=true");
        }
    }
}