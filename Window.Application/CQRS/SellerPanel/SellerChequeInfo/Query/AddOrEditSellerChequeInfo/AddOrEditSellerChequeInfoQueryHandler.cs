using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Seller.SellerChequeInfo;

namespace Window.Application.CQRS.SellerPanel.SellerChequeInfo.Query.AddOrEditSellerChequeInfo;

public record AddOrEditSellerChequeInfoQueryHandler : IRequestHandler<AddOrEditSellerChequeInfoQuery, SellerChequeInfoSellerSideDTO>
{
    #region ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public AddOrEditSellerChequeInfoQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<SellerChequeInfoSellerSideDTO?> Handle(AddOrEditSellerChequeInfoQuery request, CancellationToken cancellationToken)
    {
        return await _orderChequeQueryRepository.Fill_SellerChequeInfoSellerSide_DTO(request.SellerUserId , cancellationToken);
    }
}
