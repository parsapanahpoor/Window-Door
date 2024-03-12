using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Query;

public class ShowOrderChequeDetailQuery : IRequest<ShowOrderChequeDetailAdminDTO>
{
    #region properties

    public ulong orderId { get; set; }

    #endregion
}
