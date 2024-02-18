using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Site.Shop.Order;

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

    Task<ShopCartOrderDetailItems> FillShopCartOrderDetailItems(ulong orderDetailId, CancellationToken cancellationToken);

    Task<List<ulong>> GetOrderDetailIds_OrderDetails_ByOrderId(ulong orderId, CancellationToken cancellation);

    #endregion
}
