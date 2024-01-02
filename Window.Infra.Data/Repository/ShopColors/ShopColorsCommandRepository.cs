using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopColors;
using Window.Domain.Interfaces.ShopColors;
namespace Window.Infra.Data.Repository.ShopColors;

public class ShopColorsCommandRepository : CommandGenericRepository<Domain.Entities.ShopColors.ShopColor>, IShopColorsCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopColorsCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

    #endregion

    public async Task AddShopProductSelectedColorAsync(ShopProductsSelectedColors selectedColors, CancellationToken cancellationToken)
    {
		await _context.ShopProductsSelectedColors.AddAsync(selectedColors) ;
    }
}
