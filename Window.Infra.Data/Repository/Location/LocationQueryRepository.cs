using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.Location;
using Window.Domain.ViewModels.Site.Shop.Location;

namespace Window.Infra.Data.Repository.Location;

public class LocationQueryRepository : QueryGenericRepository<Domain.Entities.Location.Location> , ILocationQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public LocationQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Site Side 

    public async Task<List<ListOfUserLocation>> FillListOfUserLocation(ulong userId, CancellationToken cancellation)
    {
        return await _context.Locations
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete && 
                                    p.UserId == userId)
                             .OrderByDescending(p=> p.CreateDate)
                             .Select(p=> new ListOfUserLocation()
                             {
                                 City = p.City,
                                 LocationAddress = p.Address,
                                 LocationId = p.Id,
                                 Mobile = p.Mobile,
                                 PostalCode = p.PostalCode,
                                 State = p.State,
                                 Username = p.FirstName + p.LastName,
                             })
                             .ToListAsync();
    }

    #endregion
}
