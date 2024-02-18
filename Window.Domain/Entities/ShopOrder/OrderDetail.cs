using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.ShopOrder;

public sealed class OrderDetail : BaseEntity
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong ProductId { get; set; }

    public int Count { get; set; }

    #endregion
}
