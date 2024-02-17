using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.SellerDetail;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

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

    #region Site Side 

    //List Of Products
    public async Task<FilterShopProductDTO> FilterProducts(FilterShopProductDTO model, CancellationToken cancellation)
    {
        var query = _context.ShopProducts
            .Include(p => p.User)
            .Where(s => !s.IsDelete)
            .OrderByDescending(s => s.CreateDate)
            .AsQueryable();

        #region Filter

        //Color
        if (model.ColorsId != null && model.ColorsId.Any())
        {
            query = query.Where(p => model.ColorsId.Contains(p.ProductColorId));
        }

        //Brand
        if (model.BrandId != null && model.BrandId.Any())
        {
            query = query.Where(p => model.BrandId.Contains(p.ProductBrandId));
        }

        //Title
        if (!string.IsNullOrEmpty(model.ProductTitle))
        {
            query = query.Where(p => p.ProductName.Contains(model.ProductTitle) || p.User.Username.Contains(model.ProductTitle));
        }

        //ShopCategory
        if (model.shopCategories != null && model.shopCategories.Any())
        {
            var categoryProducts = _context.ShopProductSelectedCategories
                            .AsNoTracking()
                            .Include(p => p.ShopProduct)
                            .Where(p => !p.IsDelete && model.shopCategories.Contains(p.ShopCategoryId))
                            .Select(p => p.ShopProduct)
                            .Distinct()
                            .AsQueryable();

            query = from q in query
                    join c in categoryProducts
                    on q.Id equals c.Id
                    select new Domain.Entities.ShopProduct.ShopProduct
                    {
                        Id = q.Id,
                        CreateDate = q.CreateDate,
                        IsDelete = q.IsDelete,
                        Price = q.Price,
                        ProductName = q.ProductName,
                        LongDescription = q.LongDescription,
                        ProductBrandId = q.ProductBrandId,
                        ProductColorId = q.ProductColorId,
                        ProductImage = q.ProductImage,
                        SellerUserId = q.SellerUserId,
                        ShortDescription = q.ShortDescription,
                        User = q.User,
                    };
        }

        #endregion

        #region Price

        if (model.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= model.MinPrice.Value);
        }

        if (model.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= model.MaxPrice.Value);
        }

        #endregion

        await model.Paging(query.Distinct());

        return model;
    }

    public async Task<List<ShopCard>> FillShopCard(ulong sellerUserId, CancellationToken cancellation)
    {
        return await _context.ShopProducts
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.SellerUserId == sellerUserId)
                             .Select(p => new ShopCard()
                             {
                                 BrandName = _context.MainBrands
                                                     .AsNoTracking()
                                                     .Where(s => !s.IsDelete &&
                                                            s.Id == p.ProductBrandId)
                                                     .Select(s => s.BrandName)
                                                     .FirstOrDefault(),
                                 ColorName = _context.ShopColors
                                                     .AsNoTracking()
                                                     .Where(s => !s.IsDelete &&
                                                            s.Id == p.ProductColorId)
                                                     .Select(s => s.ColorTitle)
                                                     .FirstOrDefault(),
                                 Price = p.Price,
                                 ShopProductName = p.ProductName,
                                 ProductId = p.Id,
                                 ProductImage = p.ProductImage,
                             })
                             .ToListAsync();
    }

    public async Task<List<LastestShopProducts>> FillLastestShopProducts(CancellationToken cancellation)
    {
        return await _context.ShopProducts
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .OrderByDescending(p => p.CreateDate)
                             .Select(p => new LastestShopProducts()
                             {
                                 ProductPrice = p.Price,
                                 ProductTitle = p.ProductName,
                                 ProductId = p.Id,
                                 ProductImage = p.ProductImage,
                                 SellerInfo = _context.Users
                                                      .AsNoTracking()
                                                      .Where(s => !s.IsDelete &&
                                                             s.Id == p.SellerUserId)
                                                      .Select(s => new SellerInfo()
                                                      {
                                                          SellerId = s.Id,
                                                          SellerUsername = s.Username
                                                      })
                                                      .FirstOrDefault()
                             })
                             .Take(10)
                             .ToListAsync();
    }

    public async Task<List<LastestSellers>> ListOfLastestSellers(CancellationToken cancellation)
    {
        List<LastestSellers> model = new List<LastestSellers>();

        var products = await _context.ShopProducts
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .OrderByDescending(p => p.CreateDate)
                             .GroupBy(p => p.SellerUserId)
                             .ToListAsync();

        foreach (var product in products)
        {
            var user = await _context.Users
                                     .AsNoTracking()
                                     .Where(p => !p.IsDelete &&
                                            p.Id == product.FirstOrDefault().SellerUserId)
                                     .FirstOrDefaultAsync();

            model.Add(new LastestSellers()
            {
                CreateDate = user.CreateDate,
                ProductCount = product.Count(),
                UserAvatar = user.Avatar,
                Username = user.Username,
                UserSellerId = user.Id,
            });
        }

        return model;
    }

    public async Task<List<LastestBrands>> LastestMainBrands(CancellationToken cancellationToken)
    {
        return await _context.MainBrands
                                             .AsNoTracking()
                                             .Where(p => !p.IsDelete && 
                                                    p.ShowInSiteMenue)
                                             .OrderByDescending(p => p.Priority)
                                             .Take(12)
                                             .Select(p=> new LastestBrands()
                                             {
                                                 BrandId = p.Id,
                                                 BrandTitle = p.BrandName,
                                                 Image = p.BrandLogo,
                                                 ProductCounts = _context.ShopProducts
                                                                         .AsNoTracking()
                                                                         .Where(s=> !s.IsDelete &&
                                                                                s.ProductBrandId == p.Id)
                                                                         .Count()
                                             })
                                             .ToListAsync();
    }

    #endregion

    #region Seller Side 

    public async Task<List<ProductTag>> GetListOfProductTagsByProductId(ulong productId, CancellationToken cancellation)
    {
        return await _context.ProductTags
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.ProductId == productId)
                             .ToListAsync();
    }

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

    public async Task<List<ulong>> GetShopProductSelectedCategories(ulong productId, CancellationToken token)
    {
        return await _context.ShopProductSelectedCategories
                             .AsNoTracking()
                             .Where(p => p.ShopProductId == productId)
                             .Select(p => p.ShopCategoryId)
                             .ToListAsync();
    }

    public async Task<List<ShopProductSelectedCategories>?> GetListOf_ShopProductSelectedCategories_ByProductId(ulong productId,
                                                                                                                CancellationToken cancellation)
    {
        return await _context.ShopProductSelectedCategories
                             .Where(p => !p.IsDelete && p.ShopProductId == productId)
                             .ToListAsync();
    }

    #endregion
}
