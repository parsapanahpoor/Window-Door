using Window.Domain.Entities.Common;
using Window.Domain.Enums.Order;
namespace Window.Domain.Entities.ShopOrder;

public sealed class Order : BaseEntity
{
    #region properties

    public ulong UserId { get; set; }

    public ulong? LocationId { get; set; }

    public decimal? Price { get; set; }

    public bool IsFinally { get; set; }

    public OrderState OrderState { get; set; }

    #endregion
}
