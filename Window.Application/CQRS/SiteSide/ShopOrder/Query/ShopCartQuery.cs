using Window.Domain.ViewModels.Site.Shop.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public class ShopCartQuery : IRequest<ShopCartOrderDTO>
{
    public ulong UserId { get; set; }
}
