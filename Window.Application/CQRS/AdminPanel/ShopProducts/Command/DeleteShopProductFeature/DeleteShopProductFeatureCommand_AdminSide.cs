namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.DeleteShopProductFeature;

public record DeleteShopProductFeatureCommand_AdminSide : IRequest<bool>
{
    public ulong FeatureId { get; set; }
}
