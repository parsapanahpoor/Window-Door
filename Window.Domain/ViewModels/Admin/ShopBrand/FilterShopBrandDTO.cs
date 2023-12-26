using Window.Domain.ViewModels.Common;
namespace Window.Domain.ViewModels.Admin.ShopBrand;

public class FilterShopBrandDTO : BasePaging<Entities.ShopBrands.ShopBrand>
{
    #region propreties

    public string? Title { get; set; }

    #endregion
}
