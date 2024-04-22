using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.FilterShopOrders;

public record FilterShopOrdersQueryHandler : IRequestHandler<FilterShopOrdersQuery, FilterShopOrdersSellerSideDTO>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;

    public FilterShopOrdersQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    #endregion

    public async Task<FilterShopOrdersSellerSideDTO> Handle(FilterShopOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderQueryRepository.Fill_FilterShopOrdersSeller(request.Filter , cancellationToken);
    }
}
