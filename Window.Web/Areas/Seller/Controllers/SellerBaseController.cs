using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Web.Areas.Seller.ActionFilterAttributes;

namespace Window.Web.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize]
    [CheckUserHasPermission]
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
    }
}
