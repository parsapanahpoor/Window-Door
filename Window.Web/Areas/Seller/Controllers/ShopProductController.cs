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

    public async Task<IActionResult> FilterProducts(FilterShopProductSellerSideDTO filter , CancellationToken cancellation = default)
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

    #endregion

    #region Load Sub Categories

    public async Task<IActionResult> LoadSubCategories(ulong MainCategoryId , CancellationToken cancellationToken = default)
    {
        var result = await _shopCategoryService.GetCategoriesChildrent(MainCategoryId , cancellationToken);

        return JsonResponseStatus.Success(result);
    }

    #endregion
}
