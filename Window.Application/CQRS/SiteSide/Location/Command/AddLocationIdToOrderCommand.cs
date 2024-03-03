namespace Window.Application.CQRS.SiteSide.Location.Command;

public class AddLocationIdToOrderCommand : IRequest<AddLocationIdToOrderResult>
{
    public ulong UserId { get; set; }

    public ulong LocationId { get; set; }
}

public enum AddLocationIdToOrderResult
{
    Success,
    OrderNotFound,
    WaitingForPaymentOrder,
}
