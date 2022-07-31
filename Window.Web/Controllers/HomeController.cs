using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Site.Inquiry;
using Window.Web.HttpManager;
using Window.Web.Models;
using System.Text.Json;
using Window.Application.Extensions;
using Window.Domain.Enums.SellerType;

namespace Window.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region ctor

        private readonly ILogger<HomeController> _logger;

        private readonly IProductService _productService;

        private readonly IStateService _stateService;

        private readonly IBrandService _brandService;

        private readonly ISampleService _sampleService;

        public HomeController(ILogger<HomeController> logger, IProductService prodcutService, IStateService stateService, IBrandService brandService, ISampleService sampleService)
        {
            _logger = logger;
            _productService = prodcutService;
            _stateService = stateService;
            _brandService = brandService;
            _sampleService = sampleService;
        }

        #endregion

        #region Index 

        public async Task<IActionResult> Index(FilterInquiryViewModel filter , bool redirect = false)
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

            #region Sample View Bag

            if (filter.ProductKind == null && filter.ProductKind == null)
            {
                ViewData["Sample"] = await _sampleService.GetAllSample();
            }

            if (filter.ProductKind.HasValue && filter.ProductType.HasValue)
            {
                ViewData["Sample"] = await _sampleService.GetAllSampleUsingProductTypeAndProductKind(filter.ProductKind.Value, filter.ProductType.Value);
            }

            if (filter.ProductKind != null && !filter.ProductType.HasValue)
            {
                ViewData["Sample"] = await _sampleService.GetAllDoorAndWindowSample(filter.ProductKind.Value);
            }

            if (filter.ProductType != null && !filter.ProductKind.HasValue)
            {
                ViewData["Sample"] = await _sampleService.GetAlllolaieKeshoieSample(filter.ProductType.Value);
            }

            #endregion

            #region Brand ViewBag

            ViewBag.Brand = await _brandService.GetAllBrands();

            #endregion

            #region Set Session

            if (redirect == false)
            {
                if (HttpContext.Session.GetString("SellerFilter") == null)
                {
                    SellersFieldFitreViewModel sellerFilter = new SellersFieldFitreViewModel();

                    sellerFilter.CountryId = filter.CountryId;
                    sellerFilter.StateId = filter.StateId;
                    sellerFilter.CityId = filter.CityId;
                    sellerFilter.SellerType = filter.SellerType;
                    sellerFilter.BrandId = filter.MainBrandId;
                    sellerFilter.UserId = User.GetUserId();

                    HttpContext.Session.SetString("SellerFilter", JsonSerializer.Serialize(sellerFilter));
                }
                else
                {
                    var value = HttpContext.Session.GetString("SellerFilter");
                    var list = JsonSerializer.Deserialize<SellersFieldFitreViewModel>(value);

                    if (list.UserId == User.GetUserId())
                    {
                        list.CountryId = filter.CountryId;
                        list.StateId = filter.StateId;
                        list.CityId = filter.CityId;
                        list.SellerType = filter.SellerType;
                        list.BrandId = filter.MainBrandId;
                        list.UserId = User.GetUserId();
                    }

                    HttpContext.Session.SetString("SellerFilter", JsonSerializer.Serialize(list));
                }
            }

            #endregion

            return View();
        }

        #endregion

        #region Get Sample Size 

        public async Task<IActionResult> GetSampleSize(SampleSizeViewModel sample)
        {
            sample.UserId = User.GetUserId();

            if (ModelState.IsValid == false)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            if (HttpContext.Session.GetString("SampleSize") == null)
            {
                #region Set Sample Session

                List<SampleSizeViewModel> sampleViewModel = new List<SampleSizeViewModel>();
                sampleViewModel.Add(sample);

                HttpContext.Session.SetString("SampleSize", JsonSerializer.Serialize(sampleViewModel));

                #endregion

                TempData[SuccessMessage] = "افزودن محصول با موفقیت انجام شده است ";
                return RedirectToAction(nameof(Index) , new { redirect  = true});
            }
            else
            {
                var value = HttpContext.Session.GetString("SampleSize");
                var list = JsonSerializer.Deserialize<List<SampleSizeViewModel>>(value);

                if (list.Any(p => p.SampleId == sample.SampleId && p.UserId == sample.UserId) == false)
                {
                    list.Add(sample);

                    HttpContext.Session.SetString("SampleSize", JsonSerializer.Serialize(list));

                    TempData[SuccessMessage] = "افزودن محصول با موفقیت انجام شده است ";
                    return RedirectToAction(nameof(Index), new { redirect = true });
                }

                TempData[ErrorMessage] = "این محصول از قبل انتخاب شده است";
                return RedirectToAction(nameof(Index), new { redirect = true });
            }

            TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
            return RedirectToAction(nameof(Index), new { redirect = true });
        }

        #endregion

        #region Inquiry

        public async Task<IActionResult> Inquiry()
        {
            #region Seler Filter

            SellersFieldFitreViewModel sellerFilter = new SellersFieldFitreViewModel();

            if (HttpContext.Session.GetString("SellerFilter") == null)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            if (HttpContext.Session.GetString("SellerFilter") != null)
            {
                var value = HttpContext.Session.GetString("SellerFilter");
                var list = JsonSerializer.Deserialize<SellersFieldFitreViewModel>(value);

                if (list.UserId == User.GetUserId())
                {
                    sellerFilter = list;
                }
            }

            #endregion

            #region Sample Size 

            List<SampleSizeViewModel> sampleViewModel = new List<SampleSizeViewModel>();

            if (HttpContext.Session.GetString("SampleSize") == null)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            if (HttpContext.Session.GetString("SampleSize") != null)
            {
                var value = HttpContext.Session.GetString("SampleSize");
                var list = JsonSerializer.Deserialize<List<SampleSizeViewModel>>(value);

                if (list.Any(p => p.UserId == User.GetUserId()) == true)
                {
                    sampleViewModel = list.Where(p => p.UserId == User.GetUserId()).ToList();
                }
            }

            #endregion

            #region Model State Validation 

            if (sellerFilter == null)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            if (sampleViewModel == null)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            #endregion

            #region Fill Model

            var model = await _productService.ListOfInquiry(sampleViewModel, sellerFilter);
            if (model == null)
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                return RedirectToAction(nameof(Index));
            }

            #endregion

            return View();
        }

        #endregion

        #region ChangeLanguage

        [AllowAnonymous]
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(2) });
            var refereUrl = Request.Headers["Referer"].ToString().Replace("?changeLang=true", "").Replace("&changeLang=true", "");

            return Redirect(refereUrl);
        }

        #endregion

        #region Load Cities

        public async Task<IActionResult> LoadCities(ulong stateId)
        {
            var result = await _stateService.GetStateChildren(stateId);

            return JsonResponseStatus.Success(result);
        }

        #endregion
    }
}