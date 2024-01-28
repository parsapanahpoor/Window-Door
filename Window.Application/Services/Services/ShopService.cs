#region Using

using Microsoft.AspNetCore.Http;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Seller.Product;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Services;

#endregion

public class ShopProductService : IShopProductService
{
    #region Ctor

    private readonly IShopProductCommandRepository _shopProductCommandRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IShopColorsCommandRepository _shopColorsCommand;
    private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
    private readonly IShopCategoryCommandRepository _shopCategoryCommand;
    private readonly IShopCategoryQueryRepository _shopCategoryQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISellerService _marketService;

    public ShopProductService(IShopProductCommandRepository shopProductCommandRepository,
                              IShopProductQueryRepository shopProductQueryRepository,
                              IUnitOfWork unitOfWork,
                              ISellerService marketService,
                              IShopColorsCommandRepository shopColorsCommand,
                              IShopCategoryCommandRepository shopCategoryCommand,
                              IShopColorsQueryRepository shopColorsQueryRepository,
                              IShopCategoryQueryRepository shopCategoryQueryRepository)
    {
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
        _marketService = marketService;
        _shopColorsCommand = shopColorsCommand;
        _shopCategoryCommand = shopCategoryCommand;
        _shopColorsQueryRepository = shopColorsQueryRepository;
        _shopCategoryQueryRepository = shopCategoryQueryRepository;

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
            LongDescription = model.Description,
            ShortDescription = model.ShortDescription,
            Price = model.Price
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

    public async Task<EditShopProductSellerSideDTO?> FillEditShopProductSellerSideDTO(ulong productId, ulong sellerId, CancellationToken token)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return null;

        #endregion

        #region Get Product By Id 

        var product = await _shopProductQueryRepository.GetByIdAsync(token, productId);
        if (product == null) return null;
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
            Description = product.LongDescription,
            Price = product.Price,
            ShortDescription = product.ShortDescription,
        };

        #endregion

        #region Product Tags

        var tags = await _shopProductQueryRepository.GetListOfProductTagsByProductId(product.Id, token);
        model.ProductTag = string.Join(",", tags.Select(p => p.TagTitle).ToList());

        #endregion

        return model;
    }

    public async Task<EditShopProductFromSellerPanelResult> EditShopProductSellerSide(EditShopProductSellerSideDTO newProduct, ulong sellerId, IFormFile? newsImage, CancellationToken cancellation)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return EditShopProductFromSellerPanelResult.SellerIsNotFound;

        #endregion

        #region Get Product By Id 

        var oldProduct = await _shopProductQueryRepository.GetByIdAsync(cancellation, newProduct.ShopProductId);
        if (oldProduct == null) return EditShopProductFromSellerPanelResult.Faild;
        if (oldProduct.SellerUserId != sellerId) return EditShopProductFromSellerPanelResult.SellerIsNotFound;

        #endregion

        #region Brand and Color Validator

        if (!await _shopColorsQueryRepository.IsExistColorById(newProduct.ShopColorId, cancellation))
            return EditShopProductFromSellerPanelResult.Faild;

        #endregion

        #region Update Product Fields

        oldProduct.ProductName = newProduct.Title.SanitizeText();
        oldProduct.ShortDescription = newProduct.ShortDescription.SanitizeText();
        oldProduct.LongDescription = newProduct.Description.SanitizeText();
        oldProduct.Price = newProduct.Price;
        oldProduct.ProductBrandId = newProduct.ShopBrandId;
        oldProduct.ProductColorId = newProduct.ShopColorId;

        #endregion

        #region  Image

        if (newsImage != null)
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(newsImage.FileName);
            newsImage.AddImageToServer(imageName, FilePaths.ProductsPathServer, 400, 300, FilePaths.ProductsPathThumbServer);

            if (!string.IsNullOrEmpty(oldProduct.ProductImage))
            {
                oldProduct.ProductImage.DeleteImage(FilePaths.ProductsPathServer, FilePaths.ProductsPathThumbServer);
            }

            oldProduct.ProductImage = imageName;
        }

        #endregion

        _shopProductCommandRepository.Update(oldProduct);

        #region Shop Tags

        var productTags = await _shopProductQueryRepository.GetListOfProductTagsByProductId(oldProduct.Id, cancellation);
        if (productTags != null && productTags.Any()) _shopProductCommandRepository.DeleteRange(productTags);

        if (!string.IsNullOrEmpty(newProduct.ProductTag))
        {
            List<string> tagsList = newProduct.ProductTag.Split(',').ToList<string>();
            foreach (var itemTag in tagsList)
            {
                var newTag = new ProductTag
                {
                    ProductId = oldProduct.Id,
                    TagTitle = itemTag,
                    IsDelete = false,
                    CreateDate = DateTime.Now
                };
                await _shopProductCommandRepository.AddShopTagAsync(newTag, cancellation);
            }
        }

        #endregion

        await _unitOfWork.SaveChangesAsync();

        return EditShopProductFromSellerPanelResult.Success;
    }

    public async Task<List<ulong>> GetShopProductSelectedCategories(ulong productId, CancellationToken token)
    {
        return await _shopProductQueryRepository.GetShopProductSelectedCategories(productId, token);
    }

    public async Task<bool> DeleteArticleAdminSide(ulong productId, ulong sellerId, CancellationToken cancellation)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return false;

        #endregion

        #region Get Product By Id 

        var oldProduct = await _shopProductQueryRepository.GetByIdAsync(cancellation, productId);
        if (oldProduct == null) return false;
        if (oldProduct.SellerUserId != sellerId) return false;

        #endregion

        #region Delete Product File 

        if (!string.IsNullOrEmpty(oldProduct.ProductImage))
        {
            oldProduct.ProductImage.DeleteImage(FilePaths.ProductsPathServer, FilePaths.ProductsPathThumbServer);
        }

        #endregion

        #region Update Article Field 

        oldProduct.IsDelete = true;

        #endregion

        #region Shop Product Tags 

        var productTags = await _shopProductQueryRepository.GetListOfProductTagsByProductId(oldProduct.Id, cancellation);
        if (productTags != null && productTags.Any()) _shopProductCommandRepository.DeleteRange(productTags);

        #endregion

        _shopProductCommandRepository.Update(oldProduct);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<List<ListOfSellerProductCategoriesDTO>?> FillListOfSellerProductCategoriesDTO(ulong sellerId,
                                                                                                    ulong productId,
                                                                                                    CancellationToken cancellationToken)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return null;

        #endregion

        #region Get Product By Id 

        var oldProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, productId);
        if (oldProduct == null) return null;
        if (oldProduct.SellerUserId != sellerId) return null;

        #endregion

        #region Get Product Selected Categories

        var productSelectedCategoryId = await _shopProductQueryRepository.GetShopProductSelectedCategories(productId, cancellationToken);

        #endregion

        #region Get List Of Categories

        var categories = await _shopCategoryQueryRepository.GetListOfShopCategories(cancellationToken);
        if (categories == null) return null;

        #endregion

        #region Fill Model 

        var ListOfSellerProductCategories = new List<ListOfSellerProductCategoriesDTO>();

        foreach (var category in categories)
        {
            var reutnItems = new ListOfSellerProductCategoriesDTO();

            reutnItems.ShopCategory = category;

            if (productSelectedCategoryId != null && productSelectedCategoryId.Any() && productSelectedCategoryId.Contains(category.Id))
            {
                reutnItems.IsSelectedBySellerProduct = true;
            }
            else
            {
                reutnItems.IsSelectedBySellerProduct = false;
            }

            ListOfSellerProductCategories.Add(reutnItems);
        }

        #endregion

        return ListOfSellerProductCategories;
    }

    //Update Product Selected Categrory
    public async Task<bool> UpdateDoctorSpecialitySelected(List<ulong>? categoryIds,
                                                           ulong sellerId,
                                                           ulong productId,
                                                           CancellationToken cancellation)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return false;

        #endregion

        #region Get Product By Id 

        var oldProduct = await _shopProductQueryRepository.GetByIdAsync(cancellation, productId);
        if (oldProduct == null) return false;
        if (oldProduct.SellerUserId != sellerId) return false;

        #endregion

        #region Update Product Selected Categories

        //remove all Product Selected Categories
        var productSelectedCategories = await _shopProductQueryRepository.GetListOf_ShopProductSelectedCategories_ByProductId(productId, cancellation);
        if (productSelectedCategories != null && productSelectedCategories.Any())
            _shopProductCommandRepository.DeleteRangeProductSelectedCategories(productSelectedCategories);

        //add Categories To The Seller Product
        if (categoryIds != null && categoryIds.Any())
        {
            foreach (var categoryId in categoryIds)
            {
                var shopProductSelectedCategories = new ShopProductSelectedCategories
                {
                    ShopProductId = productId,
                    ShopCategoryId = categoryId,
                };

                await _shopCategoryCommand.AddShopProductSelectedCategoriesAsync(shopProductSelectedCategories, cancellation);
            }
        }

        #endregion

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}
