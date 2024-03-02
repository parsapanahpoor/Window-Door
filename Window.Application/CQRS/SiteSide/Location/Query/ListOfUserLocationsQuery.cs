using Window.Domain.ViewModels.Site.Shop.Location;

namespace Window.Application.CQRS.SiteSide.Location.Query;

public record ListOfUserLocationsQuery : IRequest<List<ListOfUserLocation>>
{
    #region properties

    public ulong UserId { get; set; }

    #endregion
}
