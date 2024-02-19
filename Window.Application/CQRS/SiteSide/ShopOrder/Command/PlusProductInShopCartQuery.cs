namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public class PlusProductInShopCartQuery : IRequest<bool>
{
    public ulong productId { get; set; }

    public ulong UserId { get; set; }
}
