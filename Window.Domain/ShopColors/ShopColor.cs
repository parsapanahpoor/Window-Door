using Window.Domain.Entities.Common;

namespace Window.Domain.ShopColors;

public sealed class ShopColor : BaseEntity
{
    #region properties

    public string ColorTitle{ get; set; }

    public string ColorCode { get; set; }

    #endregion
}
