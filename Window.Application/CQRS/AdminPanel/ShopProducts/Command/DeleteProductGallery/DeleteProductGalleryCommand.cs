namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.DeleteProductGallery;

public record DeleteProductGalleryCommand_AdminSide : IRequest<DeleteProductGalleryCommand_Result>
{
    public ulong galleryId { get; set; }
}

public record DeleteProductGalleryCommand_Result
{
    public ulong ProductId { get; set; }

    public bool Result { get; set; }
}