namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record DeleteOrderCommand : IRequest<bool>
{
    public ulong UserId { get; set; }
}
