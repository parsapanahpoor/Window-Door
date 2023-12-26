using Window.Domain.ViewModels.Admin.ShopBrand;

namespace Window.Domain.Interfaces.ShopBrands;

public interface IShopBrandsQueryRepository
{
    #region Admin 

    Task<FilterShopBrandDTO> FilterShopBrand(FilterShopBrandDTO filter, CancellationToken cancellation);

    Task<Domain.Entities.ShopBrands.ShopBrand> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion
}
