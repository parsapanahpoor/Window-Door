using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Domain.Interfaces.ShopProductGallery;

public interface IShopProductGalleryQueryRepository
{
    #region Admin 

    Task<Domain.Entities.ShopProductGallery.ShopProductGallery> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side

    Task<bool> IsExistBrandById(ulong brandId, CancellationToken cancellation);

    #endregion

    #region Seller Side 

    Task<List<ProductGalleriesDTO>?> FillProductGalleriesDTO(ulong productId, CancellationToken token);

    #endregion
}
