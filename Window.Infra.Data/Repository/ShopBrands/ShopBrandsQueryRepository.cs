using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopBrands;
namespace Window.Infra.Data.Repository.ShopBrands;

public class ShopBrandsQueryRepository : QueryGenericRepository<Domain.Entities.ShopBrands.ShopBrand>, IShopBrandsQueryRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopBrandsQueryRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion
}
