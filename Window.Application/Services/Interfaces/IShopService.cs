using Microsoft.AspNetCore.Http;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Interfaces;

public interface IShopProductService
{
    #region Seller Side 

    Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation);

    Task<CreateShopProductFromSellerPanelResult> AddShopProductToTheDataBase(ulong sellerId,
                                                                                  CreateShopProductSellerSideDTO model,
                                                                                  IFormFile newsImage,
                                                                                  CancellationToken cancellation);

    #endregion
}
