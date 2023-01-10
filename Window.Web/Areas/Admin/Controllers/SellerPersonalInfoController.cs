using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.PersonalInfo;
using Window.Domain.ViewModels.Seller.PersonalInfo;
using Window.Application.Interfaces;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageSellersInfo")]
    public class SellerPersonalInfoController : AdminBaseController
    {
        #region Ctor

        private readonly ISellerService _sellerService;

        private readonly IStateService _stateService;

        private readonly IUserService _userService;

        public SellerPersonalInfoController(ISellerService sellerService, IStateService stateService
                    , IUserService userService)
        {
            _sellerService = sellerService;
            _stateService = stateService;
            _userService = userService;
        }

        #endregion

        #region List Of Seller Personal Infos

        public async Task<IActionResult> ListOfSellersPersonalInfos(ListOfSellersInfoViewModel filter)
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

            return View(await _sellerService.FilterPersonalInfo(filter));
        }

        #endregion

        #region Seller Personal Info Detail 

        [HttpGet]
        public async Task<IActionResult> SellerPersonalInfoDetail(ulong id)
        {
            #region Get market 

            var market = await _sellerService.GetMarketByMarketId(id);
            if (market == null) return NotFound();

            ViewBag.marketId = market.Id;

            #endregion

            #region Fill Model 

            var model = await _sellerService.FillListOfPersonalInfoViewModel(market.UserId);
            if (model == null)
            {
                return NotFound();
            }

            #endregion

            ViewData["Countries"] = await _stateService.GetAllCountries();
            ViewData["States"] = await _stateService.GetStateChildren(model.CountryId);
            ViewData["Cities"] = await _stateService.GetStateChildren(model.StateId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SellerPersonalInfoDetail(ListOfPersonalInfoViewModel model , ulong marketId)
        {
            #region Get market 

            var market = await _sellerService.GetMarketByMarketId(marketId);
            if (market == null) return NotFound();


            #endregion

            #region Change State Market From Admin Panel 

            var res = await _sellerService.ChangeMarketStateFromAdminPanel(model, marketId);

            if (res == true)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است .";
                return RedirectToAction(nameof(ListOfSellersPersonalInfos));
            }

            #region Fill Model 

            var viewModel = await _sellerService.FillListOfPersonalInfoViewModel(market.UserId);

            #endregion

            ViewBag.marketId = market.Id;

            TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
            return View(viewModel);

            #endregion
        }

        #endregion

        #region LoginWithUser

        public async Task<IActionResult> LoginWithUser(ulong userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = false };

            await HttpContext.SignInAsync(principal, properties);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        #endregion
    }
}
