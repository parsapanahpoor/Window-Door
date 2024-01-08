using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

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

    #region Site Side

    public async Task<List<ListOfColorsForFilterProductsDTO>> FillListOfColorsForFilterProductsDTO(CancellationToken cancellationToken)
    {
        return await _context.ShopColors
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .OrderBy(s => s.Priority)
                             .Select(p => new ListOfColorsForFilterProductsDTO()
                             {
                                 ColorTitle = p.ColorTitle,
                                 Id = p.Id,
								 ColorCode = p.ColorCode,
                             })
                            .ToListAsync();
    }

	public async Task<bool> IsExistColorById(ulong colorId, CancellationToken cancellation)
	{
		return await _context.ShopColors
							 .AnyAsync(p => !p.IsDelete && p.Id == colorId);
	}

    #endregion
}
