using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Query;

public record ShowChequeDetailForAdminReviewQueryHandler : IRequestHandler<ShowChequeDetailForAdminReviewQuery, ChequeDetailDTO>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public ShowChequeDetailForAdminReviewQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<ChequeDetailDTO?> Handle(ShowChequeDetailForAdminReviewQuery request, CancellationToken cancellationToken)
    {
        return await _orderChequeQueryRepository.Fill_ChequeDetailDTO_ByOrderChequeId(request.ChequeId , cancellationToken);
    }
}
