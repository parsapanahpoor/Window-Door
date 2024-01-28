using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Domain.Interfaces.ShopProductFeature;

public interface IShopProductFeatureQueryRepository
{
    #region Admin 

    Task<Domain.Entities.ShopProductFeature.ShopProductFeature> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side

    Task<bool> IsExistBrandById(ulong brandId, CancellationToken cancellation);

    #endregion

    #region Seller Side 

    Task<List<ProductFeaturesDTO>?> FillProductFeaturesDTO(ulong productId, CancellationToken token);

    #endregion
}
