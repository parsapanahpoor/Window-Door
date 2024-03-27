using Window.Application.Interfaces;
using Window.Domain.ViewModels.User;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Application.Extensions;
using Window.Domain.ViewModels.Admin.Log;

namespace Window.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        #region constructor

        private readonly IConfiguration _configuration;
        public IUserService _userService;
        private readonly IProductService _productService;
        private ISellerService _sellerService;
        private readonly IContractService _contractService;

        public HomeController(IConfiguration configuration, IUserService userService, IProductService productService, ISellerService sellerService
                                , IContractService contractService)
        {
            _configuration = configuration;
            _userService = userService;
            _productService = productService;
            _sellerService = sellerService;
            _contractService = contractService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index(bool OrderFinalization = false)
        {
            #region Check Is Exist Any Market By This User

            var market = await _sellerService.GetMarketByUserId(User.GetUserId());
            if (market == null)
            {
                TempData[ErrorMessage] = "کاربر عزیز لطفا در ابتدا فروشگاه خود را ثبت کنید.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            #endregion

            #region Check User Charge

            var res = await _sellerService.CheckUserCharge(User.GetUserId());
            if (res == false) return NotFound();

            #endregion

            #region Inquiry OF Seller Logs

            ViewBag.CountOfTodayUserInInquiry = await _sellerService.CountOfTodayUserInInquiry(User.GetUserId());
            ViewBag.CountOfMonthUserInInquiry = await _sellerService.CountOfMonthUserInInquiry(User.GetUserId());
            ViewBag.CountOfYearUserInInquiry = await _sellerService.CountOfYearUserInInquiry(User.GetUserId());
            ViewBag.CountOfSellerWaitingRequest = await _contractService.CountOfUserWaitingContractRequest(User.GetUserId());

            #endregion

            #region Empty Shop Name 

            var user = await _userService.GetUserById(User.GetUserId());

            if (string.IsNullOrEmpty(user.ShopName))
            {
                ViewBag.ShopNameIsEmpty = true;
            }

            #endregion

            #region Notification

            if (OrderFinalization)
            {
                TempData[SwalSuccess] = "سفارش باموفقیت تایید شده است. لطفا هرچه سریع تر نسبت به آماده سازی اقلام خریداری شده اقدام فرمایید.";
            }

            #endregion

            return View();
        }

        #endregion

        #region SearchUserModal

        public async Task<IActionResult> SearchUserModal(FilterUserViewModel filter, string baseName)
        {
            filter.TakeEntity = 5;

            var result = await _userService.FilterUsers(filter);

            ViewBag.BaseName = baseName;

            return PartialView("_FilterUsersModalPartial", result);
        }

        #endregion

        #region Load Product Categories

        public async Task<IActionResult> LoadProductTypeCategories(ulong productTypeId)
        {
            var result = await _productService.GetProductTypeCategories(productTypeId);

            return JsonResponseStatus.Success(result);
        }

        #endregion

        #region Load Brands For DrowpDown 

        public async Task<IActionResult> LoadBrands(ulong sellerTypeId)
        {
            var result = await _productService.LoadBrands();

            return JsonResponseStatus.Success(result);
        }

        #endregion

        #region Filter Log 

        public async Task<IActionResult> FilterLogForVisitSellerProfile(FilterLogVisitSellerProfileViewModel filter)
        {
            filter.SellerId = User.GetUserId();
            return View(await _sellerService.FilterLogVisitSellerProfile(filter));
        }

        #endregion
    }
}
