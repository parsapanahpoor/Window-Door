namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteProductGallery;

public record DeleteProductGalleryCommand : IRequest<DeleteProductGalleryCommand_Result>
{
    public ulong galleryId { get; set; }

    public ulong userSellerId { get; set; }
}

public record DeleteProductGalleryCommand_Result
{
    public ulong ProductId{ get; set; }

    public bool Result { get; set; }
}