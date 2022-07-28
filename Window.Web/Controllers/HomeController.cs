using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Window.Application.Services.Interfaces;
using Window.Web.Models;

namespace Window.Web.Controllers
{
    public class HomeController : Controller
    {
        #region ctor

        private readonly ILogger<HomeController> _logger;

        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger , IProductService prodcutService)
        {
            _logger = logger;
            _productService = prodcutService;
        }

        #endregion

        #region Index 

        public IActionResult Index()
        {
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
    }
}