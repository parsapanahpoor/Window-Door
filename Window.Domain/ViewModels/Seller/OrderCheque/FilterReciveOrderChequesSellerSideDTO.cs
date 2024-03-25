using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Seller.OrderCheque;

public class FilterReciveOrderChequesSellerSideDTO : BasePaging<OrderChequesDTO>
{
    #region properties

    public OrderChequeAdminStateDTO? OrderChequeAdminState { get; set; }

    public OrderChequeSellerStateDTO? OrderChequeSellerState { get; set; }

    public string? CustomerUsername { get; set; }

    public OrderFinallyStateDTO? OrderFinallyState { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public ulong UserId { get; set; }

    #endregion
}
