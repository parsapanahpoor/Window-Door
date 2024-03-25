using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Query;

public record FilterReciveOrderChequesSellerSideQueryHandler : IRequestHandler<FilterReciveOrderChequesSellerSideQuery , FilterReciveOrderChequesSellerSideDTO>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public FilterReciveOrderChequesSellerSideQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<FilterReciveOrderChequesSellerSideDTO> Handle(FilterReciveOrderChequesSellerSideQuery request, CancellationToken cancellationToken)
    {
        return await _orderChequeQueryRepository.FillFilterReciveOrderChequesDTO(request.FilterOrderChequesDTO, cancellationToken);
    }
}
