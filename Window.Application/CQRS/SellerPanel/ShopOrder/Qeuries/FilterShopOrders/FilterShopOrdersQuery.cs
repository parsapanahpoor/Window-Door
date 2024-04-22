using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.FilterShopOrders;

public record FilterShopOrdersQuery : IRequest<FilterShopOrdersSellerSideDTO>
{
    #region proeprties

    public FilterShopOrdersSellerSideDTO Filter { get; set; }

    #endregion
}
