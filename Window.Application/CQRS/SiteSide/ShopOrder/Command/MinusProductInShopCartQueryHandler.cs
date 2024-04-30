
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record MinusProductInShopCartQueryHandler : IRequestHandler<MinusProductInShopCartQuery, bool>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MinusProductInShopCartQueryHandler(IOrderCommandRepository orderCommandRepository,
                                              IShopProductQueryRepository shopProductQueryRepository,
                                              IOrderQueryRepository orderQueryRepository,
                                              IUnitOfWork unitOfWork)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(MinusProductInShopCartQuery request, CancellationToken cancellationToken)
    {
        #region Get Order By User Id

        var order = await _orderQueryRepository.IsExistAnyWaitingOrder(request.UserId, cancellationToken);
        if (order == null) return false;

        #endregion

        #region Get Shop Product By Id 

        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken , request.productId);
        if (product == null) return false;

        #endregion

        #region Get Order Details ProductIds By Order Id 

        var productIds = await _orderQueryRepository.GetProductIds_InOrderDetails_ByOrderId(order.Id, cancellationToken);
        if (productIds == null || !productIds.Any()) return false;

        #endregion

        #region Is Exist Order Detail bt orderid and product id 

        var orderDetail = await _orderQueryRepository.Get_OrderDetail_ByOrderIdAndProductId(order.Id, request.productId, cancellationToken);
        if (orderDetail == null || orderDetail.Count == 0 ) return false;

        #endregion

        #region Update order detail 

        orderDetail.Count = product.SalesRatio == 0 ? orderDetail.Count-- : orderDetail.Count - product.SalesRatio;
        if (orderDetail.Count < 0 ) return false;

        if (orderDetail.Count == 0)
        {
            orderDetail.IsDelete = true;

            #region Remove Order 

            if (productIds.Count() == 1 && productIds.Contains(orderDetail.ProductId))
            {
                order.IsDelete = true;

                _orderCommandRepository.Update(order);
            }

            #endregion
        }

        _orderCommandRepository.UpdateOrderDetail(orderDetail);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
