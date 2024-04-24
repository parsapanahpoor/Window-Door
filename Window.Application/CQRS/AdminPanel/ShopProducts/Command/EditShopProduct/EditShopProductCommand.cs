using Microsoft.AspNetCore.Http;
using Window.Domain.ViewModels.Admin.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.EditShopProduct;

public record EditShopProductCommand : IRequest<EditShopProductFromAdminPanelResult>
{
    #region properties

    public EditShopProductAdminSideDTO model { get; set; }

    #endregion
}
