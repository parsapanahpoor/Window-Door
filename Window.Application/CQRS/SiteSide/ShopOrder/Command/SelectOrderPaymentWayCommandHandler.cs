
using Window.Application.Common.IUnitOfWork;
using Window.Application.Convertors;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record SelectOrderPaymentWayCommandHandler : IRequestHandler<SelectOrderPaymentWayCommand, SelectOrderPaymentWayResult>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly ISiteSettingService _siteSettingService;
    private static readonly HttpClient client = new HttpClient();
    private readonly IUnitOfWork _unitOfWork;

    public SelectOrderPaymentWayCommandHandler(IOrderQueryRepository orderQueryRepository,
                                               IOrderCommandRepository orderCommandRepository,
                                               ISiteSettingService siteSettingService,
                                               IUnitOfWork unitOfWork)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<SelectOrderPaymentWayResult> Handle(SelectOrderPaymentWayCommand request, CancellationToken cancellationToken)
    {
        //Get Lastest Waiting For Payment Order
        var order = await _orderQueryRepository.GetLastest_WaitingForPaymentOrder_ByUserId(request.UserId,
                                                                                           cancellationToken);
        if (order == null) return SelectOrderPaymentWayResult.Faild;
        if (order.PaymentWay.HasValue &&
           order.PaymentWay.Value == Domain.Enums.Order.OrderPaymentWay.InstallmentPayment) return SelectOrderPaymentWayResult.ChoseInstallerInPass;

        //Update Order Payment Way
        order.PaymentWay = request.OrderPaymentWay;

        _orderCommandRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        if (request.OrderPaymentWay == Domain.Enums.Order.OrderPaymentWay.InstallmentPayment)
        {
            //Send SMS To Admins And Seller
            var adminsMobiles = await _siteSettingService.GetListOf_AdminsMobiles(cancellationToken);
            if (adminsMobiles != null && adminsMobiles.Any())
            {
                string date = $"{DateTime.Now.ToShamsi()}";
                string adminLink = $"{Window.Application.StticTools.FilePaths.SiteAddress}/Admin/OrderCheques/ShowOrderChequeDetail?orderId={order.Id}";

                foreach (var adminMobile in adminsMobiles)
                {
                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={adminMobile}&token=={adminLink}&token2={DateTime.Now.ToShamsi()}&template=NewOrder-ForAdmin";
                    var results = client.GetStringAsync(result);
                }
            }

            return SelectOrderPaymentWayResult.SuccessInstallerPayment;
        }

        return SelectOrderPaymentWayResult.SuccessCashPayment;
    }
}
