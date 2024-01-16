using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopProductFeature;
using Window.Domain.Interfaces.ShopProductFeature;
namespace Window.Infra.Data.Repository.ShopProductFeature;

public class ShopProductFeatureCommandRepository : CommandGenericRepository<Domain.Entities.ShopProductFeature.ShopProductFeature> , IShopProductFeatureCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopProductFeatureCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

    #endregion
}
