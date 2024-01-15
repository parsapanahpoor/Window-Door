using MediatR;
using Microsoft.AspNetCore.Mvc;
using Window.Presentation.Filter;

namespace Window.Web.Controllers;


[CatchExceptionFilter]
public class SiteBaseController : Controller
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
