using Window.Domain.Entities.Location;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Domain.ViewModels.Seller.ShopOrder;

public record ManageShopOrderDetailDTO
{
    #region properties

    public Domain.Entities.ShopOrder.Order? Order { get; set; }

    public SellerInformations? SellerInformations { get; set; }

    public CustomerChequeInformation? CustomerChequeInformation { get; set; }

    public Location? Location { get; set; }

    public List<ManageShopOrderDetailAdminDTO>? OrderDetails { get; set; }

    public List<Domain.Entities.ShopOrder.OrderCheque>? OrderCheques { get; set; }

    public SellerChequeInfo? sellerChequeInfo { get; set; }

    public Entities.Account.User? CustomerUserInformations { get; set; }

    #endregion
}

public record SellerInformations
{
    #region properties

    public ulong SellerId  { get; set; }

    public string SellerName { get; set; }

    public string SellerUserAvatar { get; set; }

    #endregion
}

public record CustomerChequeInformation
{
    #region properties

    public int ChequeDays { get; set; }

    public int CountOfCheque { get; set; }

    #endregion
}
