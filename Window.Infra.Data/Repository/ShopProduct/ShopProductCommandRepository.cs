using Window.Data;
using Window.Data.Context;
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
}
