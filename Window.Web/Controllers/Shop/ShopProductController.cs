using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.ShopProduct.Query;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;
namespace Window.Web.Controllers;

public class ShopProductController : SiteBaseController
{
    #region Ctor

    private readonly IShopCategoryService _shopCategoryService;
    private readonly IShopColorService _shopColorService;
    private readonly IBrandService _brandService;

    public ShopProductController(IShopCategoryService shopCategoryService,
                                 IShopColorService shopColorService,
                                 IBrandService brandService)
    {
        _shopCategoryService = shopCategoryService;
        _shopColorService = shopColorService;
        _brandService = brandService;

    }

    #endregion

    #region List OF Products

    [HttpGet]
    public async Task<IActionResult> FilterProducts(FilterShopProductDTO filter,
                                                    CancellationToken cancellationToken = default)
    {
        #region Fill Query

        FilterProductsQuery query = new FilterProductsQuery()
        {
            BrandId = filter.BrandId,
            ColorsId = filter.ColorsId,
            MaxPrice = filter.MaxPrice,
            MinPrice = filter.MinPrice,
            ProductTitle = filter.ProductTitle,
            ShopCategoryId = filter.shopCategories,
        };

        #endregion

        #region View Data

        if (filter.ShopCategoryParentId.HasValue )
        {
            ViewData["ListOfShopCategories"] = await _shopCategoryService.FillShopCategoriesForShowInFilterShopProduct(filter.ShopCategoryParentId.Value, cancellationToken);
            ViewData["SelectedParentCategoryMainTitle"] = await _shopCategoryService.GetShopCategoryTitle(filter.ShopCategoryParentId.Value, cancellationToken);
        }

        if (filter.shopCategories != null && filter.shopCategories.Any() && filter.shopCategories.Count() == 1)
        {
            ViewData["SelectedParentCategoryTitle"] = await _shopCategoryService.GetShopCategoryTitle(filter.shopCategories.First(), cancellationToken);
        }

        ViewData["Brands"] = await _brandService.FillListOfBrandsForFilterProductsDTO(cancellationToken);

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellationToken);

        #endregion

        return View(await Mediator.Send(query , cancellationToken));
    }

    #endregion

    #region ShopProduct Detail

    [HttpGet]
    public async Task<IActionResult> ShopProductDetail(ShopProductDetailQuery query , CancellationToken cancellationToken=default)
    {
        var model = await Mediator.Send(query , cancellationToken);
        if (model == null)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction("Index" , "Home");
        }

        return View(model);
    }

    #endregion
}
