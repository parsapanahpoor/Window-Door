
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Location;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SiteSide.Location.Command;

public record AddLocationIdToOrderCommandHandler : IRequestHandler<AddLocationIdToOrderCommand, AddLocationIdToOrderResult>
{
    #region Ctor

    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly ILocationQueryRepository _locationQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddLocationIdToOrderCommandHandler(IOrderCommandRepository orderCommandRepository,
                                              IOrderQueryRepository orderQueryRepository,
                                              ILocationQueryRepository locationQueryRepository,
                                              IShopProductQueryRepository shopProductQueryRepository ,
                                              IUnitOfWork unitOfWork)
    {
        _locationQueryRepository = locationQueryRepository;
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<AddLocationIdToOrderResult> Handle(AddLocationIdToOrderCommand request, CancellationToken cancellationToken)
    {
        //Get Location By Id
        var location = await _locationQueryRepository.GetByIdAsync(cancellationToken, request.LocationId);
        if (location == null || location.UserId != request.UserId) return AddLocationIdToOrderResult.OrderNotFound;

        //Check That is exist any Waiting for payment order 
        if (await _orderQueryRepository.IsExistAnyOrderInWaitingForPaymentStateByUserId(request.UserId , cancellationToken))
        {
            return AddLocationIdToOrderResult.WaitingForPaymentOrder;
        }

        //Get Order By UserId
        var order = await _orderQueryRepository.GetLastestWaitingForInformationOrderByUserId(request.UserId , cancellationToken);
        if (order == null) return AddLocationIdToOrderResult.OrderNotFound;

        //Update Oredr
        order.LocationId = request.LocationId;
        order.Price = 0;

        var orderDetails = await _orderQueryRepository.GetOrderDetails_InOrderDetails_ByOrderId(order.Id, cancellationToken);
        if (orderDetails == null || !orderDetails.Any()) return AddLocationIdToOrderResult.OrderNotFound;

        foreach (var orderDetail in orderDetails)
        {
            var productPrice = await _shopProductQueryRepository.GetProductPrice_ByProductId(orderDetail.ProductId, cancellationToken);

            order.Price = (ulong?)(order.Price.Value + (productPrice * orderDetail.Count));
        }

        _orderCommandRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return AddLocationIdToOrderResult.Success;
    }
}
