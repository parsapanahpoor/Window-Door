using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopProduct;
namespace Window.Infra.Data.Repository.ShopProduct;

public class ShopProductQueryRepository : QueryGenericRepository<Domain.Entities.ShopCategory>, IShopProductQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public ShopProductQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}
