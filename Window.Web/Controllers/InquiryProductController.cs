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
    public class InquiryProductController : SiteBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        private readonly IStateService _stateService;

        private readonly IBrandService _brandService;

        private readonly ISampleService _sampleService;

        private readonly IInquiryService _inquiryService;

        private readonly ISellerService _sellerService;

        public InquiryProductController(IProductService prodcutService, IStateService stateService, IBrandService brandService
            , ISampleService sampleService, IInquiryService inquiryService, ISellerService sellerService)
        {
            _productService = prodcutService;
            _stateService = stateService;
            _brandService = brandService;
            _sampleService = sampleService;
            _inquiryService = inquiryService;
            _sellerService = sellerService;
        }

        #endregion

        #region Inquiry Step 1

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

            #region Glasses ViewBag

            ViewBag.Glasses = await _productService.GetAllGlasses();

            #endregion

            ViewBag.UserId = User.GetUserId();

            return View();
        }

        #endregion

        #region Test For Step 2

        public async Task<IActionResult> InquiryStep2(ulong CountryId, ulong StateId, ulong CityId, ProductType? ProductType, ProductKind? ProductKind, SellerType? SellerType, ulong? MainBrandId, ulong? GlassId, string UserMacAddress)
        {
            #region Model State Valdiation

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا اطلاعات را کامل وارد کنید";
                return RedirectToAction(nameof(InquiryStep1));
            }

            #endregion

            #region Step 1 Log For User 

            FilterInquiryViewModel log = new FilterInquiryViewModel()
            {
                CountryId = CountryId,
                StateId = StateId,
                CityId = CityId,
                MainBrandId = MainBrandId,
                UserMacAddress = UserMacAddress,
                SellerType = SellerType,
                GlassId = GlassId
            };

            #endregion

            #region Proccess Step 1 Log 

            await _inquiryService.LogInquiryForUserPart1(log);

            #endregion

            return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = UserMacAddress });
        }

        #endregion

        #region Inquiry Step 3

        [HttpGet]
        public async Task<IActionResult> InquiryStep3(string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            #endregion

            ViewBag.UserMacAddress = userMacAddress;

            return View(samples);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> InquiryStep3(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            #endregion

            #region Check Is Exist Sample 

            var sample = await _sampleService.GetSampleBySampleId(sampleId);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            if (sample.MaxHeight < height)
            {
                TempData[ErrorMessage] = "ارتفاع وارد شده بیشتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MinHeight > height)
            {
                TempData[ErrorMessage] = "ارتفاع وارد شده کمتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MaxWidth < width)
            {
                TempData[ErrorMessage] = "عرض وارد شده بیشتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MinWidth > width)
            {
                TempData[ErrorMessage] = "عرض وارد شده کمتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }

            #endregion

            #region Add Log For User

            var res = await _inquiryService.LogInquiryForUserPart2(sampleId, width, height, katibeSize, userMacAddress, SampleCount);
            if (!res) return NotFound();

            #endregion

            ViewBag.UserMacAddress = userMacAddress;

            return View(samples);
        }

        #endregion

        #region Inquiry Step 4 (proccess inquiry)

        public async Task<IActionResult> InquiryStep4(string userMacAddress, string? brandTitle, int? orderByPrice, int? orderByScore, int pageId = 1)
        {
            #region Brand ViewBag

            ViewBag.Brand = await _brandService.GetAllBrands();

            #endregion

            #region Update Inqury By Brand 

            if (!string.IsNullOrEmpty(brandTitle))
            {
                var res = await _inquiryService.UpdateUserInquryInLastStep(userMacAddress, brandTitle);
                if (!res) return NotFound();
            }

            #endregion

            #region Fill Model

            var model = await _inquiryService.ListOfInquiry(userMacAddress);
            if (model == null || !model.Any())
            {
                TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
                return RedirectToAction("Index", "Home");
            }

            #endregion

            ViewBag.pageId = pageId;
            ViewBag.userMacAddress = userMacAddress;
            ViewBag.OredrByPrice = orderByPrice;

            #region Oredr By Price Value

            if (orderByPrice == 1)
            {
                model = model.OrderByDescending(p => p.Price).ToList();
            }
            else if (orderByPrice == 2)
            {
                model = model.OrderBy(p => p.Price).ToList();
            }

            #endregion

            #region Order By Score

            if (orderByScore == 1)
            {
                model = model.OrderByDescending(p => p.Score).ToList();
            }
            else if (orderByScore == 2)
            {
                model = model.OrderBy(p => p.Score).ToList();
            }

            #endregion

            #region Paginaition

            int take = 50;

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
