using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopProduct;
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

    Task<List<ProductTag>> GetListOfProductTagsByProductId(ulong productId, CancellationToken cancellation);

    Task<List<ShopProductSelectedCategories>?> GetListOf_ShopProductSelectedCategories_ByProductId(ulong productId,
                                                                                                   CancellationToken cancellation);

    #endregion
}
