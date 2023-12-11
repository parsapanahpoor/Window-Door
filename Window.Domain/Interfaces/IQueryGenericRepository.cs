namespace Window.Domain.Interfaces;

public interface IQueryGenericRepository<TEntity> where TEntity : class
{
    TEntity GetById(params object[] ids);

    Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);
}
