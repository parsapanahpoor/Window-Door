using Window.Domain.Entities.Brand;

namespace Window.Domain.ViewModels.Site.Shop.Landing;

public record ShopSiteBarDTO
{
    #region properties

    public List<ShopCategoriesDTO>? ShopCategoriesDTOs { get; set; }

    public List<ShopBrandsDTO>? ShopBrandsDTOs { get; set; }

    #endregion
}
