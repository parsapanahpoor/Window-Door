using Window.Domain.Entities.Common;
using Window.Domain.Enums.Order;

namespace Window.Domain.Entities.ShopOrder;

public sealed class OrderCheque : BaseEntity
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong CustomerUserId{ get; set; }

    public ulong SellerUserId { get; set; }

    public string ChequeImage { get; set; }

    public decimal ChequePrice { get; set; }

    public string CustomerNationalId { get; set; }

    public OrderChequeAdminState OrderChequeAdminState { get; set; }

    public OrderChequeSellerState OrderChequeSellerState { get; set; }

    public string? SellerRejectDescription { get; set; }

    public string? AdminRejectDescription { get; set; }

    public DateTime ChequeDateTime { get; set; }

    public string? ChequeReceipt { get; set; }

    #endregion
}
