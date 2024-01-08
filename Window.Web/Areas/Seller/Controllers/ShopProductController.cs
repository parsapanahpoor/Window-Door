using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Seller.Controllers;

public class ShopProductController : SellerBaseController
{
    #region Ctor

    private readonly IShopProductService _shopProductService;
    private readonly IShopCategoryService _shopCategoryService;
    private readonly IShopBrandsService _shopBrandsService;
    private readonly IShopColorService _shopColorService;

    public ShopProductController(IShopProductService shopProductService, 
                                 IShopCategoryService shopCategoryService,
                                 IShopColorService shopColorService  , 
                                 IShopBrandsService shopBrandsService)
    {
        _shopProductService = shopProductService;
        _shopCategoryService = shopCategoryService;
        _shopColorService = shopColorService;
        _shopBrandsService = shopBrandsService;
    }

    #endregion

    #region Filter Products

    public async Task<IActionResult> FilterShopProducts(FilterShopProductSellerSideDTO filter , CancellationToken cancellation = default)
    {
        filter.SellerUserId = User.GetUserId();

        return View(await _shopProductService.FilterShopProductSellerSide(filter , cancellation));
    }

    #endregion

    #region Create Product

    [HttpGet]
    public async Task<IActionResult> CreateProduct(CancellationToken cancellation = default)
    {
        #region View Bags

        ViewData["MianCategory"] = await _shopCategoryService.GetAllMainShopCategoriesCategories(cancellation);

        ViewData["Brands"] = await _shopBrandsService.FillListOfBrandsForFilterProductsDTO(cancellation);

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        #endregion

        return View();
    }

    [HttpPost , ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(CreateShopProductSellerSideDTO model , IFormFile NewsImage,  CancellationToken cancellation = default)
    {
        #region Model State Validation 

        if (!ModelState.IsValid)
        {
            #region View Bags

            ViewData["MianCategory"] = await _shopCategoryService.GetAllMainShopCategoriesCategories(cancellation);

            ViewData["Brands"] = await _shopBrandsService.FillListOfBrandsForFilterProductsDTO(cancellation);

            ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion

        #region Add Product To The Data Base

        var res = await _shopProductService.AddShopProductToTheDataBase(User.GetUserId() , model , NewsImage, cancellation);
        switch (res)
        {
            case CreateShopProductFromSellerPanelResult.Success:
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(FilterShopProducts));

            case CreateShopProductFromSellerPanelResult.Faild:
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                break;

            case CreateShopProductFromSellerPanelResult.MainCategoryNotFound:
                TempData[ErrorMessage] = "دسته بندی های محصول انتخاب نشده است";
                break;

            case CreateShopProductFromSellerPanelResult.SellerIsNotFound:
                TempData[ErrorMessage] = "فروشنده ی مورد نظر یافت نشده است.";
                break;

            default:
                break;
        }

        #endregion

        #region View Bags

        ViewData["MianCategory"] = await _shopCategoryService.GetAllMainShopCategoriesCategories(cancellation);

        ViewData["Brands"] = await _shopBrandsService.FillListOfBrandsForFilterProductsDTO(cancellation);

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        #endregion

        return View(model);
    }

    #endregion

    #region Edit Product

    [HttpGet]
    public async Task<IActionResult> EditProduct(ulong productId , CancellationToken cancellation)
    {
        var product = await _shopProductService.FillEditShopProductSellerSideDTO(productId , User.GetUserId() , cancellation);
        if (product == null) return NotFound();

        #region View Bags

        ViewData["MianCategory"] = await _shopCategoryService.GetAllMainShopCategoriesCategories(cancellation);

        ViewData["SelectedCategories"] = await _shopProductService.GetShopProductSelectedCategories( productId , cancellation);

        ViewData["Brands"] = await _shopBrandsService.FillListOfBrandsForFilterProductsDTO(cancellation);

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        #endregion

        return View(product);
    }

    #endregion

    #region Load Sub Categories

    public async Task<IActionResult> LoadSubCategories(ulong MainCategoryId , CancellationToken cancellationToken = default)
    {
        var result = await _shopCategoryService.GetCategoriesChildrent(MainCategoryId , cancellationToken);

        return JsonResponseStatus.Success(result);
    }

    #endregion
}
