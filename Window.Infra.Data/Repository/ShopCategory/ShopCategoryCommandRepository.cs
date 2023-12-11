using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities;
using Window.Domain.Interfaces.ShopCategory;

namespace Window.Infra.Data.Repository.ShopCategory;

public class ShopCategoryCommandRepository : CommandGenericRepository<Domain.Entities.ShopCategory>,  IShopCategoryCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopCategoryCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion
}
