using Microsoft.AspNetCore.Http;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopShopProductFeature;

public class CreateShopProductFeatureQuery : IRequest<bool>
{
    public ulong sellerUserId { get; set; }

    public ulong productId { get; set; }

    public string FeatureTitle { get; set; }

    public string FeatureValue { get; set; }
}
