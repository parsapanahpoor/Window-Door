using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.ShopBrands;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopColors;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Interfaces.ShopBrands;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Services;

public class ShopProductService : IShopProductService
{
    #region Ctor

    private readonly IShopProductCommandRepository _shopProductCommandRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IShopBrandsCommandRepository _shopBrandsCommand;
    private readonly IShopColorsCommandRepository _shopColorsCommand;
    private readonly IShopCategoryCommandRepository _shopCategoryCommand;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISellerService _marketService;

    public ShopProductService(IShopProductCommandRepository shopProductCommandRepository,
                              IShopProductQueryRepository shopProductQueryRepository,
                              IUnitOfWork unitOfWork,
                              ISellerService marketService,
                              IShopBrandsCommandRepository shopBrandsCommand,
                              IShopColorsCommandRepository shopColorsCommand,
                              IShopCategoryCommandRepository shopCategoryCommand)
    {
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
        _marketService = marketService;
        _shopBrandsCommand = shopBrandsCommand;
        _shopColorsCommand = shopColorsCommand;
        _shopCategoryCommand = shopCategoryCommand;
    }

    #endregion

    #region Seller Side 

    public async Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation)
    {
        return await _shopProductQueryRepository.FilterShopProductSellerSide(filter, cancellation);
    }

    public async Task<CreateShopProductFromSellerPanelResult> AddShopProductToTheDataBase(ulong sellerId,
                                                                                  CreateShopProductSellerSideDTO model,
                                                                                  IFormFile newsImage,
                                                                                  CancellationToken cancellation)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return CreateShopProductFromSellerPanelResult.SellerIsNotFound;

        #endregion

        #region Add Shop Product Entity

        ShopProduct product = new ShopProduct()
        {
            CreateDate = DateTime.Now,
            IsDelete = false,
            ProductBrandId = model.ShopBrandId,
            ProductColorId = model.ShopColorId,
            ProductName = model.Title,
            SellerUserId = sellerId,
        };

        if (newsImage != null && newsImage.IsImage())
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(newsImage.FileName);
            newsImage.AddImageToServer(imageName, FilePaths.ProductsPathServer, 400, 300, FilePaths.ProductsPathThumbServer);
            product.ProductImage = imageName;
        }

        #endregion

        #region Add Shop Product 

        await _shopProductCommandRepository.AddAsync(product, cancellation);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        #region Shop Tags

        if (!string.IsNullOrEmpty(model.ProductTag))
        {
            List<string> tagsList = model.ProductTag.Split(',').ToList<string>();
            foreach (var itemTag in tagsList)
            {
                var newTag = new ProductTag
                {
                    ProductId = product.Id,
                    TagTitle = itemTag,
                    IsDelete = false,
                    CreateDate = DateTime.Now
                };
                await _shopProductCommandRepository.AddShopTagAsync(newTag, cancellation);
            }
        }

        #endregion

        #region Products Selected Brands

        if (model.ShopBrandId != 0)
        {
            var selectedBrand = new ShopProductsSelectedBrands()
            {
                ShopBrandId = model.ShopBrandId,
                ShopProductId = product.Id,
            };

            await _shopBrandsCommand.AddShopProductSelectedBrandAsync(selectedBrand , cancellation);
        };

        #endregion

        #region Products Selected Colors

        if (model.ShopColorId != 0)
        {
            var selectedColor = new ShopProductsSelectedColors()
            {
                ColorId = model.ShopColorId,
                ShopProductId = product.Id,
            };

            await _shopColorsCommand.AddShopProductSelectedColorAsync(selectedColor, cancellation);
        };

        #endregion

        #region Selected Shop Categories

        //First Step
        if (model.MainCategory != 0)
        {
            var selectedMainCategory = new ShopProductSelectedCategories()
            {
                ShopCategoryId = model.MainCategory,
                ShopProductId= product.Id,
            };

            await _shopCategoryCommand.AddShopProductSelectedCategoriesAsync(selectedMainCategory , cancellation) ;
        }

        //Seconde Step
        if (model.FirstSubCategory.HasValue && model.FirstSubCategory != 0)
        {
            var selectedSecondeCategory = new ShopProductSelectedCategories()
            {
                ShopCategoryId = model.FirstSubCategory.Value,
                ShopProductId = product.Id,
            };

            await _shopCategoryCommand.AddShopProductSelectedCategoriesAsync(selectedSecondeCategory, cancellation);
        }
        else
        {
            return CreateShopProductFromSellerPanelResult.MainCategoryNotFound;
        }

        //Third Step
        if (model.SecondeSubCategory.HasValue && model.SecondeSubCategory != 0)
        {
            var selectedThirdCategory = new ShopProductSelectedCategories()
            {
                ShopCategoryId = model.SecondeSubCategory.Value,
                ShopProductId = product.Id,
            };

            await _shopCategoryCommand.AddShopProductSelectedCategoriesAsync(selectedThirdCategory, cancellation);
        }
        else
        {
            return CreateShopProductFromSellerPanelResult.MainCategoryNotFound;
        }

        #endregion

        await _unitOfWork.SaveChangesAsync();

        return CreateShopProductFromSellerPanelResult.Success;
    }

    #endregion
}
