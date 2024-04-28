namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.ProductBrandsCommand;

public record ProductBrandsCommand_AdminSide : IRequest<bool>
{
    #region properties

    public ulong ProductId { get; set; }

    public ulong BrandId { get; set; }

    #endregion
}
