using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Site.Shop.Landing;
namespace Window.Web.ViewComponents;

public class ShopCategoriesBigPartViewComponent : ViewComponent
{
    #region Ctor

    private readonly IShopCategoryService _shopCategoryService;
    private readonly IBrandService _brandService;

    public ShopCategoriesBigPartViewComponent(IShopCategoryService shopCategoryService, 
                                              IBrandService brandService)
    {
        _shopCategoryService = shopCategoryService;
        _brandService = brandService;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        ShopSiteBarDTO model = new ShopSiteBarDTO()
        {
            ShopCategoriesDTOs = await _shopCategoryService.FillLargSideShopCategoriesDTO(cancellationToken),
            ShopBrandsDTOs = await _brandService.FillShopBrandsDTOForSiteSideBar(cancellationToken)
        };

        return View("ShopCategoriesBigPart", model);
    }

}

public class ShopCategoriesShortPartViewComponent : ViewComponent
{
    #region Ctor

    private readonly IShopCategoryService _shopCategoryService;
    private readonly IBrandService _brandService;

    public ShopCategoriesShortPartViewComponent(IShopCategoryService shopCategoryService,
                                                IBrandService brandService)
    {
        _shopCategoryService = shopCategoryService;
        _brandService = brandService;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        ShopSiteBarDTO model = new ShopSiteBarDTO()
        {
            ShopCategoriesDTOs = await _shopCategoryService.FillLargSideShopCategoriesDTO(cancellationToken),
            ShopBrandsDTOs = await _brandService.FillShopBrandsDTOForSiteSideBar(cancellationToken)
        };

        return View("ShopCategoriesShortPart", model);
    }

}
