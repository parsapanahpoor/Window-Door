using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.Log;

namespace Window.Web.Areas.Admin.Controllers
{
    public class LogForVisitSellerProfileController : AdminBaseController
    {
        #region Ctor

        private readonly ISellerService _sellerService;

        public LogForVisitSellerProfileController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region Filter Log 

        public async Task<IActionResult> FilterLogForVisitSellerProfile(FilterLogVisitSellerProfileViewModel filter)
        {
            return View(await _sellerService.FilterLogVisitSellerProfile(filter));    
        }

        #endregion

        #region Log For Brands 

        public async Task<IActionResult> LogForBrands(LogForBrandsViewModel filter)
        {
            return View(await _sellerService.FilterLogForBrands(filter));
        }

        #endregion
    }
}
