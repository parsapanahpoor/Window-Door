using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Seller.ShopProduct;

public class FilterShopProductSellerSideDTO : BasePaging<Domain.Entities.ShopProduct.ShopProduct>
{
    #region properties

    public ulong SellerUserId { get; set; }

    public string ProductName { get; set; }

    #endregion
}
