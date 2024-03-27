using Window.Domain.Enums.Order;

namespace Window.Domain.ViewModels.Seller.OrderCheque;

public class ChequeDetailSellerSideDTO
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong ChequeId { get; set; }

    public string ChequeImage { get; set; }

    public string? ChequeReceiptImage { get; set; }

    public string? ChequeReceiptDescription { get; set; }

    public decimal ChequePrice { get; set; }

    public string CustomerNationalId { get; set; }

    public OrderChequeSellerState OrderChequeSellerState { get; set; }

    public string? SellerRejectDescription { get; set; }

    public DateTime ChequeDateTime { get; set; }

    public OrderChequeAdminState OrderChequeAdminState { get; set; }

    public string? AdminRejectDescription { get; set; }

    #endregion
}

public class SellerChequeDetailChangeStateDTO
{
    #region properties

    public ulong SellerUserId { get; set; }

    public ulong OrderId { get; set; }

    public ulong OrderChequeId { get; set; }

    public OrderChequeSellerState OrderChequeSellerState { get; set; }

    public string? SellerRejectDescription { get; set; }

    #endregion
}
