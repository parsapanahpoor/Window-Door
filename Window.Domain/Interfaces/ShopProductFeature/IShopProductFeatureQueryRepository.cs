using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Domain.Interfaces.ShopProductFeature;

public interface IShopProductFeatureQueryRepository
{
    #region Admin 

    Task<Domain.Entities.ShopProductFeature.ShopProductFeature> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side

    Task<bool> IsExistBrandById(ulong brandId, CancellationToken cancellation);

    #endregion
}
