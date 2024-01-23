using Microsoft.EntityFrameworkCore;
using Window.Data.Context;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Data;

public class QueryGenericRepository<TEntity> where TEntity : class
{
    #region Ctor

    protected readonly WindowDbContext _context;

    public DbSet<TEntity> Entities { get; }

    public QueryGenericRepository(WindowDbContext context)
    {
        _context = context;
        Entities = _context.Set<TEntity>() ?? throw new ArgumentNullException(nameof(TEntity));
        _context.SaveChangesAsync();
    }

    #endregion

    #region async Method

    public virtual async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return await Entities.FindAsync(ids, cancellationToken);
    }

    #endregion

    #region sync Method

    public virtual IQueryable<TEntity> Table => Entities;

    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    public virtual TEntity GetById(params object[] ids)
    {
        return Entities.Find(ids);
    }

    #endregion
}
