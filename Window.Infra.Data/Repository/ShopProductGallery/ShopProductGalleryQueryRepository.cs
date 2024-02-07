using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopProductGallery;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Infra.Data.Repository.ShopProductGallery;

public class ShopProductGalleryQueryRepository : QueryGenericRepository<Domain.Entities.ShopProductGallery.ShopProductGallery>, IShopProductGalleryQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public ShopProductGalleryQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Admin 



    #endregion

    #region Seller Side 

    public async Task<List<ProductGalleriesDTO>?> FillProductGalleriesDTO(ulong productId , CancellationToken token)
    {
        return await _context.ShopProductGalleries
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.ProductId == productId)
                             .Select(p => new ProductGalleriesDTO()
                             {
                                 ImageName = p.ImageName,
                                 ProductGalleryId = productId,
                                 Id = p.Id
                             })
                             .ToListAsync();
    }

    #endregion

    #region Site Side

    public async Task<bool> IsExistBrandById(ulong brandId , CancellationToken cancellation)
    {
        return await _context.ShopProductGalleries
                             .AnyAsync(p=> !p.IsDelete && p.Id == brandId);
    }

    public async Task<List<string?>> GetProductGalleriesImages(ulong productId , CancellationToken cancellation)
    {
        return await _context.ShopProductGalleries
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.ProductId == productId)
                             .Select(p=> p.ImageName)
                             .ToListAsync();
    }

    #endregion
}
