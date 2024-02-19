namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public class DeleteProductFromShopCartCommand : IRequest<bool>
{
    #region properties

    public ulong ProductId { get; set; }

    public ulong UserId { get; set; }

    #endregion
}
