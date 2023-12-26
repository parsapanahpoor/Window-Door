using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers;

public class ShopBrandController : AdminBaseController
{
    #region Ctor

    private readonly IShopBrandsService  _shopBrandsService;

    public ShopBrandController(IShopBrandsService shopBrandsService)
    {
        _shopBrandsService = shopBrandsService;
    }

    #endregion

    #region Filter Shop Brand

    public async Task<IActionResult> FilterShopBrands(FilterShopBrandDTO filter, CancellationToken cancellation)
    {
        return View(await _shopBrandsService.FilterShopBrand(filter, cancellation));
    }

    #endregion

    #region Create ShopBrand

    [HttpGet]
    public IActionResult CreateShopBrand()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateShopBrand(CreateShopBrandDTO shopBrand, CancellationToken cancellation = default)
    {
        #region Model State Validations

        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشد";

            return View(shopBrand);
        }

        #endregion

        var result = await _shopBrandsService.CreateShopBrandAdminSide(shopBrand, cancellation);
        switch (result)
        {
            case CreateShopBrandResult.Success:
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                return RedirectToAction(nameof(FilterShopBrands));

            case CreateShopBrandResult.Fail:
                TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                break;
        }

        return View(shopBrand);
    }

    #endregion

    #region Edit ShopBrand

    public async Task<IActionResult> EditShopBrand(ulong id, CancellationToken cancellation = default)
    {
        var result = await _shopBrandsService.FillEditShopCategoryDTO(id, cancellation);
        if (result == null) return NotFound();

        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditShopBrand(EditShopBrandDTO shopBrand, CancellationToken cancellation = default)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمیباشد";
            return View(shopBrand);
        }

        var result = await _shopBrandsService.EditShopBrand(shopBrand, cancellation);
        switch (result)
        {
            case EditShopBrandResult.Success:
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                return RedirectToAction("FilterShopBrands");

            case EditShopBrandResult.Fail:
                TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                break;
        }

        return View(shopBrand);
    }

    #endregion

    #region Delete ShopBrand

    public async Task<IActionResult> DeleteShopBrand(ulong shopBrandId, CancellationToken cancellation)
    {
        var result = await _shopBrandsService.DeleteShopBrand(shopBrandId, cancellation);
        if (result) return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion
}
