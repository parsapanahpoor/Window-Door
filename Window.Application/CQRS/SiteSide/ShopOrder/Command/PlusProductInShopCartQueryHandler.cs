
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record PlusProductInShopCartQueryHandler : IRequestHandler<PlusProductInShopCartQuery, bool>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlusProductInShopCartQueryHandler(IOrderCommandRepository orderCommandRepository,
                                             IOrderQueryRepository orderQueryRepository,
                                             IShopProductQueryRepository shopProductQueryRepository,
                                             IUnitOfWork unitOfWork)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(PlusProductInShopCartQuery request, CancellationToken cancellationToken)
    {
        #region Get Order By User Id

        var order = await _orderQueryRepository.IsExistAnyWaitingOrder(request.UserId , cancellationToken);
        if (order == null) return false;

        #endregion

        #region Get Shop Product By Id 

        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken , request.productId);
        if (product == null) return false;

        #endregion

        #region Is Exist Order Detail bt orderid and product id 

        var orderDetail = await _orderQueryRepository.Get_OrderDetail_ByOrderIdAndProductId(order.Id , request.productId , cancellationToken);
        if (orderDetail == null) return false;

        #endregion

        #region Update order detail 

        orderDetail.Count = (int)(product.SalesRatio == 0 ? orderDetail.Count + 1 : orderDetail.Count + product.SalesRatio);

        _orderCommandRepository.UpdateOrderDetail(orderDetail);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
