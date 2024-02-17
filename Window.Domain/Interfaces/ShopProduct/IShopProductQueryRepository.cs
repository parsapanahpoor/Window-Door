using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.SellerDetail;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Domain.Interfaces.ShopProduct;

public interface IShopProductQueryRepository
{

    #region Site Side 

    //List Of Products
    Task<FilterShopProductDTO> FilterProducts(FilterShopProductDTO model, CancellationToken cancellation);

    Task<List<ShopCard>> FillShopCard(ulong sellerUserId, CancellationToken cancellation);

    Task<List<LastestShopProducts>> FillLastestShopProducts(CancellationToken cancellation);

    Task<List<LastestSellers>> ListOfLastestSellers(CancellationToken cancellation);

    Task<List<LastestBrands>> LastestMainBrands(CancellationToken cancellationToken);

    #endregion

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
