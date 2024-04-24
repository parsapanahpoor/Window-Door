using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductGallery;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductGallery;

public record ListOfProductGalleryQueryHandler : IRequestHandler<ListOfProductGalleryQuery, List<ProductGalleriesDTO>>
{
    #region Ctor

    private readonly IShopProductGalleryQueryRepository _shopProductGalleryQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public ListOfProductGalleryQueryHandler(IShopProductGalleryQueryRepository shopProductGalleryQueryRepository,
                                            IShopProductQueryRepository shopProductQueryRepository)
    {
        _shopProductGalleryQueryRepository = shopProductGalleryQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<List<ProductGalleriesDTO>?> Handle(ListOfProductGalleryQuery request,
                                                                      CancellationToken cancellationToken)
    {
        //Get Origin Product
        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken , request.productId);
        if (product == null) return null;

        //Get List Of Shop Galleries
        var gallery = await _shopProductGalleryQueryRepository.FillProductGalleriesDTO(request.productId, cancellationToken);

        if (gallery != null && gallery.Any(p=> p.ImageName == product.ProductImage))
        {
            var mainImage = gallery.Where(p => p.ImageName == product.ProductImage).FirstOrDefault();
            mainImage.MainImage = true;
        }

        return gallery;
    }
}
