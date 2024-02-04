using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopProduct.Query;

public record ShopProductDetailQueryHandler : IRequestHandler<ShopProductDetailQuery, ShopProductDetailDTO>
{
    #region Ctor

    private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public ShopProductDetailQueryHandler(IShopColorsQueryRepository shopColorsQueryRepository , 
                                         IShopProductQueryRepository shopProductQueryRepository)
    {
        _shopColorsQueryRepository = shopColorsQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    public Task<ShopProductDetailDTO> Handle(ShopProductDetailQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion

}
