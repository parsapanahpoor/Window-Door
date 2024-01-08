using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Domain.Interfaces.ShopBrands;

public interface IShopBrandsQueryRepository
{
    #region Admin 

    Task<FilterShopBrandDTO> FilterShopBrand(FilterShopBrandDTO filter, CancellationToken cancellation);

    Task<Domain.Entities.ShopBrands.ShopBrand> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side

    Task<List<ListOfBrandsForFilterProductsDTO>> FillListOfBrandsForFilterProductsDTO(CancellationToken cancellationToken);

    Task<bool> IsExistBrandById(ulong brandId, CancellationToken cancellation);

    #endregion
}
