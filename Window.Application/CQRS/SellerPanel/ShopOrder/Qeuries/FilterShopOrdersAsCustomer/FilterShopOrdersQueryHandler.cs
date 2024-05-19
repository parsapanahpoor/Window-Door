using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.FilterShopOrdersAsCustomer;

public record FilterShopOrdersAsCustomerQueryHandler : IRequestHandler<FilterShopOrdersAsCustomerQuery, FilterShopOrdersSellerAsCustomerSideDTO>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;

    public FilterShopOrdersAsCustomerQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    #endregion

    public async Task<FilterShopOrdersSellerAsCustomerSideDTO> Handle(FilterShopOrdersAsCustomerQuery request, CancellationToken cancellationToken)
    {
        return await _orderQueryRepository.Fill_FilterShopOrdersSellerAsCustomer(request.Filter , cancellationToken);
    }
}
