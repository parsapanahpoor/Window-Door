﻿using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopBrands;
using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Infra.Data.Repository.ShopBrands;

public class ShopBrandsQueryRepository : QueryGenericRepository<Domain.Entities.ShopBrands.ShopBrand>, IShopBrandsQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public ShopBrandsQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Admin 

    public async Task<FilterShopBrandDTO> FilterShopBrand(FilterShopBrandDTO filter, CancellationToken cancellation)
    {
        var query = _context.ShopBrands
                            .AsNoTracking()
                            .Where(a => !a.IsDelete)
                            .OrderBy(s => s.Priority)
                            .AsQueryable();

        #region Filter

        if (!string.IsNullOrEmpty(filter.Title))
        {
            query = query.Where(s => EF.Functions.Like(s.ShopBrandTitle, $"%{filter.Title}%"));
        }

        #endregion

        await filter.Paging(query);

        return filter;
    }

    #endregion

    #region Site Side

    public async Task<List<ListOfBrandsForFilterProductsDTO>> FillListOfBrandsForFilterProductsDTO(CancellationToken cancellationToken)
    {
        return await _context.ShopBrands
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .OrderBy(s => s.Priority)
                             .Select(p => new ListOfBrandsForFilterProductsDTO()
                             {
                                 BrandTitle = p.ShopBrandTitle,
                                 Id = p.Id,
                             })
                            .ToListAsync();
    }

    #endregion
}