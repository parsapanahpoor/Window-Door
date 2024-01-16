using Window.Domain.Entities.Market;

namespace Window.Domain.Interfaces.Maket;

public interface IMarketQueryRepository
{
    #region Methods 

    Task<Market?> GetMarketByMarketId(ulong marketId, CancellationToken token);

    Task<Market?> GetMarketByUserId(ulong userId, CancellationToken token);

    #endregion

    #region Admin 

    Task<Domain.Entities.Market.Market> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side

    #endregion
}
