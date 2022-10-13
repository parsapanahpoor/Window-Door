using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.ViewModels.Admin.Log;

namespace Window.Web.Areas.Admin.Controllers
{
    public class LogForVisitSellerProfileController : AdminBaseController
    {
        #region Ctor

        private readonly ISellerService _sellerService;
        private readonly IStateService _stateService;
        private readonly IInquiryService _inquiryService;

        public LogForVisitSellerProfileController(ISellerService sellerService, IStateService stateService, IInquiryService inquiryService)
        {
            _sellerService = sellerService;
            _stateService = stateService;
            _inquiryService = inquiryService;
        }

        #endregion

        #region Filter Log 

        public async Task<IActionResult> FilterForLogInquiry(FilterLogForInquiryViewModel filter)
        {
            #region Location ViewBags 

            ViewData["Countries"] = await _stateService.GetAllCountries();

            if (filter.CountryId != null)
            {
                ViewData["States"] = await _stateService.GetStateChildren(filter.CountryId.Value);
                if (filter.StateId != null)
                {
                    ViewData["Cities"] = await _stateService.GetStateChildren(filter.StateId.Value);
                }
            }

            //Today Inquiry 
            ViewBag.Today = await _inquiryService.CountOfTodayInquiry();

            //Month Inqury
            ViewBag.Month = await _inquiryService.CountOfMonthInquiry();

            //Year Inqiry 
            ViewBag.Year = await _inquiryService.CountOfYearInquiry();

            #endregion

            return View(await _sellerService.FilterLogForInquiryViewModel(filter));
        }

        #endregion

        #region Filter Log 

        public async Task<IActionResult> FilterLogForVisitSellerProfile(FilterLogVisitSellerProfileViewModel filter)
        {
            #region Location ViewBags 

            ViewData["Countries"] = await _stateService.GetAllCountries();

            if (filter.CountryId != null)
            {
                ViewData["States"] = await _stateService.GetStateChildren(filter.CountryId.Value);
                if (filter.StateId != null)
                {
                    ViewData["Cities"] = await _stateService.GetStateChildren(filter.StateId.Value);
                }
            }

            #endregion

            return View(await _sellerService.FilterLogVisitSellerProfile(filter));    
        }

        #endregion

        #region Log For Brands 

        public async Task<IActionResult> LogForBrands(LogForBrandsViewModel filter)
        {
            #region Location ViewBags 

            ViewData["Countries"] = await _stateService.GetAllCountries();

            if (filter.CountryId != null)
            {
                ViewData["States"] = await _stateService.GetStateChildren(filter.CountryId.Value);
                if (filter.StateId != null)
                {
                    ViewData["Cities"] = await _stateService.GetStateChildren(filter.StateId.Value);
                }
            }

            #endregion

            return View(await _sellerService.FilterLogForBrands(filter));
        }

        #endregion
    }
}
