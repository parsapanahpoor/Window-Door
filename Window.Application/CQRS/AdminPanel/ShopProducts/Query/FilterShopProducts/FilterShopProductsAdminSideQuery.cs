using Window.Domain.ViewModels.Admin.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Query.FilterShopProducts;

public record FilterShopProductsAdminSideQuery : IRequest<FilterShopProductsAdminSideDTO>
{
    public FilterShopProductsAdminSideDTO filter { get; set; }
}
