﻿using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.State;

namespace Window.Web.Areas.Admin.Controllers;

public class ShopCategoryController : AdminBaseController
{
	#region Ctor

	private readonly IShopCategoryService _shopCategoryService;

    public ShopCategoryController(IShopCategoryService shopCategoryService)
    {
            _shopCategoryService = shopCategoryService;
    }

	#endregion

	#region Filter

	public async Task<IActionResult> FilterShopCategory(FilterShopCategoryDTO filter)
	{ 
		return View(await _shopCategoryService.FilterShopCategory(filter));	
	}

    #endregion

    #region Create State

    [HttpGet]
    public async Task<IActionResult> CreateShopCategory(ulong? parentId , CancellationToken cancellation = default)
    {
        ViewBag.parentId = parentId;

        if (parentId != null)
        {
            ViewBag.parentState = await _shopCategoryService.GetShopCategoryById(parentId.Value , cancellation );
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateShopCategory(CreateShopCategoriesDTO shopCategory, CancellationToken cancellation = default)
    {
        #region Model State Validations

        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشد";
            if (shopCategory.ParentId != null)
            {
                ViewBag.parentState = await _shopCategoryService.GetShopCategoryById(shopCategory.ParentId.Value, cancellation);
            }

            return View(shopCategory);
        }

        #endregion

        var result = await _shopCategoryService.CreateShopCategoryAdminSide(shopCategory , cancellation);
        switch (result)
        {
            case CreateShopCategoryResult.Success:
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                if (shopCategory.ParentId.HasValue)
                {
                    return RedirectToAction("FilterState", new { ParentId = shopCategory.ParentId.Value });
                }
                return RedirectToAction("FilterState");

            case CreateShopCategoryResult.Fail:
                TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                break;
        }

        ViewBag.parentId = shopCategory.ParentId;

        if (shopCategory.ParentId != null)
        {
            ViewBag.parentState = await _shopCategoryService.GetShopCategoryById(shopCategory.ParentId.Value, cancellation);
        }

        return View(shopCategory);
    }

    #endregion
}
