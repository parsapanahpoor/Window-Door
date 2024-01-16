using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopProductFeature;
using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Infra.Data.Repository.ShopProductFeature;

public class ShopProductFeatureQueryRepository : QueryGenericRepository<Domain.Entities.ShopProductFeature.ShopProductFeature>, IShopProductFeatureQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public ShopProductFeatureQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Admin 



    #endregion

    #region Site Side

    public async Task<bool> IsExistBrandById(ulong brandId , CancellationToken cancellation)
    {
        return await _context.ShopProductFeature
                             .AnyAsync(p=> !p.IsDelete && p.Id == brandId);
    }

    #endregion
}
