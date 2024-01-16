using Microsoft.AspNetCore.Http;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopProductGallery;

public record CreateProductGalleryQuery(ulong sellerUserId , ulong productId , IFormFile image) : IRequest<bool>;
