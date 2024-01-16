using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Window.Domain.Interfaces.ShopProductGallery;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductGallery;

public record ListOfProductGalleryQueryHandler : IRequestHandler<ListOfProductGalleryQuery, List<ProductGalleriesDTO>>
{
    #region Ctor

    private readonly IShopProductGalleryQueryRepository _shopProductGalleryQueryRepository;

    public ListOfProductGalleryQueryHandler(IShopProductGalleryQueryRepository shopProductGalleryQueryRepository)
    {
            _shopProductGalleryQueryRepository = shopProductGalleryQueryRepository;
    }

    #endregion

    public async Task<List<ProductGalleriesDTO>?> Handle(ListOfProductGalleryQuery request,
                                                                      CancellationToken cancellationToken)
    {
        return await _shopProductGalleryQueryRepository.FillProductGalleriesDTO(request.productId, cancellationToken);
    }
}
