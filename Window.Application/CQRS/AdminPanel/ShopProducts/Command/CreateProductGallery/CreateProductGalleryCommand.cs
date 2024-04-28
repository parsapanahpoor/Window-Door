using Microsoft.AspNetCore.Http;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.CreateProductGallery;

public record CreateProductGalleryCommand : IRequest<bool>
{
    public ulong productId { get; set; }

    public IFormFile image { get; set; }
}