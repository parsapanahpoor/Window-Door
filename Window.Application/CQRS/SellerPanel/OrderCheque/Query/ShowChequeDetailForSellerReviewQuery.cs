using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Query;

public class ShowChequeDetailForSellerReviewQuery : IRequest<ChequeDetailSellerSideDTO>
{
    #region properties

    public ulong SellerUserId { get; set; }

    public ulong ChequeId { get; set; }

    #endregion
}
