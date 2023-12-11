namespace Window.Domain.Interfaces;

public interface ICommandGenericRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    void AddRange(IEnumerable<TEntity> entities);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
}
