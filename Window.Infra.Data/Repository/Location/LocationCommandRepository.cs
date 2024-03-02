using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.Location;
namespace Window.Infra.Data.Repository.Location;

public class LocationCommandRepository : CommandGenericRepository<Domain.Entities.Location.Location> , ILocationCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

    public LocationCommandRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}
