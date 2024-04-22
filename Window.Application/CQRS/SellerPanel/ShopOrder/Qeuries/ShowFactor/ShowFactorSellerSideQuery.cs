using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries.ShowFactor;

public record ShowFactorSellerSideQuery : IRequest<ShowOrderFactorDTO>
{
    #region proeprties

    public ulong OrderId { get; set; }

    #endregion
}
