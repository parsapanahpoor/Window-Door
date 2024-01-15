using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Presentation.Filter;
using Window.Web.Areas.Seller.ActionFilterAttributes;

namespace Window.Web.Areas.Seller.Controllers;

[Area("Seller")]
[Authorize]
[CheckUserHasPermission]
[CatchExceptionFilter]

public class SellerBaseController : Controller
{
    public static string SuccessMessage = "SuccessMessage";
    public static string ErrorMessage = "ErrorMessage";
    public static string InfoMessage = "InfoMessage";
    public static string WarningMessage = "WarningMessage";

    // Swal Messages Temp Data Key
    public static string SwalSuccess = "success";
    public static string SwalError = "error";
    public static string SwalInfo = "info";

    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
