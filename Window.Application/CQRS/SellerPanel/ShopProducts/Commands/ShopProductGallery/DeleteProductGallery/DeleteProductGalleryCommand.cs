namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteProductGallery;

public record DeleteProductGalleryCommand : IRequest<bool>
{
    public ulong galleryId { get; set; }

    public ulong userSellerId { get; set; }
}
