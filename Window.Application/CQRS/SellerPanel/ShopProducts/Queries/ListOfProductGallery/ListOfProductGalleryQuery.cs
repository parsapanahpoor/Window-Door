using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductGallery;

public record ListOfProductGalleryQuery(ulong productId) : IRequest<List<ProductGalleriesDTO>>;

