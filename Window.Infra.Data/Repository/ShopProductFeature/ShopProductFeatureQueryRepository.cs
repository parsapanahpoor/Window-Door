using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopProductFeature;
using Window.Domain.ViewModels.Seller.ShopProduct;

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

    #region Seller Side 

    public async Task<List<ProductFeaturesDTO>?> FillProductFeaturesDTO(ulong productId, CancellationToken token)
    {
        return await _context.ShopProductFeature
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.ProductId == productId)
                             .Select(p => new ProductFeaturesDTO()
                             {
                                 ProductId = productId,
                                 Id = p.Id,
                                 Title = p.FeatureTitle,
                                 Value = p.FeatureValue
                             })
                             .ToListAsync();
    }

    #endregion

    #region Site Side

    public async Task<bool> IsExistBrandById(ulong brandId , CancellationToken cancellation)
    {
        return await _context.ShopProductFeature
                             .AnyAsync(p=> !p.IsDelete && p.Id == brandId);
    }

    #endregion
}
