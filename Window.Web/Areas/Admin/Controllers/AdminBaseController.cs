using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Web.Areas.Admin.ActionFilterAttributes;

namespace Window.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [CheckUserHasPermission]
    public class AdminBaseController : Controller
    {
        public static string SuccessMessage = "SuccessMessage";
        public static string ErrorMessage = "ErrorMessage";
        public static string InfoMessage = "InfoMessage";
        public static string WarningMessage = "WarningMessage";

        // Swal Messages Temp Data Key
        public static string SwalSuccess = "success";
        public static string SwalError = "error";
        public static string SwalInfo = "info";
    }
}
