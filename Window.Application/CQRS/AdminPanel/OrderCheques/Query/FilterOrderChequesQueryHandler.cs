using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Query;

public record FilterOrderChequesQueryHandler : IRequestHandler<FilterOrderChequesQuery, FilterOrderChequesDTO>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public FilterOrderChequesQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<FilterOrderChequesDTO> Handle(FilterOrderChequesQuery request, CancellationToken cancellationToken)
    {
        return await _orderChequeQueryRepository.FillFilterOrderChequesDTO(request.FilterOrderChequesDTO , cancellationToken);
    }
}
