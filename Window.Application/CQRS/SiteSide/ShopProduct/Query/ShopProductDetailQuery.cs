using Window.Domain.ViewModels.Site.Shop.ShopProduct;
namespace Window.Application.CQRS.SiteSide.ShopProduct.Query;

public record ShopProductDetailQuery(ulong productId) : IRequest<ShopProductDetailDTO>;