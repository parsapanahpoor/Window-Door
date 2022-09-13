using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.Enums.SellerType;
using Window.Domain.ViewModels.Seller.Pricing;
using Window.Domain.ViewModels.Seller.Product;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region List Of Seller Product

        public async Task<IActionResult> FilterProducts(FilterProductSellerSideViewModel filter)
        {
            filter.UserId = User.GetUserId();
            return View(await _productService.FilterProductSellerSideViewModel(filter));
        }

        #endregion

        #region Create Product

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            #region Validation For Add New Product

            if (!await _productService.GetSellerTypeForValidAddProduct(User.GetUserId()))
            {
                TempData[ErrorMessage] = "کاربر عزیز شما حد مجاز برای درج محصول را تکمبل کرده اید . ";
                return RedirectToAction(nameof(FilterProducts));
            }

            #endregion

            ViewBag.Brands = await _productService.LoadBrands();

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductSellerSideViewModel model)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد .";
                return RedirectToAction("Index" , "Home" , new { area = "Seller"});
            }

            #endregion

            #region Validation For Add New Product

            if (!await _productService.GetSellerTypeForValidAddProductStep2(User.GetUserId() , model.SellerType))
            {
                TempData[ErrorMessage] = "کاربر عزیز شما حد مجاز برای درج محصول را تکمبل کرده اید . ";
                return RedirectToAction(nameof(FilterProducts));
            }

            #endregion

            #region Add Method

            var productId = await _productService.CreateProduct(model, User.GetUserId());

            if (productId == null || productId == 0)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد .";
                return RedirectToAction("Index", "Home", new { area = "Seller" });
            }
            else
            {
                TempData[SuccessMessage] = "افزودن محصول با موفقیت انجام شده است . ";
                return RedirectToAction(nameof(FilterProducts));
            }

            #endregion

            ViewBag.Brands = await _productService.LoadBrands();

            return View(model);
        }

        #endregion

        #region Add Segment Pricing

        [HttpGet]
        public async Task<IActionResult> SegmentPricing(ulong productId)
        {
            #region Fill View Model

            var model = await _productService.FillSegmentPricingViewModel(productId , User.GetUserId());
            if (model == null) return NotFound();

            #endregion

            #region Segment Pricing

            ViewBag.SegmentPRicing = await _productService.FillSegmentPricing(productId , User.GetUserId());

            #endregion

            return View(model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSegmentPricing(ulong ProductId , ulong SegmentId , int Price)
        {
            #region Add Pricing

            var res = await _productService.AddPricingForSegment(ProductId , SegmentId , Price , User.GetUserId());

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(SegmentPricing) , new { productId = ProductId });
            }

            #endregion

            TempData[ErrorMessage] = "عملیات با شکست روبرو شده است .";
            return RedirectToAction(nameof(SegmentPricing), new { productId = ProductId });
        }

        #endregion

        #region Create Glass Pricing  

        [HttpGet]
        public async Task<IActionResult> CreateGlassPricing()
        {
            #region Fill View Model

            var model = await _productService.FillGlassPricingEntityViewModel(User.GetUserId());
            if (model == null) return NotFound();

            #endregion

            #region Glass Pricing

            ViewBag.GlassPricing = await _productService.FillGlassPricing(User.GetUserId());

            #endregion

            return View(model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGlassPricing(ulong GlassId , int Price)
        {
            #region Fill View Model

            var model = await _productService.FillGlassPricingEntityViewModel(User.GetUserId());
            if (model == null) return NotFound();

            #endregion

            #region Glass Pricing

            ViewBag.GlassPricing = await _productService.FillGlassPricing(User.GetUserId());

            #endregion

            #region Add Pricing

            var res = await _productService.AddPricingForGlass(GlassId, Price, User.GetUserId());

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(CreateGlassPricing));
            }

            #endregion

            TempData[ErrorMessage] = "عملیات با شکست روبرو شده است .";
            return RedirectToAction(nameof(CreateGlassPricing));
        }

        #endregion

        #region Delete Product 

        public async Task<IActionResult> DeleteProduct(ulong Id)
        {
            var res = await _productService.DeleteProductById(Id , User.GetUserId());

            if (res)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
