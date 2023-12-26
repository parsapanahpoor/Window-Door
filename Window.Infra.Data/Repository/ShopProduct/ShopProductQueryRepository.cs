﻿using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Infra.Data.Repository.ShopProduct;

public class ShopProductQueryRepository : QueryGenericRepository<Domain.Entities.ShopProduct.ShopProduct>, IShopProductQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public ShopProductQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Seller Side 

    public async Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation)
    {
        var query = _context.ShopProducts
                            .AsNoTracking()
                            .Where(a => !a.IsDelete && 
                                   a.SellerUserId == filter.SellerUserId)
                            .OrderBy(s => s.CreateDate)
                            .AsQueryable();

        #region Filter

        if (!string.IsNullOrEmpty(filter.ProductName))
        {
            query = query.Where(s => EF.Functions.Like(s.ProductName, $"%{filter.ProductName}%"));
        }

        #endregion

        await filter.Paging(query);

        return filter;
    }

    #endregion
}