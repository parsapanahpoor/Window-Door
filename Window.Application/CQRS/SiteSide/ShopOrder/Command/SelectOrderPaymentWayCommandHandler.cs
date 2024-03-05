
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record SelectOrderPaymentWayCommandHandler : IRequestHandler<SelectOrderPaymentWayCommand, SelectOrderPaymentWayResult>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SelectOrderPaymentWayCommandHandler(IOrderQueryRepository orderQueryRepository,
                                               IOrderCommandRepository orderCommandRepository,
                                               IUnitOfWork unitOfWork)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<SelectOrderPaymentWayResult> Handle(SelectOrderPaymentWayCommand request, CancellationToken cancellationToken)
    {
        //Get Lastest Waiting For Payment Order
        var order = await _orderQueryRepository.GetLastest_WaitingForPaymentOrder_ByUserId(request.UserId,
                                                                                           cancellationToken);
        if (order == null) return SelectOrderPaymentWayResult.Faild;

        //Update Order Payment Way
        order.PaymentWay = request.OrderPaymentWay;

        _orderCommandRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        if (request.OrderPaymentWay == Domain.Enums.Order.OrderPaymentWay.InstallmentPayment)
            return SelectOrderPaymentWayResult.SuccessInstallerPayment;

        return SelectOrderPaymentWayResult.SuccessCashPayment;
    }
}
