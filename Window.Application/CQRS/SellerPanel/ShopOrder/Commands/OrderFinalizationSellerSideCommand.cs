using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Commands;

public record OrderFinalizationSellerSideCommand : IRequest<bool>
{
    #region properties

    public ulong SellerUserId { get; set; }

    public ulong OrderId { get; set; }

    #endregion
}
