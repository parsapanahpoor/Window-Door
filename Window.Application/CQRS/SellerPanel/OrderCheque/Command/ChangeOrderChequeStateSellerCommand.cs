using Window.Domain.Enums.Order;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Command;

public class ChangeOrderChequeStateSellerCommand : IRequest<bool>
{
    #region properties

    public ulong OrderChequeId { get; set; }

    public ulong SellerUserId { get; set; }

    public OrderChequeSellerState OrderChequeSellerState { get; set; }

    public string? SellerRejectDescription { get; set; }

    #endregion
}
