using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductFeature;

public class ListOfProductFeatureQuery : IRequest<List<ProductGalleriesDTO>>
{
    public ulong productId { get; set; }
}

