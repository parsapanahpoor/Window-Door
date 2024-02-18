using Window.Domain.Entities.ShopOrder;

namespace Window.Domain.Interfaces.Order;

public interface IOrderQueryRepository
{
    #region Site Side 

    Task<Domain.Entities.ShopOrder.Order> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    Task<Domain.Entities.ShopOrder.Order?> IsExistAnyWaitingOrder(ulong userId,
                                                                              CancellationToken cancellationToken);

    Task<OrderDetail?> Get_OrderDetail_ByOrderIdAndProductId(ulong orderId,
                                                                         ulong productId,
                                                                         CancellationToken cancellation);

    #endregion
}
