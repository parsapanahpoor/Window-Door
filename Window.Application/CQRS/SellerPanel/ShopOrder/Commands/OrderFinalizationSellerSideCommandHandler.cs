
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Commands;

public record OrderFinalizationSellerSideCommandHandler : IRequestHandler<OrderFinalizationSellerSideCommand, bool>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderFinalizationSellerSideCommandHandler(IOrderQueryRepository orderQueryRepository ,
                                                     IOrderCommandRepository orderCommandRepository ,
                                                     IShopProductQueryRepository shopProductQueryRepository ,
                                                     IUnitOfWork unitOfWork)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(OrderFinalizationSellerSideCommand request, CancellationToken cancellationToken)
    {
        //Get Order by Id
        var order = await _orderQueryRepository.GetByIdAsync(cancellationToken , request.OrderId);
        if (order == null) return false;

        //Check That Is Product Owner And Current Seller Are The Same
        var productId = await _orderQueryRepository.GetLastestProductId_InOrderDetail_ByOrderId(request.OrderId);
        if (productId == 0) return false;

        //Get Product
        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, productId);
        if (product == null) return false;
        if (product.SellerUserId != request.SellerUserId) return false;

        //Order Finalization 
        order.IsFinally = true;

        _orderCommandRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();   

        return true;
    }
}
