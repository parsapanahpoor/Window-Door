using Window.Domain.Enums.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Command;

public record SelectOrderPaymentWayCommand : IRequest<SelectOrderPaymentWayResult>
{
    #region properties

    public ulong UserId{ get; set; }

    public OrderPaymentWay OrderPaymentWay{ get; set; }

    #endregion
}

public enum SelectOrderPaymentWayResult
{
    SuccessInstallerPayment,
    SuccessCashPayment,
    Faild
}
