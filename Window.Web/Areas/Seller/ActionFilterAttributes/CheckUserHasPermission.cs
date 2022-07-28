using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Window.Web.Areas.Seller.ActionFilterAttributes
{
    public class CheckUserHasPermission : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var service = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService))!;

            base.OnActionExecuting(context);

            var hasUserAnyRole = service.IsUserSeller(context.HttpContext.User.GetUserId()).Result;

            if (!hasUserAnyRole)
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
    }
}
