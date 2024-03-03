namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public class IsExistAnyWaitingForPaymentOrderQuery : IRequest<bool>
{
    public ulong UserId { get; set; }
}
