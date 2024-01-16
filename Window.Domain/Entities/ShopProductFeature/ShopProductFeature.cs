using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.ShopProductFeature;

public sealed class ShopProductFeature : BaseEntity
{
    #region properties

    public ulong ProductId { get; set; }

    public string FeatureTitle{ get; set; }

    public string FeatureValue { get; set; }

    #endregion
}
