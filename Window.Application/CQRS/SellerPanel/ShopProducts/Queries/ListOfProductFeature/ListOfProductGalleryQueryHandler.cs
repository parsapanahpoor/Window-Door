using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Window.Domain.Interfaces.ShopProductFeature;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Queries.ListOfProductFeature;

public record ListOfProductFeatureQueryHandler : IRequestHandler<ListOfProductFeatureQuery, List<ProductFeaturesDTO>>
{
    #region Ctor

    private readonly IShopProductFeatureQueryRepository _shopProductFeatureQueryRepository;

    public ListOfProductFeatureQueryHandler(IShopProductFeatureQueryRepository shopProductFeatureQueryRepository)
    {
            _shopProductFeatureQueryRepository = shopProductFeatureQueryRepository;
    }

    #endregion

    public async Task<List<ProductFeaturesDTO>?> Handle(ListOfProductFeatureQuery request,
                                                                      CancellationToken cancellationToken)
    {
        return await _shopProductFeatureQueryRepository.FillProductFeaturesDTO(request.productId, cancellationToken);
    }
}
