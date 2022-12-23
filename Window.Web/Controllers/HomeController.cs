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
using System.Text.Json.Serialization;
using System.Net;

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

        public async Task<IActionResult> Index()
        {

        

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
                return RedirectToAction(nameof(Index), new { redirect = true });
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

        public async Task<IActionResult> Inquiry(ulong? MainBrandId, SellerType? SellerType, int pageId = 1)
        {
            #region Brand ViewBag

            ViewBag.Brand = await _brandService.GetAllBrands();

            #endregion

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
                    if (MainBrandId.HasValue)
                    {
                        list.BrandId = MainBrandId.Value;
                    }

                    if (SellerType != null)
                    {
                        list.SellerType = SellerType;
                    }

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

            ViewBag.pageId = pageId;

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