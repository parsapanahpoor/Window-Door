namespace Window.Domain.Interfaces.Location;

public interface ILocationCommandRepository
{
    Task AddAsync(Domain.Entities.Location.Location location, CancellationToken cancellationToken);

    void Update(Entities.Location.Location location);
}
