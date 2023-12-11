using Window.Data.Context;

namespace Window.Application.Common.UnitOfWork;

public partial class UnitOfWork : Window.Application.Common.IUnitOfWork.IUnitOfWork
{
    #region Using

    private readonly WindowDbContext _context;

    public UnitOfWork(WindowDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Save Changes

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    #endregion
}
