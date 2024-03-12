using Window.Domain.Enums.Order;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Command;

public class ChangeOrderChequeStateAdminCommand : IRequest<bool>
{
    #region properties

    public ulong OrderChequeId { get; set; }

    public OrderChequeAdminState OrderChequeAdminState { get; set; }

    public string? AdminRejectDescription { get; set; }

    #endregion
}
