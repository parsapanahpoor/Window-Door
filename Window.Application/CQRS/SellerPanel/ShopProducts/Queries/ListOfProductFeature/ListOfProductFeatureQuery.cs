using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductFeature;

public class ListOfProductFeatureQuery : IRequest<List<ProductFeaturesDTO>>
{
    public ulong productId { get; set; }
}

