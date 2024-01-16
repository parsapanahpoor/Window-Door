using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductGallery;

public class ListOfProductGalleryQuery : IRequest<List<ProductGalleriesDTO>>
{
    public ulong productId { get; set; }
}

