using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.ShopBrands;

public sealed class ShopProductsSelectedBrands : BaseEntity
{
    #region properties

    public ulong ShopProductId { get; set; }

    public ulong ShopBrandId { get; set; }

    #endregion
}
