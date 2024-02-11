using Window.Domain.ViewModels.Site.Shop.SellerDetail;

namespace Window.Application.CQRS.SiteSide.ShopSeller.ShopSellerDetail;

public record ShopSellerDetailQuery(ulong sellerId) : IRequest<SellerDetailDTO>;
