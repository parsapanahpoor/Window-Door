using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopBrands;
namespace Window.Infra.Data.Repository.ShopBrands;

public class ShopBrandsCommandRepository : CommandGenericRepository<Domain.Entities.ShopBrands.ShopBrand> , IShopBrandsCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopBrandsCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion
}
