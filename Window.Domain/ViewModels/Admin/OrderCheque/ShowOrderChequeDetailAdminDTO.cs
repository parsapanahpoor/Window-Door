using Microsoft.EntityFrameworkCore.ChangeTracking;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Domain.ViewModels.Admin.OrderCheque;

public class ShowOrderChequeDetailAdminDTO
{
    #region properties

    public Domain.Entities.ShopOrder.Order? Order { get; set; }

    public Entities.Account.User? SellerInformations { get; set; }

    public CustomerChequeInformationAdminSide? CustomerChequeInformation { get; set; }

    public Location? Location { get; set; }

    public List<ManageShopOrderDetailAdminDTO>? OrderDetails { get; set; }

    public List<Domain.Entities.ShopOrder.OrderCheque>? OrderCheques { get; set; }

    public SellerChequeInfo? sellerChequeInfo { get; set; }

    public Entities.Account.User? CustomerUserInformations{ get; set; }

    #endregion
}

public record CustomerChequeInformationAdminSide
{
    #region properties

    public int ChequeDays { get; set; }

    public int CountOfCheque { get; set; }

    #endregion
}

public record ManageShopOrderDetailAdminDTO
{
    public ProductAdminSideDTO? Product { get; set; }

    public int CountOfChoice { get; set; }
}

public record ProductAdminSideDTO
{
    public ulong ProductId { get; set; }

    public string ProductTitle { get; set; }

    public string ProductImage { get; set; }

    public decimal ProducPrice { get; set; }

    public string ShortDescription { get; set; }
}