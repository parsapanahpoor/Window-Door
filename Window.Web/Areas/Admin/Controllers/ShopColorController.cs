using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers;

public class ShopColorController : AdminBaseController
{
	#region Ctor

	private readonly IShopColorService _shopColorService;

    public ShopColorController(IShopColorService shopColorService)
    {
            _shopColorService = shopColorService;
    }

	#endregion

	#region Filter Shop Color

	public async Task<IActionResult> FilterShopColors(FilterShopColorDTO filter , CancellationToken cancellation)
	{
		return View(await _shopColorService.FilterShopColor(filter , cancellation));
	}

	#endregion

	#region Create ShopColor

	[HttpGet]
	public IActionResult CreateShopColor()
	{
		return View();
	}

	[HttpPost , ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateShopColor(CreateShopColorDTO shopColor, CancellationToken cancellation = default)
	{
		#region Model State Validations

		if (!ModelState.IsValid)
		{
			TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشد";

			return View(shopColor);
		}

		#endregion

		var result = await _shopColorService.CreateShopColorAdminSide(shopColor, cancellation);
		switch (result)
		{
			case CreateShopColorResult.Success:
				TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
				return RedirectToAction(nameof(FilterShopColors));

			case CreateShopColorResult.Fail:
				TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
				break;
		}

		return View(shopColor);
	}

	#endregion

	#region Edit ShopColor

	public async Task<IActionResult> EditShopColor(ulong id, CancellationToken cancellation = default)
	{
		var result = await _shopColorService.FillEditShopCategoryDTO(id, cancellation);
		if (result == null) return NotFound();

		return View(result);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EditShopColor(EditShopColorDTO shopColor, CancellationToken cancellation = default)
	{
		if (!ModelState.IsValid)
		{
			TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمیباشد";
			return View(shopColor);
		}

		var result = await _shopColorService.EditShopColor(shopColor, cancellation);
		switch (result)
		{
			case EditShopColorResult.Success:
				TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
				return RedirectToAction("FilterShopColors");

			case EditShopColorResult.Fail:
				TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
				break;
		}

		return View(shopColor);
	}

	#endregion

	#region Delete ShopColor

	public async Task<IActionResult> DeleteShopColor(ulong shopColorId, CancellationToken cancellation)
	{
		var result = await _shopColorService.DeleteShopColor(shopColorId, cancellation);
		if (result) return JsonResponseStatus.Success();

		return JsonResponseStatus.Error();
	}

	#endregion
}
