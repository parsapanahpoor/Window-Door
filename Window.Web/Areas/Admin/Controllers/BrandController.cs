#region properties

using Microsoft.AspNetCore.Mvc;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Brand;
using Window.Domain.ViewModels.Admin.Brand;
using Window.Web.HttpManager;
namespace Window.Web.Areas.Admin.Controllers;

#endregion

[PermissionChecker("ManageBrands")]
public class BrandController : AdminBaseController
{
    #region ctor

    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    #endregion

    #region Main Brand 

    #region Lits Of Main Brands

    public async Task<IActionResult> FilterMainBrand(FilterMainBrandViewModel filter)
    {
        return View(await _brandService.FilterMainBrandViewModel(filter));
    }

    #endregion

    #region Create Main Brand 

    [HttpGet]
    public async Task<IActionResult> CreateMainBrand()
    {
        TempData["BrandCategories"] = await _brandService.GetListOfBrandCategories();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMainBrand(MainBrand brand,
                                                     IFormFile? brandLogo )
    {       
        #region Create Brand Method

        var res = await _brandService.CreateMainBrand(brand, brandLogo);

        if (res)
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
            return RedirectToAction(nameof(FilterMainBrand));
        }

        #endregion

        TempData["BrandCategories"] = await _brandService.GetListOfBrandCategories();
        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";
        return View(brand);
    }

    #endregion

    #region Edit Main Brand 

    [HttpGet]
    public async Task<IActionResult> EditMainBrand(ulong brandId )
    {
        #region Get Brand By Id 

        var brand = await _brandService.GetMainBrandById(brandId);
        if (brand == null) return NotFound();

        TempData["BrandCategories"] = await _brandService.GetListOfBrandCategories();

        #endregion

        return View(brand);
    }

    [HttpPost]
    public async Task<IActionResult> EditMainBrand(MainBrand brand, 
                                                   IFormFile? brandLogo)
    {
        #region Update Method 

        var res = await _brandService.UpdateMainBrand(brand, brandLogo);

        if (res)
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
            return RedirectToAction(nameof(FilterMainBrand));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";
        TempData["BrandCategories"] = await _brandService.GetListOfBrandCategories();

        return View(brand);
    }

    #endregion

    #region Delete Main Brand 

    public async Task<IActionResult> DeleteArticle(ulong Id)
    {
        var result = await _brandService.DeleteMainBrand(Id);

        if (result)
        {
            return JsonResponseStatus.Success();
        }

        return JsonResponseStatus.Error();
    }
    #endregion

    #endregion

    #region Yaragh Brand 

    #region Lits Of Main Brands

    public async Task<IActionResult> FilterYaraghBrand(FilterYaraghBrandViewModel filter)
    {
        return View(await _brandService.FilterYaraghBrandViewModel(filter));
    }

    #endregion

    #region Create Yaragh Brand 

    [HttpGet]
    public async Task<IActionResult> CreateYaraghBrand()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateYaraghBrand(YaraghBrand brand, IFormFile? brandLogo)
    {
    

        #region Create Brand Method

        var res = await _brandService.CreateYaraghBrand(brand, brandLogo);

        if (res)
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
            return RedirectToAction(nameof(FilterYaraghBrand));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";

        return View(brand);
    }

    #endregion

    #region Edit Main Brand 

    [HttpGet]
    public async Task<IActionResult> EditYaraghBrand(ulong brandId)
    {
        #region Get Brand By Id 

        var brand = await _brandService.GetYaraghBrandById(brandId);

        if (brand == null) return NotFound();

        #endregion

        return View(brand);
    }

    [HttpPost]
    public async Task<IActionResult> EditYaraghBrand(YaraghBrand brand, IFormFile? brandLogo)
    {

        #region Update Method 

        var res = await _brandService.UpdateYaraghBrand(brand, brandLogo);

        if (res)
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
            return RedirectToAction(nameof(FilterYaraghBrand));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";

        return View(brand);
    }

    #endregion

    #region Delete Main Brand 

    public async Task<IActionResult> DeleteYaraghBrand(ulong Id)
    {
        var result = await _brandService.DeleteYaraghBrand(Id);

        if (result)
        {
            return JsonResponseStatus.Success();
        }

        return JsonResponseStatus.Error();
    }

    #endregion

    #endregion
}
