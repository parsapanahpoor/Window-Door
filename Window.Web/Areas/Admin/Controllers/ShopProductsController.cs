using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.CreateProductGallery;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.CreateShopProductFeature;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.DeleteProductGallery;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.DeleteShopProductFeature;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.EditShopProduct;
using Window.Application.CQRS.AdminPanel.ShopProducts.Command.ProductBrandsCommand;
using Window.Application.CQRS.AdminPanel.ShopProducts.Query.EditShopProduct;
using Window.Application.CQRS.AdminPanel.ShopProducts.Query.FilterShopProducts;
using Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductFeature;
using Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductGallery;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers;

public class ShopProductsController : AdminBaseController
{
    #region Ctor

    private readonly ISiteSettingService _siteSettingService;
    private readonly IShopColorService _shopColorService;
    private readonly IShopProductService _shopProductService;
    private readonly IShopCategoryService _shopCategoryService;
    private readonly IBrandService _brandService;

    public ShopProductsController(ISiteSettingService siteSettingService,
                                  IShopColorService shopColorService ,
                                  IShopProductService shopProductService , 
                                  IShopCategoryService shopCategoryService ,
                                  IBrandService brandService)
    {
        _siteSettingService = siteSettingService;
        _shopColorService = shopColorService;
        _shopProductService = shopProductService;
        _shopCategoryService = shopCategoryService;
        _brandService = brandService;
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
                                                 CancellationToken cancellation)
    {
        #region Edit Product

        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(new EditShopProductCommand()
            {
                model = model,
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

    #region Delete Shop Product 

    public async Task<IActionResult> DeleteShopProduct(ulong shopProductId, CancellationToken cancellation)
    {
        var result = await _shopProductService.DeleteProduct_AdminSide(shopProductId, cancellation);
        if (result)
        {
            return JsonResponseStatus.Success();
        }

        return JsonResponseStatus.Error();
    }

    #endregion

    #region Edit Product Category

    [HttpGet]
    public async Task<IActionResult> EditProductCategory(ulong productId, CancellationToken cancellation)
    {
        ViewData["ProductId"] = productId;

        return View(await _shopProductService.FillListOfSellerProductCategoriesDTO(User.GetUserId(), productId, cancellation));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProductCategory(ulong productId, List<ulong>? Permissions, CancellationToken cancellation)
    {
        #region Update Doctor Speciality Info

        var res = await _shopProductService.UpdateProductSelectedCategrory_AdminSide(Permissions, productId, cancellation);
        if (res)
        {
            TempData[SuccessMessage] = "دسته بندی های انتخابی باموفقیت ثبت گردید.";

            return RedirectToAction(nameof(FilterShopProducts));
        }

        #endregion

        ViewData["ProductId"] = productId;

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(await _shopProductService.FillListOfSellerProductCategoriesDTO(User.GetUserId(), productId, cancellation));
    }

    #endregion

    #region Load Sub Categories

    public async Task<IActionResult> LoadSubCategories(ulong MainCategoryId, CancellationToken cancellationToken = default)
    {
        var result = await _shopCategoryService.GetCategoriesChildrent(MainCategoryId, cancellationToken);

        return JsonResponseStatus.Success(result);
    }

    #endregion

    #region Gallery

    #region Product Gallery 

    [HttpGet]
    public async Task<IActionResult> ProductGallery(ListOfProductGalleryQuery query, CancellationToken token = default)
    {
        #region Get Products Gallery 

        ViewData["ListOfProductGalleries"] = await Mediator.Send(query, token);

        #endregion

        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductGallery(CreateShopProductGalleryDTO model,
                                                    IFormFile NewsImage,
                                                    CancellationToken cancellation = default)
    {
        #region Map DTOs

        CreateProductGalleryCommand command = new CreateProductGalleryCommand()
        {
            image = NewsImage,
            productId = model.ProductId,
        };

        ListOfProductGalleryQuery query = new ListOfProductGalleryQuery()
        {
            productId = model.ProductId
        };

        #endregion

        #region Add Gallery

        var res = await Mediator.Send(command, cancellation);
        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ProductGallery), new { productId = model.ProductId });
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        ViewData["ListOfProductGalleries"] = Mediator.Send(query, cancellation);

        return View(model);
    }

    #endregion

    #region Delete Product Gallery

    [HttpGet]
    public async Task<IActionResult> DeleteProductGallery(DeleteProductGalleryCommand_AdminSide command, 
                                                          CancellationToken token = default)
    {
        var res = await Mediator.Send(command, token);
        if (res.Result == true)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ProductGallery), new { area = "Admin", productId = res.ProductId });
        }

        return NotFound();
    }

    #endregion

    #endregion

    #region Product Feature

    #region Product Feature 

    [HttpGet]
    public async Task<IActionResult> ProductFeature(ListOfProductFeatureQuery query,
                                                    CancellationToken token = default)
    {
        #region Get Products Feature 

        ViewData["ListOfProductFeatures"] = await Mediator.Send(query, token);

        #endregion

        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductFeature(ProductFeaturesDTO model,
                                                    CancellationToken cancellation = default)
    {
        #region Map DTOs

        CreateShopProductFeatureQuery_AdminSide command = new CreateShopProductFeatureQuery_AdminSide()
        {
            FeatureTitle = model.Title,
            FeatureValue = model.Value,
            productId = model.ProductId,
        };

        ListOfProductFeatureQuery query = new ListOfProductFeatureQuery()
        {
            productId = model.ProductId
        };

        #endregion

        #region Add Gallery

        var res = await Mediator.Send(command, cancellation);
        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ProductFeature), new { productId = model.ProductId });
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        ViewData["ListOfProductGalleries"] = Mediator.Send(query, cancellation);

        return View(model);
    }

    #endregion

    #region Delete Product Feature

    [HttpGet]
    public async Task<IActionResult> DeleteProductFeature(DeleteShopProductFeatureCommand_AdminSide command,
                                                          CancellationToken token = default)
    {
        var res = await Mediator.Send(command, token);
        if (res)
        {
            return JsonResponseStatus.Success();
        }

        return JsonResponseStatus.Error();
    }

    #endregion

    #endregion

    #region Product Brand

    [HttpGet]
    public async Task<IActionResult> ProductBrand(ulong productId,
                                                  CancellationToken cancellationToken)
    {
        ViewData["SelectedBrandId"] = await _brandService.GetShopProductSelectedBrandByProductId(productId, cancellationToken);
        ViewData["ProductId"] = productId;

        return View(await _brandService.FillShopProductsBrandDTO(cancellationToken));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductBrand(ulong productId,
                                                  List<ulong>? Permissions,
                                                  CancellationToken cancellationToken)
    {
        if (Permissions == null || !Permissions.Any())
        {
            TempData[ErrorMessage] = "انتخاب برند اجباری است.";
            return RedirectToAction(nameof(FilterShopProducts));
        }

        #region Add Brand To The Product

        ProductBrandsCommand_AdminSide command = new ProductBrandsCommand_AdminSide()
        {
            BrandId = Permissions.First(),
            ProductId = productId,
        };

        #endregion

        #region Update Product

        var res = await Mediator.Send(command, cancellationToken);
        if (res)
        {
            TempData[SuccessMessage] = "برند انتخابی باموفقیت ثبت گردید.";
            return RedirectToAction(nameof(FilterShopProducts));
        }

        #endregion

        ViewData["SelectedBrandId"] = await _brandService.GetShopProductSelectedBrandByProductId(productId, cancellationToken);

        return View(await _brandService.FillShopProductsBrandDTO(cancellationToken));
    }
    #endregion
}
