using Microsoft.EntityFrameworkCore.Infrastructure;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductFeature;
using Window.Domain.Interfaces.ShopProductGallery;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopProduct.Query;

public record ShopProductDetailQueryHandler : IRequestHandler<ShopProductDetailQuery, ShopProductDetailDTO>
{
    #region Ctor

    private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IShopProductGalleryQueryRepository _shopProductGalleryQueryRepository;
    private readonly IBrandService _brandService;
    private readonly IShopProductFeatureQueryRepository _shopProductFeatureQueryRepository;
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUserRepository _userRepository;

    public ShopProductDetailQueryHandler(IShopColorsQueryRepository shopColorsQueryRepository , 
                                         IShopProductQueryRepository shopProductQueryRepository , 
                                         IBrandService brandService ,
                                         IShopProductGalleryQueryRepository shopProductGalleryQueryRepository , 
                                         IShopProductFeatureQueryRepository shopProductFeatureQueryRepository,
                                         IUserRepository userRepository ,
                                         ISiteSettingService siteSettingService)
    {
        _shopColorsQueryRepository = shopColorsQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _brandService = brandService;
        _shopProductGalleryQueryRepository = shopProductGalleryQueryRepository;
        _shopProductFeatureQueryRepository = shopProductFeatureQueryRepository;
        _userRepository = userRepository;
        _siteSettingService = siteSettingService;
    }

    public async Task<ShopProductDetailDTO?> Handle(ShopProductDetailQuery request, CancellationToken cancellationToken)
    {
        #region Get Shop Product

        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken , request.productId);
        if (product == null) return null;

        #endregion

        #region Fill Return Model 

        ShopProductDetailDTO model = new()
        {
            Brand = await _brandService.FillShopProductDetailBrand(product.ProductBrandId.Value, cancellationToken),
            SaleScale = product.SaleScaleId != null ? await _siteSettingService.Get_SaleScaleTitle_ById(product.SaleScaleId , cancellationToken) : null ,
            Color = await _shopColorsQueryRepository.FillShopProductDetailColor(product.ProductColorId, cancellationToken),
            LongDescription = product.LongDescription,
            Price = product.Price,
            ProductId = product.Id,
            ProductImage = product.ProductImage,
            ProductName = product.ProductName,
            ShortDescription = product.ShortDescription,
            Seller = await _userRepository.FillSeller(product.SellerUserId , cancellationToken),
            ShopProductFeatures = await _shopProductFeatureQueryRepository.GetListOfProductFeaturesByProductId(product.Id , cancellationToken),
            ProductInventory = product.ProductInventory,
            ShowProductInventory = product.ShowProductInventory,
        };

        var productImages = new List<string>();
        productImages.Add(product.ProductImage);
        productImages.AddRange(await _shopProductGalleryQueryRepository.GetProductGalleriesImages(request.productId, cancellationToken));

        model.ProductPirctures = productImages;

        #endregion

        return model;
    }

    #endregion
}
