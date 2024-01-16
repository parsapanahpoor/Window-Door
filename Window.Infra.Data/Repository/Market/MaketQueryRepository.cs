using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.Market;
using Window.Domain.Interfaces.Maket;
using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Infra.Data.Repository.Maket;

public class MarketQueryRepository : QueryGenericRepository<Domain.Entities.Market.Market>, IMarketQueryRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public MarketQueryRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

    #endregion

    #region General 

    public async Task<Market?> GetMarketByMarketId(ulong marketId, CancellationToken token)
    {
        return await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketId);
    }

    public async Task<Market?> GetMarketByUserId(ulong userId , CancellationToken token)
    {
        #region Get Market

        var marketUser = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
        if (marketUser == null) return null;

        var market = await GetMarketByMarketId(marketUser.MarketId , token);
        if (market == null) return null;

        #endregion

        return market;
    }

    #endregion

    #region Admin 



    #endregion

    #region Site Side



    #endregion
}
