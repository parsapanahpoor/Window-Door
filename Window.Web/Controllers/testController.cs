using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Web.Controllers
{
    [Authorize]
    public class testController : SiteBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        private readonly IStateService _stateService;

        private readonly IBrandService _brandService;

        private readonly ISampleService _sampleService;

        private readonly IInquiryService _inquiryService;

        public testController( IProductService prodcutService, IStateService stateService, IBrandService brandService, ISampleService sampleService, IInquiryService inquiryService)
        {
            _productService = prodcutService;
            _stateService = stateService;
            _brandService = brandService;
            _sampleService = sampleService;
            _inquiryService = inquiryService;
        }

        #endregion

        #region Test For Step 1

        public async Task<IActionResult> InquiryStep1(FilterInquiryViewModel filter)
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

            #region Brand ViewBag

            ViewBag.Brand = await _brandService.GetAllBrands();

            #endregion

            ViewBag.UserId = User.GetUserId();

            return View();
        }

        #endregion

        #region Test For Step 2

        public async Task<IActionResult> InquiryStep2(ulong CountryId , ulong StateId , ulong CityId , ProductType? ProductType , ProductKind? ProductKind , SellerType? SellerType , ulong? MainBrandId , string UserMacAddress)
        {
            #region Model State Valdiation

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            #endregion

            #region Step 1 Log For User 

            FilterInquiryViewModel log = new FilterInquiryViewModel()
            {
                CountryId = CountryId,
                StateId = StateId,
                CityId = CityId,
                ProductType = ProductType,
                ProductKind = ProductKind,
                MainBrandId = MainBrandId,
                UserMacAddress = UserMacAddress,
                SellerType = SellerType
            };

            #endregion

            #region Proccess Step 1 Log 

            await _inquiryService.LogInquiryForUserPart1(log);

            #endregion

            return RedirectToAction(nameof(InquiryStep3) , new { userMacAddress = UserMacAddress });
        }

        #endregion

        #region Inquiry Step 3

        [HttpGet]
        public async Task<IActionResult> InquiryStep3(string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null) return NotFound();

            #endregion

            ViewBag.UserMacAddress = userMacAddress;

            return View(samples);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> InquiryStep3(ulong sampleId , int width , int height , string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null) return NotFound();

            #endregion

            #region Check Is Exist Sample 

            var sample = await _sampleService.GetSampleBySampleId(sampleId);
            if (sample == null) return NotFound();

            #endregion

            #region Add Log For User

            var res = await _inquiryService.LogInquiryForUserPart2(sampleId , width , height , userMacAddress);
            if (!res) return NotFound();

            #endregion

            ViewBag.UserMacAddress = userMacAddress;

            return View(samples);
        }

        #endregion

        #region Inquiry Step 4 (proccess inquiry)

        public async Task<IActionResult> InquiryStep4(string userMacAddress , ulong? MainBrandId, int pageId = 1 )
        {
            #region Brand ViewBag

            ViewBag.Brand = await _brandService.GetAllBrands();

            #endregion

            #region Fill Model

            var model = await _inquiryService.ListOfInquiry(userMacAddress);
            if (model == null)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            #endregion

            ViewBag.pageId = pageId;

            ViewBag.userMacAddress = userMacAddress;

            #region Paginaition

            int take = 20;

            int skip = (pageId - 1) * take;

            int pageCount = (model.Count() / take);

            if ((pageCount % 2) == 0 || (pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = model.Skip(skip).Take(take).ToList();

            var viewModel = Tuple.Create(query, pageCount);

            #endregion

            TempData[SuccessMessage] = "استعلام گیری با موفقیت انجام شده است .";
            return View(viewModel);
        }

        #endregion
    }
}
