using Window.Domain.Interfaces.Location;
using Window.Domain.ViewModels.Site.Shop.Location;

namespace Window.Application.CQRS.SiteSide.Location.Query;

public record ListOfUserLocationsQueryHandler : IRequestHandler<ListOfUserLocationsQuery, List<ListOfUserLocation>>
{
    #region Ctor 

    private readonly ILocationQueryRepository _locationQueryRepository;

    public ListOfUserLocationsQueryHandler(ILocationQueryRepository locationQueryRepository)
    {
        _locationQueryRepository = locationQueryRepository;
    }

    #endregion

    public async Task<List<ListOfUserLocation>> Handle(ListOfUserLocationsQuery request, CancellationToken cancellationToken)
    {
        return await _locationQueryRepository.FillListOfUserLocation(request.UserId , cancellationToken);
    }
}
