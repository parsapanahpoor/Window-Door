using Window.Domain.ViewModels.Site.Shop.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public record ShowInvoiceQuery : IRequest<InvoiceDTO>
{
    #region properties

    public ulong UserId { get; set; }

    #endregion
}
