using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.State;

namespace Window.Infra.Data.Repository.ShopCategory;

public class ShopCategoryQueryRepository : QueryGenericRepository<Domain.Entities.ShopCategory>, IShopCategoryQueryRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopCategoryQueryRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

	#endregion

	#region Admin 

	public async Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter)
	{
		var query = _context.ShopCategories
							.AsNoTracking()
							.Where(a => !a.IsDelete)
							.OrderByDescending(s => s.CreateDate)
							.AsQueryable();

		if (filter.ParentId.HasValue)
		{
             query = _context.ShopCategories
                            .AsNoTracking()
                            .Where(a => !a.IsDelete && a.ParentId == filter.ParentId)
                            .OrderByDescending(s => s.CreateDate)
                            .AsQueryable();
        }

		#region Filter

		if (!string.IsNullOrEmpty(filter.Title))
		{
			query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));
		}

		#endregion

		await filter.Paging(query);

		return filter;
	}

	#endregion
}
