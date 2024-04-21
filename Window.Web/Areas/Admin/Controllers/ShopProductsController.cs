using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.EditShopProduct;
using Window.Application.CQRS.AdminPanel.ShopProducts.Query.EditShopProduct;
using Window.Application.CQRS.AdminPanel.ShopProducts.Query.FilterShopProducts;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Web.Areas.Admin.Controllers;

public class ShopProductsController : AdminBaseController
{
    #region Ctor

    private readonly ISiteSettingService _siteSettingService;
    private readonly IShopColorService _shopColorService;

    public ShopProductsController(ISiteSettingService siteSettingService,
                                  IShopColorService shopColorService)
    {
        _siteSettingService = siteSettingService;
        _shopColorService = shopColorService;
    }

    #endregion

    #region Filter Shop Products

    [HttpGet]
    public async Task<IActionResult> FilterShopProducts(FilterShopProductsAdminSideDTO filter, CancellationToken cancellation)
    {
        return View(await Mediator.Send(new FilterShopProductsAdminSideQuery()
        {
            filter = filter
        },
        cancellation));
    }

    #endregion

    #region Edit Product

    [HttpGet]
    public async Task<IActionResult> EditProduct(ulong productId, CancellationToken cancellation)
    {
        var product = await Mediator.Send(new EditShopProductQuery()
        {
            ProductId = productId,
        },
        cancellation);

        if (product == null) return NotFound();

        #region View Bags

        ViewData["SaleScales"] = await _siteSettingService.ListOfSalesScales();

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        #endregion

        return View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(EditShopProductAdminSideDTO model,
                                                 IFormFile? NewsImage,
                                                 CancellationToken cancellation)
    {
        #region Edit Product

        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(new EditShopProductCommand()
            {
                model = model,
                ProductImage = NewsImage
            },
            cancellation);

            switch (res)
            {
                case EditShopProductFromAdminPanelResult.Success:
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                    return RedirectToAction(nameof(FilterShopProducts));

                case EditShopProductFromAdminPanelResult.Faild:
                    TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                    break;

                case EditShopProductFromAdminPanelResult.MainCategoryNotFound:
                    TempData[ErrorMessage] = "دسته بندی های محصول انتخاب نشده است";
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region View Bags

        ViewData["SaleScales"] = await _siteSettingService.ListOfSalesScales();

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        #endregion

        return View(model);
    }

    #endregion
}
