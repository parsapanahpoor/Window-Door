using Window.Domain.Enums.Order;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Seller.ShopOrder;

public class FilterShopOrdersSellerSideDTO : BasePaging<ListOfShopOrdersSellerSideDTO>
{
    #region properties

    public string CustomerName  { get; set; }

    public ulong SellerUserId { get; set; }

    #endregion
}

public class FilterShopOrdersSellerAsCustomerSideDTO : BasePaging<ListOfShopOrdersSellerSideDTO>
{
    #region properties

    public string SellerName { get; set; }

    public ulong CustomerId { get; set; }

    #endregion
}

public class ListOfShopOrdersSellerSideDTO
{
    #region properties

    public ulong OrderId{ get; set; }

    public ListOfShopOrdersSellerSideDTO_CustomerUserInfo? CustomerUserInfo { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsFinally { get; set; }

    public OrderPaymentWay? PaymentWay { get; set; }

    #endregion
}

public class ListOfShopOrdersSellerSideDTO_CustomerUserInfo
{
    #region properties

    public ulong CustomerUserId{ get; set; }

    public string Username { get; set; }

    public string Mobile { get; set; }

    #endregion
}
