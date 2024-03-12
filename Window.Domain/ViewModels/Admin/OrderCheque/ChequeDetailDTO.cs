using Window.Domain.Enums.Order;

namespace Window.Domain.ViewModels.Admin.OrderCheque;

public class ChequeDetailDTO
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong ChequeId { get; set; }

    public string ChequeImage { get; set; }

    public decimal ChequePrice { get; set; }

    public string CustomerNationalId { get; set; }

    public OrderChequeSellerState OrderChequeSellerState { get; set; }

    public string? SellerRejectDescription { get; set; }

    public DateTime ChequeDateTime { get; set; }

    public OrderChequeAdminState OrderChequeAdminState { get; set; }

    public string? AdminRejectDescription { get; set; }

    #endregion
}

public class AdminChequeDetailChangeStateDTO
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong OrderChequeId { get; set; }

    public OrderChequeAdminState OrderChequeAdminState { get; set; }

    public string? AdminRejectDescription { get; set; }

    #endregion
}

