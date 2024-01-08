using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Infra.Data.Repository.ShopProduct;

public class ShopProductCommandRepository : CommandGenericRepository<Domain.Entities.ShopProduct.ShopProduct>, IShopProductCommandRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public ShopProductCommandRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region General Methods 

    public void DeleteRange(IEnumerable<ProductTag> productTags)
    {
        _context.ProductTags.RemoveRange(productTags);
    }

    public void DeleteRangeProductSelectedCategories(IEnumerable<ShopProductSelectedCategories> productSelectedCategories)
    {
        _context.ShopProductSelectedCategories.RemoveRange(productSelectedCategories);
    }

    public async Task AddShopTagAsync(ProductTag tag, CancellationToken cancellationToken)
    {
        await _context.ProductTags.AddAsync(tag);
    }


    #endregion
}
