using Microsoft.AspNetCore.Http;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopProductGallery;

public class CreateProductGalleryQuery : IRequest<bool>
{
    public ulong sellerUserId { get; set; }

    public ulong productId { get; set; }

    public IFormFile image { get; set; }
}
