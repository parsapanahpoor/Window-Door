using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.Maket;
namespace Window.Infra.Data.Repository.Maket;

public class MarketCommandRepository : CommandGenericRepository<Domain.Entities.Market.Market>, IMarketCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public MarketCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

    #endregion
}
