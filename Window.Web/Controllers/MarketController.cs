using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Market;

namespace Window.Web.Controllers
{
    public class MarketController : SiteBaseController
    {
        #region Ctor

        private readonly IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        #endregion

        #region Request For Add Market 

        [HttpGet]
        public async Task<IActionResult> AddMarket()
        {
            return View();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMarket(Market market)
        {
            #region Add Market

            var res = await _marketService.AddMarket(User.GetUserId() , market.MarketName);

            if (res == false)
            {
                TempData[ErrorMessage] = "عملیات ناموفق بوده است .";
                return View();
            }
            else
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction("Index", "Home");
            }

            #endregion

            return View();
        }

        #endregion
    }
}
