using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Admin.ShopProduct;
using Window.Infra.Data.Repository.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Query.FilterShopProducts;

public record FilterShopProductsAdminSideQueryHandler : IRequestHandler<FilterShopProductsAdminSideQuery, FilterShopProductsAdminSideDTO>
{
    #region Ctor 

    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public FilterShopProductsAdminSideQueryHandler(IShopProductQueryRepository shopProductQueryRepository)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<FilterShopProductsAdminSideDTO> Handle(FilterShopProductsAdminSideQuery request, CancellationToken cancellationToken)
    {
        return await _shopProductQueryRepository.FilterShopProductAdminSide(request.filter , cancellationToken);
    }
}
