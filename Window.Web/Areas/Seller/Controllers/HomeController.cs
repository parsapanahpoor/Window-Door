using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Domain.ViewModels.User;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;

namespace Window.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        #region constructor

        private readonly IConfiguration _configuration;

        public IUserService _userService;

        private readonly IProductService _productService;

        public HomeController(IConfiguration configuration, IUserService userService , IProductService productService)
        {
            _configuration = configuration;
            _userService = userService;
            _productService = productService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index()
        {
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
            var result = await _productService.LoadBrands(sellerTypeId);

            return JsonResponseStatus.Success(result);
        }

        #endregion
    }
}
