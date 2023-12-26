using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Domain.Interfaces.ShopProduct;

public interface IShopProductQueryRepository
{
    #region Seller Side 

    Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation);

    #endregion
}
