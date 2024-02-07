using Window.Domain.Entities.Brand;

namespace Window.Domain.ViewModels.Site.Shop.Landing;

public record ShopBrandForMenuDTO
{
    #region properties

    public BrandCategory BrandCategory { get; set; }

    public List<ShopBrandsDTO> ShopBrands { get; set; }

    #endregion
}

public record ShopBrandsDTO
{
    #region properties

    public ulong ShopBrandId { get; set; }

    public string ShopBrandTitle { get; set; }

    #endregion
}
