using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Query;

public class ShowChequeDetailForAdminReviewQuery : IRequest<ChequeDetailDTO>
{
    #region properties

    public ulong ChequeId { get; set; }

    #endregion
}
