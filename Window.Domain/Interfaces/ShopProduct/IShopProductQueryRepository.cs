using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Domain.Interfaces.ShopProduct;

public interface IShopProductQueryRepository
{
    #region General Methods

    Task<Domain.Entities.ShopProduct.ShopProduct> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Seller Side 

    Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation);

    Task<List<ulong>> GetShopProductSelectedCategories(ulong productId, CancellationToken token);

    #endregion
}
