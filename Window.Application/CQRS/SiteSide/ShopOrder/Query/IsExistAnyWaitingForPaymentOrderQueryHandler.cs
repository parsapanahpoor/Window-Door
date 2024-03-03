
using Window.Domain.Interfaces.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public record IsExistAnyWaitingForPaymentOrderQueryHandler : IRequestHandler<IsExistAnyWaitingForPaymentOrderQuery, bool>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;

    public IsExistAnyWaitingForPaymentOrderQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository   = orderQueryRepository;
    }

    #endregion

    public async Task<bool> Handle(IsExistAnyWaitingForPaymentOrderQuery request, CancellationToken cancellationToken)
    {
        //Check That is exist any Waiting for payment order 
        return await _orderQueryRepository.IsExistAnyOrderInWaitingForPaymentStateByUserId(request.UserId, cancellationToken);
    }
}
