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

    Task<EditShopProductSellerSideDTO?> FillEditShopProductSellerSideDTO(ulong productId, ulong sellerId, CancellationToken token);

    Task<List<ulong>> GetShopProductSelectedCategories(ulong productId, CancellationToken token);

    Task<EditShopProductFromSellerPanelResult> EditShopProductSellerSide(EditShopProductSellerSideDTO newProduct, ulong sellerId, IFormFile? newsImage, CancellationToken cancellation);

    Task<bool> DeleteArticleAdminSide(ulong productId, ulong sellerId, CancellationToken cancellation);

    #endregion
}
