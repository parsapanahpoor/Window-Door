using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopColor;

namespace Window.Infra.Data.Repository.ShopColors;

public class ShopColorsQueryRepository : QueryGenericRepository<Domain.Entities.ShopColors.ShopColor>, IShopColorsQueryRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopColorsQueryRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion

	#region Admin 

	public async Task<FilterShopColorDTO> FilterShopColor(FilterShopColorDTO filter , CancellationToken cancellation)
	{
		var query = _context.ShopColors
							.AsNoTracking()
							.Where(a => !a.IsDelete )
							.OrderBy(s => s.Priority)
							.AsQueryable();

		#region Filter

		if (!string.IsNullOrEmpty(filter.Title))
		{
			query = query.Where(s => EF.Functions.Like(s.ColorTitle, $"%{filter.Title}%"));
		}

		#endregion

		await filter.Paging(query);

		return filter;
	}

	#endregion
}
