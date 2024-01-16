namespace Window.Domain.Interfaces.ShopProductFeature;

public interface IShopProductFeatureCommandRepository
{
    #region Admin Side 

    Task AddAsync(Domain.Entities.ShopProductFeature.ShopProductFeature productFeature, CancellationToken cancellationToken);

    void Update(Domain.Entities.ShopProductFeature.ShopProductFeature productFeature);

    #endregion
}
