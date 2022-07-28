using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Site.Inquiry;
using Window.Web.Models;

namespace Window.Web.Controllers
{
    public class HomeController : Controller
    {
        #region ctor

        private readonly ILogger<HomeController> _logger;

        private readonly IProductService _productService;

        private readonly IStateService _stateService;

        private readonly IBrandService _brandService;

        public HomeController(ILogger<HomeController> logger, IProductService prodcutService, IStateService stateService, IBrandService brandService)
        {
            _logger = logger;
            _productService = prodcutService;
            _stateService = stateService;
            _brandService = brandService;
        }

        #endregion

        #region Index 

        public async Task<IActionResult> Index(FilterInquiryViewModel filter)
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

            var model = await _productService.FilterInquiryViewModel(filter);

            return View(model);
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
    }
}