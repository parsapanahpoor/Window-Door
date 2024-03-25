using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Query;

public record FilterDepositOrderChequesSellerSideQueryHandler : IRequestHandler<FilterDepositOrderChequesSellerSideQuery, FilterReciveOrderChequesSellerSideDTO>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public FilterDepositOrderChequesSellerSideQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<FilterReciveOrderChequesSellerSideDTO> Handle(FilterDepositOrderChequesSellerSideQuery request, CancellationToken cancellationToken)
    {
        return await _orderChequeQueryRepository.FillFilterDepositOrderChequesDTO(request.FilterOrderChequesDTO, cancellationToken);
    }
}
