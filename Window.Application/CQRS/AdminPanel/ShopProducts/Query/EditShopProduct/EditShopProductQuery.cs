using Window.Domain.ViewModels.Admin.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Query.EditShopProduct;

public record EditShopProductQuery : IRequest<EditShopProductAdminSideDTO>
{
    #region properties

    public ulong ProductId { get; set; }

    #endregion
}
