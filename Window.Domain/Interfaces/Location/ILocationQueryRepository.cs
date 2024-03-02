using Window.Domain.ViewModels.Site.Shop.Location;

namespace Window.Domain.Interfaces.Location;

public interface ILocationQueryRepository
{
    #region Site Side 

    Task<List<ListOfUserLocation>> FillListOfUserLocation(ulong userId, CancellationToken cancellation);

    #endregion
}
