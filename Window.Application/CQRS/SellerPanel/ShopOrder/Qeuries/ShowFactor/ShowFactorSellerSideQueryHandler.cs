using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.ShowFactor;

public record ShowFactorSellerSideQueryHandler : IRequestHandler<ShowFactorSellerSideQuery, ShowOrderFactorDTO>
{
    #region Ctor 

    private readonly IOrderQueryRepository _orderQueryRepository;

    public ShowFactorSellerSideQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    #endregion

    public async Task<ShowOrderFactorDTO?> Handle(ShowFactorSellerSideQuery request, CancellationToken cancellationToken)
    {
        return await _orderQueryRepository.ShowOrderFactorDTO_ByOrderId(request.OrderId , cancellationToken);
    }
}
