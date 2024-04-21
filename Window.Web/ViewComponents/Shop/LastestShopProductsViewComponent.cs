using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.Landing;
namespace Window.Web.ViewComponents.Shop;

public class LastestShopProductsViewComponent : ViewComponent
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public LastestShopProductsViewComponent(IShopProductQueryRepository shopProductQueryRepository)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        return View("LastestShopProducts", await _shopProductQueryRepository.Fill_LastestCustomersSuggestionsProducts(cancellationToken));
    }

}
