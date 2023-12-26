using Window.Domain.Entities;
using Window.Domain.ViewModels.Common;
namespace Window.Domain.ViewModels.Site.Shop.ShopProduct;

public class FilterShopProductDTO : BasePaging<ShopCategory>
{
    #region properties

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