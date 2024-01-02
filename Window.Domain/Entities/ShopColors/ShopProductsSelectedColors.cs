using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.ShopColors;

public sealed class ShopProductsSelectedColors : BaseEntity
{
    #region properties

    public ulong ShopProductId { get; set; }

    public ulong ColorId { get; set; }

    #endregion
}
