using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.ShopColors;
using Window.Domain.Enums.Order;

namespace Window.Domain.ViewModels.Seller.ShopOrder;

public record ShowOrderFactorDTO 
{
    #region Properties

    public ulong OrdeId { get; set; }

    public bool IsFinally { get; set; }

    public OrderPaymentWay? PaymentWay { get; set; }

    public decimal? Price { get; set; }

    public DateTime CreateDate { get; set; }

    public ShowOrderFactor_CustmoerInfoDTO? CustomerInfo { get; set; }

    public Location? Location { get; set; }

    public List<ShowOrderFactor_OrderDetailDTO>? OrderDetails { get; set; }

    #endregion
}

public record ShowOrderFactor_OrderDetailDTO
{
    #region proeprties

    public ulong OrderDetailId { get; set; }

    public int Count { get; set; }

    public ShowOrderFactor_ProductDetailsDTO? Product { get; set; }

    #endregion
}

public record ShowOrderFactor_ProductDetailsDTO
{
    #region properties

    public ulong ProductId { get; set; }

    public ShowOrderFactor_ProductSellerInfoDTO? SellerInfo { get; set; }

    public ShopColor? ShopColor { get; set; }

    public MainBrand? ShopBrand { get; set; }

    public string? ScaleTitle { get; set; }

    public string ProductName { get; set; }

    public decimal Price { get; set; }

    #endregion
}

public record ShowOrderFactor_ProductSellerInfoDTO
{
    #region properties

    public ulong SellerUserId { get; set; }

    public string SellerUsername { get; set; }

    public string SellerMobile { get; set; }

    #endregion
}

public record ShowOrderFactor_CustmoerInfoDTO
{
    #region properties

    public ulong SellerUserId { get; set; }

    public string CustomerUsername { get; set; }

    public string CustoemrMobile { get; set; }

    #endregion
}
