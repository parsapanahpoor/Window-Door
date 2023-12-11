#region Using


using Microsoft.EntityFrameworkCore;
using Window.Data.Context;

namespace Window.Data;

#endregion

public class CommandGenericRepository<TEntity> where TEntity : class
{
    #region Ctor

    protected readonly WindowDbContext _context;

    public DbSet<TEntity> Entities { get; }

    public CommandGenericRepository(WindowDbContext context)
    {
        _context = context;
        Entities = _context.Set<TEntity>() ?? throw new ArgumentNullException(nameof(TEntity));
        _context.SaveChangesAsync();
    }

    #endregion

    #region Async Methods

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Entities.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await Entities.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        Entities.Add(entity);
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        Entities.AddRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
        _context.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _context.Remove(entity);
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Remove(entities);
    }

    #endregion
}
