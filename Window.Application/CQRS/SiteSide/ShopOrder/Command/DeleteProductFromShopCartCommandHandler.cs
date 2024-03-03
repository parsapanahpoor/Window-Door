using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;
namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record DeleteProductFromShopCartCommandHandler : IRequestHandler<DeleteProductFromShopCartCommand, bool>
{
    #region Ctor

    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductFromShopCartCommandHandler(IOrderCommandRepository orderCommandRepository,
                                                   IOrderQueryRepository orderQueryRepository,
                                                   IUnitOfWork unitOfWork)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(DeleteProductFromShopCartCommand request, CancellationToken cancellationToken)
    {
        #region Get Order By User Id

        var order = await _orderQueryRepository.IsExistAnyWaitingOrder(request.UserId, cancellationToken);
        if (order == null) return false;

        #endregion

        #region Get Order Details ProductIds By Order Id 

        var productIds = await _orderQueryRepository.GetProductIds_InOrderDetails_ByOrderId(order.Id , cancellationToken);
        if (productIds == null || !productIds.Any()) return false;

        #endregion

        #region Is Exist Order Detail bt orderid and product id 

        var orderDetail = await _orderQueryRepository.Get_OrderDetail_ByOrderIdAndProductId(order.Id, request.ProductId, cancellationToken);
        if (orderDetail == null || orderDetail.Count == 0) return false;

        #endregion

        #region Remove Order 

        if (productIds.Count() == 1 && productIds.Contains(orderDetail.ProductId))
        {
            order.IsDelete = true;

            _orderCommandRepository.Update(order);
        }

        #endregion

        #region Update order detail 

        orderDetail.IsDelete = true;

        _orderCommandRepository.UpdateOrderDetail(orderDetail);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
