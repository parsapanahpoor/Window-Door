using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.Landing;

namespace Window.Application.CQRS.SiteSide.ShopLanding;

public class ShopLandingQueryHandler : IRequestHandler<ShopLandingQuery, ShopLandingDTO>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public ShopLandingQueryHandler(IShopProductQueryRepository shopProductQueryRepository)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<ShopLandingDTO> Handle(ShopLandingQuery request, CancellationToken cancellationToken)
    {
        ShopLandingDTO model = new ShopLandingDTO();

        //Lastest Shop Products
        model.LastestShopProducts = await _shopProductQueryRepository.FillLastestShopProducts(cancellationToken);

        //Lastest Sellers
        model.LastestSellers = await _shopProductQueryRepository.ListOfLastestSellers(cancellationToken);

        //Lastest Brands
        model.LastestBrands = await _shopProductQueryRepository.LastestMainBrands(cancellationToken);

        return model;
    }
}
