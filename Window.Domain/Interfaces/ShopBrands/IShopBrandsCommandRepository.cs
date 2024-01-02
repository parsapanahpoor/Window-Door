namespace Window.Domain.Interfaces.ShopBrands;

public interface IShopBrandsCommandRepository
{
    #region Admin Side 

    Task AddAsync(Domain.Entities.ShopBrands.ShopBrand shopBrand, CancellationToken cancellationToken);

    Task AddShopProductSelectedBrandAsync(Domain.Entities.ShopBrands.ShopProductsSelectedBrands selectedBrands, CancellationToken cancellationToken);

    void Update(Domain.Entities.ShopBrands.ShopBrand shopBrand);

    #endregion
}
