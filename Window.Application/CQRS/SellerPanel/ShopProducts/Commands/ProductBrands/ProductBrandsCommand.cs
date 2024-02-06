namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.ProductBrands;

public record ProductBrandsCommand : IRequest<bool>
{
    #region properties

    public ulong ProductId { get; set; }

    public ulong UserId { get; set; }

    public ulong BrandId { get; set; }

    #endregion
}
