using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;

public record ManageShopOrderDetailQuery(ulong userId) : IRequest<ManageShopOrderDetailDTO>;
