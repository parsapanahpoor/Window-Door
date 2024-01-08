using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        await _unitOfWork.SaveChangesAsync();

        return CreateShopProductFromSellerPanelResult.Success;
    }

    public async Task<EditShopProductSellerSideDTO?> FillEditShopProductSellerSideDTO(ulong productId , ulong sellerId , CancellationToken token)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return null;

        #endregion

        #region Get Product By Id 

        var product = await _shopProductQueryRepository.GetByIdAsync(token , productId);
        if(product == null) return null;
        if (product.SellerUserId != sellerId) return null;

        #endregion

        #region Fill DTO 

        EditShopProductSellerSideDTO model = new EditShopProductSellerSideDTO()
        {
            ShopProductId = product.Id,
            ShopBrandId = product.ProductBrandId,
            ShopColorId = product.ProductColorId,
            Title = product.ProductName,
            ProductImage = product.ProductImage,
        };

        #endregion

        return model;
    }

    public async Task<List<ulong>> GetShopProductSelectedCategories(ulong productId, CancellationToken token)
    {
        return await _shopProductQueryRepository.GetShopProductSelectedCategories(productId, token);
    }

    #endregion
}
