using Window.Domain.Entities.Brand;

namespace Window.Domain.ViewModels.Seller.ShopProduct;

public record ShopProductsBrandDTO
{
    #region properties

    public BrandCategory BrandCategory { get; set; }

    public List<MainBrand> MainBrands { get; set; }

    #endregion
}
