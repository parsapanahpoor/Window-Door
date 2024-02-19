
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record MinusProductInShopCartQueryHandler : IRequestHandler<MinusProductInShopCartQuery, bool>
{
    #region Ctor

    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MinusProductInShopCartQueryHandler(IOrderCommandRepository orderCommandRepository,
                                             IOrderQueryRepository orderQueryRepository,
                                             IUnitOfWork unitOfWork)
    {
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

        #region Is Exist Order Detail bt orderid and product id 

        var orderDetail = await _orderQueryRepository.Get_OrderDetail_ByOrderIdAndProductId(order.Id, request.productId, cancellationToken);
        if (orderDetail == null || orderDetail.Count == 0) return false;

        #endregion

        #region Update order detail 

        if (orderDetail.Count > 1)
        {
            orderDetail.Count--;
        }
        else
        {
            orderDetail.Count--;
            orderDetail.IsDelete = true;
        }

        _orderCommandRepository.UpdateOrderDetail(orderDetail);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
