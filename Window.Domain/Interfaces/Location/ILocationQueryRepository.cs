using Window.Domain.ViewModels.Site.Shop.Location;

namespace Window.Domain.Interfaces.Location;

public interface ILocationQueryRepository
{
    #region Site Side 

    Task<List<ListOfUserLocation>> FillListOfUserLocation(ulong userId, CancellationToken cancellation);

    Task<Domain.Entities.Location.Location> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion
}
