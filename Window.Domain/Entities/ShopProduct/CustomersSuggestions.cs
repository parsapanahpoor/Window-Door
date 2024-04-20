using Window.Domain.Entities.Common;
namespace Window.Domain.Entities.ShopProduct;

public sealed class CustomersSuggestions : BaseEntity
{
    #region properties

    public ulong ShopProductId { get; set; }

    #endregion
}
