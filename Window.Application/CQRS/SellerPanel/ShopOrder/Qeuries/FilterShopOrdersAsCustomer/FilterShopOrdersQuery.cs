using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.FilterShopOrdersAsCustomer;

public record FilterShopOrdersAsCustomerQuery : IRequest<FilterShopOrdersSellerAsCustomerSideDTO>
{
    #region proeprties

    public FilterShopOrdersSellerAsCustomerSideDTO Filter { get; set; }

    #endregion
}
