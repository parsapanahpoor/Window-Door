using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.State;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

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

    #region General Methods

    public async Task<List<SelectListViewModel>> GetAllMainShopCategoriesCategories(CancellationToken cancellationToken)
    {
        return await _context.ShopCategories
                             .AsNoTracking()
                             .Where(s => s.ParentId == null &&
                                    !s.IsDelete)
                             .OrderBy(p => p.Priority)
                             .Select(s => new SelectListViewModel
                             {
                                 Id = s.Id,
                                 Title = s.Title
                             })
                             .ToListAsync();
    }

    public async Task<List<SelectListViewModel>> GetCategoriesChildrent(ulong parentId, CancellationToken cancellationToken)
    {
        return await _context.ShopCategories
                             .AsNoTracking()
                             .Where(s => !s.IsDelete &&
                                    s.ParentId.HasValue &&
                                    s.ParentId.Value == parentId)
                             .OrderBy(p => p.Priority)
                             .Select(s => new SelectListViewModel
                             {
                                 Id = s.Id,
                                 Title = s.Title
                             })
                             .ToListAsync();
    }

    #endregion

    #region Admin 

    public async Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter)
    {
        var query = _context.ShopCategories
                            .AsNoTracking()
                            .Where(a => !a.IsDelete && !a.ParentId.HasValue)
                            .OrderBy(s => s.Priority)
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

    public async Task<List<ShopCategoriesDTO>?> FillLargSideShopCategoriesDTO(CancellationToken cancellationToken)
    {
        return await _context.ShopCategories
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.ShowOnSiteLanding)
                             .Select(p => new ShopCategoriesDTO()
                             {
                                 ShopCategoryId = p.Id,
                                 ShopCategoryTitle = p.Title,
                                 ParentId = p.ParentId,
                                 Priority = p.Priority,
                                 ShowOnSiteLanding = p.ShowOnSiteLanding,
                             })
                             .ToListAsync();
    }

    public async Task<List<ShopCategoriesDTO>?> FillShopCategoriesDTO(CancellationToken cancellationToken)
    {
        return await _context.ShopCategories
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .Select(p => new ShopCategoriesDTO()
                             {
                                 ShopCategoryId = p.Id,
                                 ShopCategoryTitle = p.Title,
                                 ParentId = p.ParentId,
                                 Priority = p.Priority,
                                 ShowOnSiteLanding = p.ShowOnSiteLanding,
                             })
                             .ToListAsync();
    }

    public async Task<List<ShopCategoriesForShowInFilterShopProduct>> FillShopCategoriesForShowInFilterShopProduct(ulong shopCategoryParentId, CancellationToken cancellationToken)
    {
        return await _context.ShopCategories
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.ParentId == shopCategoryParentId)
                             .OrderBy(p => p.Priority)
                             .Select(p => new ShopCategoriesForShowInFilterShopProduct()
                             {
                                 ShopCategoryId = p.Id,
                                 ShopCategoryTitle = p.Title,
                             })
                             .ToListAsync();
    }

    public async Task<string?> GetShopCategoryTitle(ulong shopCategoryId, CancellationToken cancellationToken)
    {
        return await _context.ShopCategories
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == shopCategoryId)
                             .Select(p => p.Title)
                             .FirstOrDefaultAsync();
    }

    #endregion
}
