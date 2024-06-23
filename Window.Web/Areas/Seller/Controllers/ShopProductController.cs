#region Using

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopProductGallery;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopShopProductFeature;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteProductGallery;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteShopProductFeature;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.ProductBrands;
using Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductFeature;
using Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductGallery;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Web.Areas.Seller.ActionFilterAttributes;
using Window.Web.HttpManager;
namespace Window.Web.Areas.Seller.Controllers;

#endregion

[CheckHasSellerAnyChequeInfo]
public class ShopProductController : SellerBaseController
{
    #region Ctor

    private readonly IShopProductService _shopProductService;
    private readonly IShopCategoryService _shopCategoryService;
    private readonly IShopColorService _shopColorService;
    private readonly IBrandService _brandService;
    private readonly ISiteSettingService _siteSettingService;

    public ShopProductController(IShopProductService shopProductService,
                                 IShopCategoryService shopCategoryService,
                                 IShopColorService shopColorService,
                                 IBrandService brandService,
                                 ISiteSettingService siteSettingService)
    {
        _shopProductService = shopProductService;
        _shopCategoryService = shopCategoryService;
        _shopColorService = shopColorService;
        _brandService = brandService;
        _siteSettingService = siteSettingService;
    }

    #endregion

    #region Filter Products

    public async Task<IActionResult> FilterShopProducts(FilterShopProductSellerSideDTO filter, CancellationToken cancellation = default)
    {
        filter.SellerUserId = User.GetUserId();

        return View(await _shopProductService.FilterShopProductSellerSide(filter, cancellation));
    }

    #endregion

    #region Create Product

    [HttpGet]
    public async Task<IActionResult> CreateProduct(CancellationToken cancellation = default)
    {
        #region View Bags

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        ViewData["SaleScales"] = await _siteSettingService.ListOfSalesScales();

        #endregion

        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(CreateShopProductSellerSideDTO model,
                                                   CancellationToken cancellation = default)
    {
        #region Model State Validation 

        if (!ModelState.IsValid)
        {
            #region View Bags

            ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

            ViewData["SaleScales"] = await _siteSettingService.ListOfSalesScales();

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion

        #region Add Product To The Data Base

        var res = await _shopProductService.AddShopProductToTheDataBase(User.GetUserId(), model, cancellation);
        switch (res)
        {
            case CreateShopProductFromSellerPanelResult.Success:
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(FilterShopProducts));

            case CreateShopProductFromSellerPanelResult.Faild:
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                break;

            case CreateShopProductFromSellerPanelResult.MainCategoryNotFound:
                TempData[ErrorMessage] = "دسته بندی های محصول انتخاب نشده است";
                break;

            case CreateShopProductFromSellerPanelResult.SellerIsNotFound:
                TempData[ErrorMessage] = "فروشنده ی مورد نظر یافت نشده است.";
                break;

            default:
                break;
        }

        #endregion

        #region View Bags

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        ViewData["SaleScales"] = await _siteSettingService.ListOfSalesScales();

        #endregion

        return View(model);
    }

    #endregion

    #region Edit Product

    [HttpGet]
    public async Task<IActionResult> EditProduct(ulong productId, CancellationToken cancellation)
    {
        var product = await _shopProductService.FillEditShopProductSellerSideDTO(productId, User.GetUserId(), cancellation);
        if (product == null) return NotFound();

        #region View Bags

        ViewData["SaleScales"] = await _siteSettingService.ListOfSalesScales();

        ViewData["Colors"] = await _shopColorService.FillListOfColorsForFilterProductsDTO(cancellation);

        #endregion

        return View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(EditShopProductSellerSideDTO model, CancellationToken cancellation)
    {
        #region Edit Product

        if (ModelState.IsValid)
        {
            var res = await _shopProductService.EditShopProductSellerSide(model, User.GetUserId(), cancellation);
            switch (res)
            {
                case EditShopProductFromSellerPanelResult.Success:
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                    return RedirectToAction(nameof(FilterShopProducts));

                case EditShopProductFromSellerPanelResult.Faild:
                    TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                    break;

                case EditShopProductFromSellerPanelResult.MainCategoryNotFound:
                    TempData[ErrorMessage] = "دسته بندی های محصول انتخاب نشده است";
                    break;

                case EditShopProductFromSellerPanelResult.SellerIsNotFound:
                    TempData[ErrorMessage] = "فروشنده ی مورد نظر یافت نشده است.";
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
        var result = await _shopProductService.DeleteArticleAdminSide(shopProductId, User.GetUserId(), cancellation);
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

        return View(await _shopProductService.FillListOf_AdminProductCategoriesDTO( productId, cancellation));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProductCategory(ulong productId, List<ulong>? Permissions, CancellationToken cancellation)
    {
        #region Update Doctor Speciality Info

        var res = await _shopProductService.UpdateDoctorSpecialitySelected(Permissions, User.GetUserId(), productId, cancellation);
        if (res)
        {
            TempData[SuccessMessage] = "دسته بندی های انتخابی باموفقیت ثبت گردید.";

            return RedirectToAction(nameof(FilterShopProducts));
        }

        #endregion

        ViewData["ProductId"] = productId;

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(await _shopProductService.FillListOf_AdminProductCategoriesDTO(productId, cancellation));
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

        CreateProductGalleryQuery command = new CreateProductGalleryQuery()
        {
            image = NewsImage,
            productId = model.ProductId,
            sellerUserId = User.GetUserId(),
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
    public async Task<IActionResult> DeleteProductGallery(DeleteProductGalleryCommand command, CancellationToken token = default)
    {
        command.userSellerId = User.GetUserId();
        var res = await Mediator.Send(command, token);
        if (res.Result == true)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ProductGallery), new { area = "Seller", productId = res.ProductId });
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

        CreateShopProductFeatureQuery command = new CreateShopProductFeatureQuery()
        {
            FeatureTitle = model.Title,
            FeatureValue = model.Value,
            productId = model.ProductId,
            sellerUserId = User.GetUserId(),
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
    public async Task<IActionResult> DeleteProductFeature(DeleteShopProductFeatureCommand command, CancellationToken token = default)
    {
        command.SellerUserId = User.GetUserId();
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
                                                  string? brandTitle ,
                                                  CancellationToken cancellationToken)
    {
        ViewData["SelectedBrandId"] = await _brandService.GetShopProductSelectedBrandByProductId(productId, cancellationToken);
        ViewData["ProductId"] = productId;
        ViewData["brandTitle"] = brandTitle;

        var model = string.IsNullOrEmpty(brandTitle) ?
                    await _brandService.FillShopProductsBrandDTO(cancellationToken) :
                    await _brandService.FillShopProductsBrandDTO(brandTitle , cancellationToken);

        return View(model);
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

        ProductBrandsCommand command = new ProductBrandsCommand()
        {
            BrandId = Permissions.First(),
            ProductId = productId,
            UserId = User.GetUserId()
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
