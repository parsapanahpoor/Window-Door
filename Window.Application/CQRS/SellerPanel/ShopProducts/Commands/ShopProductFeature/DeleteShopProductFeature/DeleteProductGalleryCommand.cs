namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteShopProductFeature;

public class DeleteShopProductFeatureCommand : IRequest<bool>
{
    public ulong FeatureId { get; set; }

    public ulong SellerUserId { get; set; }
}
