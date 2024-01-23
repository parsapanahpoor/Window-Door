using Window.Domain.Entities;
using Window.Domain.ViewModels.Common;
namespace Window.Domain.ViewModels.Site.Shop.ShopProduct;

public class FilterShopProductDTO : BasePaging<Entities.ShopProduct.ShopProduct>
{
    #region properties

    public List<ulong>? ColorsId { get; set; }

    public List<ulong>? BrandId { get; set; }

    public int? MaxPrice { get; set; }

    public int? MinPrice { get; set; }

    public string? ProductTitle { get; set; }

    public List<ulong>? shopCategories { get; set; }

    public ulong? ShopCategoryParentId { get; set; }

    #endregion
}


public record ShopCategoriesForShowInFilterShopProduct 
{
    #region properties

    public ulong ShopCategoryId { get; set; }

    public string ShopCategoryTitle { get; set; }

    #endregion
}