using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Web.Controllers;

public class ShopProductController : SiteBaseController
{
    #region Ctor

    private readonly IShopCategoryService _shopCategoryService;

    public ShopProductController(IShopCategoryService shopCategoryService)
    {
        _shopCategoryService = shopCategoryService;
    }

    #endregion

    #region List OF Products

    [HttpGet]
    public async Task<IActionResult> FilterProducts(FilterShopProductDTO filter , CancellationToken cancellationToken = default)
    {
        #region View Data

        if (filter.ShopCategoryParentId.HasValue)
        {
            ViewData["ListOfShopCategories"] = await _shopCategoryService.FillShopCategoriesForShowInFilterShopProduct(filter.ShopCategoryParentId.Value , cancellationToken);
            ViewData["SelectedParentCategoryTitle"] = await _shopCategoryService.GetShopCategoryTitle(filter.ShopCategoryParentId.Value , cancellationToken);
        }

        #endregion

        return View();
    }

    #endregion
}
