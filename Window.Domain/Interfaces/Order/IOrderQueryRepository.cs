using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Site.Shop.Order;

namespace Window.Domain.Interfaces.Order;

public interface IOrderQueryRepository
{
    #region Site Side 

    Task<Domain.Entities.ShopOrder.Order?> IsExistAnyNotFinallyOrder(ulong userId,
                                                                             CancellationToken cancellationToken);

    Task<Domain.Entities.ShopOrder.Order> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    Task<Domain.Entities.ShopOrder.Order?> IsExistAnyWaitingOrder(ulong userId,
                                                                              CancellationToken cancellationToken);

    Task<OrderDetail?> Get_OrderDetail_ByOrderIdAndProductId(ulong orderId,
                                                                         ulong productId,
                                                                         CancellationToken cancellation);

    Task<List<ulong>> GetProductIds_InOrderDetails_ByOrderId(ulong orderId,
                                                                          CancellationToken cancellation);

    Task<ShopCartOrderDetailItems> FillShopCartOrderDetailItems(ulong orderDetailId, CancellationToken cancellationToken);

    Task<List<ulong>> GetOrderDetailIds_OrderDetails_ByOrderId(ulong orderId, CancellationToken cancellation);

    Task<List<OrderDetail>> GetOrderDetails_ByOrderId(ulong orderId, CancellationToken cancellation);

    Task<Domain.Entities.ShopOrder.Order?> GetLastestWaitingForInformationOrderByUserId(ulong UserId,
                                                                                        CancellationToken cancellation);

    Task<bool> IsExistAnyOrderInWaitingForPaymentStateByUserId(ulong userId,
                                                               CancellationToken cancellationToken);

    Task<List<OrderDetail>> GetOrderDetails_InOrderDetails_ByOrderId(ulong orderId,
                                                                     CancellationToken cancellationToken);

    Task<Entities.ShopOrder.Order?> GetLastest_WaitingForPaymentOrder_ByUserId(ulong UserId,
                                                                                      CancellationToken cancellation);

    Task<InvoiceDTO?> FillInvoiceDTO(ulong userId,
                                     ulong orderId,
                                     CancellationToken cancellationToken);

    Task<Entities.ShopOrder.Order?> GetLastest_NotFinally_Order(ulong userId);

    #endregion
}
