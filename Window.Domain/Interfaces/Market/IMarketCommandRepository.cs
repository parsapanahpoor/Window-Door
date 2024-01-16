namespace Window.Domain.Interfaces.Maket;

public interface IMarketCommandRepository
{
	#region Admin Side 

	Task AddAsync(Domain.Entities.Market.Market market, CancellationToken cancellationToken);

	void Update(Domain.Entities.Market.Market market);

	#endregion
}
