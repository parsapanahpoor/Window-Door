using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;

public record ManageShopOrderDetailQuery : IRequest<ManageShopOrderDetailDTO>
{
    #region properties

    public ulong userId { get; set; }

    public ulong? orderId { get; set; }

    #endregion
}
