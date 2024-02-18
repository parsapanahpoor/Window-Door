using Window.Domain.Entities.Common;
namespace Window.Domain.Entities.ShopOrder;

public sealed class Order : BaseEntity
{
    #region properties

    public ulong UserId { get; set; }

    public bool IsFinally { get; set; }

    #endregion
}
