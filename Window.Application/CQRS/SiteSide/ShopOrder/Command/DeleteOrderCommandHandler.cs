
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    #region Ctor

    private readonly IOrderQueryRepository _shopProductQueryRepository;
    private readonly IOrderCommandRepository _shopProductCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IOrderQueryRepository shopProductQueryRepository ,
                                     IOrderCommandRepository shopProductCommandRepository ,
                                     IUnitOfWork unitOfWork)
    {
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        //Get Lastest Waiting For Payment Order
        var order = await _shopProductQueryRepository.GetLastest_WaitingForPaymentOrder_ByUserId(request.UserId, cancellationToken);
        if (order == null || order.IsFinally) return false;

        //Remove Order Details
        var orderDetails = await _shopProductQueryRepository.GetOrderDetails_ByOrderId(order.Id , cancellationToken);
        if (orderDetails != null && orderDetails.Any())
        {
            _shopProductCommandRepository.DeleteRangeOrderDetails(orderDetails);
        }

        //Remove Order
        _shopProductCommandRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
