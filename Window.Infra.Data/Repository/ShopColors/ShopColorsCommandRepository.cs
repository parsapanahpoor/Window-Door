using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopColors;
namespace Window.Infra.Data.Repository.ShopColors;

public class ShopColorsCommandRepository : CommandGenericRepository<Domain.Entities.ShopCategory>, IShopColorsCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopColorsCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion
}
