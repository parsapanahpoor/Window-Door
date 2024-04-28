using Microsoft.AspNetCore.Http;
using Window.Domain.ViewModels.Seller.Product;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Interfaces;

public interface IShopProductService
{
    #region Admin Side 

    Task<List<ListOfSellerProductCategoriesDTO>?> FillListOf_AdminProductCategoriesDTO(ulong productId,
                                                                                       CancellationToken cancellationToken);

    Task<bool> UpdateProductSelectedCategrory_AdminSide(List<ulong>? categoryIds,
                                                        ulong productId,
                                                        CancellationToken cancellation);

    #endregion

    #region Seller Side 

    Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation);

    Task<CreateShopProductFromSellerPanelResult> AddShopProductToTheDataBase(ulong sellerId,
                                                                                  CreateShopProductSellerSideDTO model,
                                                                                  CancellationToken cancellation);

    Task<EditShopProductSellerSideDTO?> FillEditShopProductSellerSideDTO(ulong productId, ulong sellerId, CancellationToken token);

    Task<List<ulong>> GetShopProductSelectedCategories(ulong productId, CancellationToken token);

    Task<bool> DeleteProduct_AdminSide(ulong productId,
                                       CancellationToken cancellation);

    Task<EditShopProductFromSellerPanelResult> EditShopProductSellerSide(EditShopProductSellerSideDTO newProduct, ulong sellerId, CancellationToken cancellation);

    Task<bool> DeleteArticleAdminSide(ulong productId, ulong sellerId, CancellationToken cancellation);

    Task<List<ListOfSellerProductCategoriesDTO>?> FillListOfSellerProductCategoriesDTO(ulong sellerId,
                                                                                                    ulong productId,
                                                                                                    CancellationToken cancellationToken);

    //Update Product Selected Categrory
    Task<bool> UpdateDoctorSpecialitySelected(List<ulong>? categoryIds,
                                                           ulong sellerId,
                                                           ulong productId,
                                                           CancellationToken cancellation);

    #endregion
}
