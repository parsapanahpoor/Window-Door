namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.CreateShopProductFeature;

public record CreateShopProductFeatureQuery_AdminSide : IRequest<bool>
{
    public ulong productId { get; set; }

    public string FeatureTitle { get; set; }

    public string FeatureValue { get; set; }
}
