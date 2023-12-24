using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
namespace Window.Web.ViewComponents;

public class ShopCategoriesBigPartViewComponent : ViewComponent
{
    #region Ctor

    private readonly IShopCategoryService _shopCategoryService;

    public ShopCategoriesBigPartViewComponent(IShopCategoryService shopCategoryService)
    {
        _shopCategoryService = shopCategoryService;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        return View("ShopCategoriesBigPart", await _shopCategoryService.FillLargSideShopCategoriesDTO(cancellationToken));
    }

}

public class ShopCategoriesShortPartViewComponent : ViewComponent
{
    #region Ctor

    private readonly IShopCategoryService _shopCategoryService;

    public ShopCategoriesShortPartViewComponent(IShopCategoryService shopCategoryService)
    {
        _shopCategoryService = shopCategoryService;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        return View("ShopCategoriesShortPart", await _shopCategoryService.FillShopCategoriesDTO(cancellationToken));
    }

}
