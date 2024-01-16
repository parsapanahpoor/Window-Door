namespace Window.Domain.Interfaces.ShopProductGallery;

public interface IShopProductGalleryCommandRepository
{
    #region Admin Side 

    Task AddAsync(Domain.Entities.ShopProductGallery.ShopProductGallery shopProductGallery, CancellationToken cancellationToken);

    void Update(Domain.Entities.ShopProductGallery.ShopProductGallery shopProductGallery);

    #endregion
}
