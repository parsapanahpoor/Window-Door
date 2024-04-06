using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopProduct.Query;

public class FilterProductsQuery : IRequest<FilterShopProductDTO>
{
    #region properties
    public List<ulong>? ColorsId { get; set; }

    public List<ulong>? BrandId { get; set; }

    public List<ulong>? ShopCategoryId { get; set; }

    public int? MaxPrice { get; set; }

    public int? MinPrice { get; set; }

    public string? ProductTitle { get; set; }

    #endregion
}
