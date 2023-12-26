using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Interfaces;

public interface IShopProductService
{
    #region Seller Side 

    Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation);

    #endregion
}
