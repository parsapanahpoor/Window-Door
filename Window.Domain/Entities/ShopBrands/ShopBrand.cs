using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.ShopBrands;

public sealed class ShopBrand : BaseEntity
{
    #region properties

    public string ShopBrandTitle { get; set; }

    public decimal Priority { get; set; }

    #endregion
}
