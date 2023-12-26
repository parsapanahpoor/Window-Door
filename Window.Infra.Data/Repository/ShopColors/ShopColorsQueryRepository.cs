using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopColors;
namespace Window.Infra.Data.Repository.ShopColors;

public class ShopColorsQueryRepository : QueryGenericRepository<Domain.Entities.ShopBrands.ShopBrand>, IShopColorsQueryRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopColorsQueryRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion
}
