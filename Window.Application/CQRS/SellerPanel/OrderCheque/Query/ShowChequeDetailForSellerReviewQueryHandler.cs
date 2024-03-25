using Window.Application.CQRS.AdminPanel.OrderCheques.Query;
using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Query;

public record ShowChequeDetailForSellerReviewQueryHandler : IRequestHandler<ShowChequeDetailForSellerReviewQuery, ChequeDetailSellerSideDTO>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public ShowChequeDetailForSellerReviewQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<ChequeDetailSellerSideDTO?> Handle(ShowChequeDetailForSellerReviewQuery request, CancellationToken cancellationToken)
    {
        return await _orderChequeQueryRepository.Fill_SellerSideChequeDetailDTO_ByOrderChequeId(request.ChequeId, request.SellerUserId , cancellationToken);
    }
}
