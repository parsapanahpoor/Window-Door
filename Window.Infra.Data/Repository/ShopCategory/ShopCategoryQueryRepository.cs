using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.State;
using Window.Domain.ViewModels.Site.Shop.Landing;

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
							.Where(a => !a.IsDelete && !a.ParentId.HasValue)
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

	#region Site Side

	public async Task<List<ShopCategoriesDTO>?> FillShopCategoriesDTO(CancellationToken cancellationToken)
	{
		return await _context.ShopCategories
							 .AsNoTracking()		
							 .Where(p=> !p.IsDelete)
							 .Select(p=> new ShopCategoriesDTO()
							 {
								 ShopCategoryId = p.Id,
								 ShopCategoryTitle = p.Title,
								 ParentId = p.ParentId,
							 })
							 .ToListAsync();
	}

    #endregion
}
