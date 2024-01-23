using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopProduct.Query;

public class FilterProductsQueryHandler : IRequestHandler<FilterProductsQuery, FilterShopProductDTO>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public FilterProductsQueryHandler(IShopProductQueryRepository shopProductQueryRepository)
    {
            _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<FilterShopProductDTO> Handle(FilterProductsQuery request, CancellationToken cancellationToken)
    {
        FilterShopProductDTO model = new FilterShopProductDTO()
        {
            ColorsId = request.ColorsId,
            BrandId = request.BrandId,
            MaxPrice = request.MaxPrice,
            MinPrice = request.MinPrice,
            ProductTitle = request.ProductTitle,
            shopCategories = request.ShopCategoryId,
        };

        return await _shopProductQueryRepository.FilterProducts(model, cancellationToken);
    }
}
