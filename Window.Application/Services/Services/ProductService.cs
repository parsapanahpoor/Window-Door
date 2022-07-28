using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Product;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Seller.Pricing;
using Window.Domain.ViewModels.Seller.Product;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Services
{
    public class ProductService : IProductService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public ProductService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Seller Side

        public async Task<bool> AddPriceForSelectedYaraghBrand(AddPriceForSelectedYaraghBrandViewModel model, ulong userId)
        {
            #region Get Product

            var product = await _context.Products.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.ProductId && p.UserId == userId);
            if (product == null) return false;

            #endregion

            #region Get Segment 

            var segment = await _context.Segments.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.SegmentId);
            if (segment == null) return false;

            #endregion

            #region Get Yaragh Brand 

            var mainBrand = await _context.YaraghBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.YaraghBrandId);
            if (mainBrand == null) return false;

            #endregion

            #region If Exist Selected Segment For This Product

            if (await _context.ProductYaraghBrandPrices.AnyAsync(p => !p.IsDelete && p.ProductId == model.ProductId && p.YaraghBrandId == model.YaraghBrandId && p.SegmentId == model.SegmentId))
            {
                return false;
            }

            #endregion

            #region Fill Entity

            ProductYaraghBrandPrice price = new ProductYaraghBrandPrice()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                YaraghBrandId = model.YaraghBrandId,
                ProductId = model.ProductId,
                SegmentId = model.SegmentId,
                Price = model.Price
            };

            #endregion

            #region Add Method

            await _context.ProductYaraghBrandPrices.AddAsync(price);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }


        public async Task<bool> AddPriceForSelectedMainBrand(AddPriceForSelectedMainBrandViewModel model, ulong userId)
        {
            #region Get Product

            var product = await _context.Products.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.ProductId && p.UserId == userId);
            if (product == null) return false;

            #endregion

            #region Get Segment 

            var segment = await _context.Segments.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.SegmentId);
            if (segment == null) return false;

            #endregion

            #region Get Main Brand 

            var mainBrand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.MainBrandId);
            if (mainBrand == null) return false;

            #endregion

            #region If Exist Selected Segment For This Product

            if (await _context.ProductMainBrandPrices.AnyAsync(p => !p.IsDelete && p.ProductId == model.ProductId && p.MainBrandId == model.MainBrandId && p.SegmentId == model.SegmentId))
            {
                return false;
            }

            #endregion

            #region Fill Entity

            ProductMainBrandPrice price = new ProductMainBrandPrice()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                MainBrandId = model.MainBrandId,
                ProductId = model.ProductId,
                SegmentId = model.SegmentId,
                Price = model.Price
            };

            #endregion

            #region Add Method

            await _context.ProductMainBrandPrices.AddAsync(price);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        public async Task<List<ProductMainBrandPrice>?> ListOfProductMainBrandPriceByProductId(ulong productId, ulong brandId)
        {
            return await _context.ProductMainBrandPrices.Include(p => p.Segment).Where(p => !p.IsDelete && p.MainBrandId == brandId && p.ProductId == productId).ToListAsync();
        }

        public async Task<List<ProductYaraghBrandPrice>?> ListOfProductYaraghBrandPriceByProductId(ulong productId, ulong brandId)
        {
            return await _context.ProductYaraghBrandPrices.Include(p => p.Segment).Where(p => !p.IsDelete && p.YaraghBrandId == brandId && p.ProductId == productId).ToListAsync();
        }

        public List<SelectListItem> GetSegmentsForAddProduct(ulong productId)
        {
            #region Get Product By Id 

            var product = _context.Products.FirstOrDefault(p => !p.IsDelete && p.Id == productId);
            if (product == null) return null;

            #endregion

            return _context.Segments.Where(p => !p.IsDelete)
                            .Select(p => new SelectListItem
                            {
                                Value = p.Id.ToString(),
                                Text = p.SegmentName
                            }).ToList();
        }

        public async Task<List<SelectListViewModel>> GetProductTypeCategories(ulong productTypeId)
        {
            if (productTypeId == 0)
            {
                return await _context.ProductTypeCategories.Where(p => !p.IsDelete && p.ProductType == Domain.Enums.Types.ProductType.Keshoie)
                                 .Select(p => new SelectListViewModel
                                 {
                                     Id = p.Id,
                                     Title = p.Name
                                 }).ToListAsync();

            }

            return await _context.ProductTypeCategories.Where(p => !p.IsDelete && p.ProductType == Domain.Enums.Types.ProductType.Lolaie)
                                .Select(p => new SelectListViewModel
                                {
                                    Id = p.Id,
                                    Title = p.Name
                                }).ToListAsync();
        }

        public async Task<List<SelectListViewModel>> LoadBrands(ulong sellerTypeId)
        {
            if (sellerTypeId == 0)
            {
                return await _context.MainBrands.Where(p => !p.IsDelete && p.UPVC)
                                 .Select(p => new SelectListViewModel
                                 {
                                     Id = p.Id,
                                     Title = p.BrandName
                                 }).ToListAsync();

            }

            return await _context.MainBrands.Where(p => !p.IsDelete && p.Alominum)
                                .Select(p => new SelectListViewModel
                                {
                                    Id = p.Id,
                                    Title = p.BrandName
                                }).ToListAsync();
        }

        public async Task<ulong> CreateProduct(CreateProductSellerSideViewModel model, ulong userId)
        {
            #region Get User By ID 

            var user = await _context.Users.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == userId);
            if (user == null) return 0;

            #endregion

            #region Get seller informations 

            var sellerInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (sellerInfo == null) return 0;

            #endregion

            #region Check User Activity 

            var marketState = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketState == null) return 0;
            if (marketState.MarketPersonalsInfoState != Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount) return 0;

            #endregion

            #region Check Valid Brand

            var brand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == model.BrandId);
            if (brand == null) return 0;

            #endregion

            #region Fill Model 

            Product product = new Product()
            {
                UserId = userId,
                CityId = sellerInfo.CityId.Value,
                StateId = sellerInfo.StateId.Value,
                CountryId = sellerInfo.CountryId.Value,
                CreateDate = DateTime.Now,
                IsDelete = false,
                ProductKind = model.ProductKind,
                ProductType = model.ProductType,
                SellerType = model.SellerType,
                ProductTypeCategoryId = model.ProductTypeCategory,
                MainBrandId = model.BrandId
            };

            #endregion

            #region Add Prodct

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            #endregion

            return product.Id;
        }

        public async Task<Product?> GetProductById(ulong productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == productId);
        }

        public async Task<List<MainBrand>> GetListOfMainBrandBaseOnSelectedProduct(Product product)
        {
            return await _context.MainBrands.Where(p => !p.IsDelete).ToListAsync();
        }

        public async Task<List<YaraghBrand>> GetListOfYaraghBrandBaseOnSelectedProduct(Product product)
        {
            return await _context.YaraghBrands.Where(p => !p.IsDelete && p.SellerType == product.SellerType).ToListAsync();
        }

        public async Task<FilterProductSellerSideViewModel> FilterProductSellerSideViewModel(FilterProductSellerSideViewModel filter)
        {
            var query = _context.Products
                .Include(p => p.User)
                .Include(p => p.MainBrand)
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter By Properties

            if (filter.UserId != null && filter.UserId != 0)
            {
                query = query.Where(p => p.UserId == filter.UserId);
            }

            #endregion

            await filter.Paging(query);

            return filter;

        }

        public async Task<bool> GetSellerTypeForValidAddProduct(ulong userId)
        {
            #region Get seller informations 

            var sellerInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (sellerInfo == null) return false;

            #endregion

            #region Get User Product Count 

            var count = await _context.Products.CountAsync(p => !p.IsDelete && p.UserId == userId);

            #endregion

            if (sellerInfo.SellerType == Domain.Enums.SellerType.SellerType.Aluminium)
            {
                if (count >= 2)
                {
                    return false;
                }
            }

            if (sellerInfo.SellerType == Domain.Enums.SellerType.SellerType.UPC)
            {
                if (count >= 3)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<SegmentPricingViewModel?> FillSegmentPricingViewModel(ulong productId, ulong userId)
        {
            #region Get Product 

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.UserId == userId && !p.IsDelete);
            if (product == null) return null;

            #endregion

            #region Get Segments

            var segments = _context.Segments.Where(p => !p.IsDelete).AsQueryable();

            #endregion

            #region Fill View Model

            SegmentPricingViewModel model = new SegmentPricingViewModel()
            {
                ProductId = productId,
                Segments = segments.Select(p => new SegmentPRicingEntityViewModel()
                {
                    Segment = p,
                }).ToList()
            };

            #endregion

            return model;
        }

        public async Task<GlassPricingViewModel?> FillGlassPricingEntityViewModel()
        {
            #region Fill View Model

            GlassPricingViewModel model = new GlassPricingViewModel()
            {
                Glass = await _context.Glasses.Where(p => !p.IsDelete).Select(p => new GlassPricingEntityViewModel()
                {
                    Glass = p
                }).ToListAsync()
            };

            #endregion

            return model;
        }

        public async Task<bool> AddPricingForSegment(ulong ProductId, ulong SegmentId, int Price, ulong userId)
        {
            #region Get Product 

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId && p.UserId == userId && !p.IsDelete);
            if (product == null) return false;

            #endregion

            #region Get Segment

            var segment = await _context.Segments.FirstOrDefaultAsync(p => p.Id == SegmentId && !p.IsDelete);
            if (segment == null) return false;

            #endregion

            #region Is Exist Any Same Segment Price

            var samePrice = await _context.SegmentPricings.AnyAsync(p => p.SegmentId == SegmentId && !p.IsDelete && p.ProductId == ProductId);

            #endregion

            //Update Pricing
            if (samePrice == true)
            {
                var lastSegmentPricing = await _context.SegmentPricings.FirstOrDefaultAsync(p => p.SegmentId == SegmentId && !p.IsDelete && p.ProductId == ProductId);

                lastSegmentPricing.Price = Price;

                _context.SegmentPricings.Update(lastSegmentPricing);
                await _context.SaveChangesAsync();
            }

            //Add New Pricing
            if (samePrice == false)
            {
                #region Fill Entity

                SegmentPricing model = new SegmentPricing()
                {
                    SegmentId = SegmentId,
                    ProductId = ProductId,
                    Price = Price
                };

                #endregion

                #region Add Segment Price

                await _context.SegmentPricings.AddAsync(model);
                await _context.SaveChangesAsync();

                #endregion
            }

            return true;
        }

        public async Task<bool> AddPricingForGlass(ulong GlassId, int Price, ulong userId)
        {
            #region Get Segment

            var glass = await _context.Glasses.FirstOrDefaultAsync(p => p.Id == GlassId && !p.IsDelete);
            if (glass == null) return false;

            #endregion

            #region Is Exist Any Same Glass Price

            var samePrice = await _context.GlassPricings.AnyAsync(p => p.GlassId == GlassId && !p.IsDelete);

            #endregion

            //Update Pricing
            if (samePrice == true)
            {
                var lastSegmentPricing = await _context.GlassPricings.FirstOrDefaultAsync(p => p.GlassId == GlassId && !p.IsDelete);

                lastSegmentPricing.Price = Price;

                _context.GlassPricings.Update(lastSegmentPricing);
                await _context.SaveChangesAsync();
            }

            //Add New Pricing
            if (samePrice == false)
            {
                #region Fill Entity

                GlassPricing model = new GlassPricing()
                {
                    GlassId = GlassId,
                    Price = Price,
                    UserId = userId
                };

                #endregion

                #region Add Glass Price

                await _context.GlassPricings.AddAsync(model);
                await _context.SaveChangesAsync();

                #endregion
            }

            return true;
        }


        public async Task<List<SegmentPricing>?> FillSegmentPricing(ulong productId, ulong userId)
        {
            #region Get Product 

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.UserId == userId && !p.IsDelete);
            if (product == null) return null;

            #endregion

            return await _context.SegmentPricings.Where(p => !p.IsDelete && p.ProductId == productId).ToListAsync();
        }

        public async Task<List<GlassPricing>?> FillGlassPricing(ulong userId)
        {
            return await _context.GlassPricings.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();
        }

        #endregion

        #region Site Side

        public async Task<FilterInquiryViewModel> FilterInquiryViewModel(FilterInquiryViewModel filter)
        {
            List<InquiryViewModel> model = new List<InquiryViewModel>();

            var product = _context.Products
                .Include(p => p.MainBrand)
                .Include(p=> p.User)
                .ThenInclude(p=> p.SellersPersonalInfos)
                .Where(p => !p.IsDelete)
                .Select(p=> new InquiryViewModel()
                {
                    User = p.User,
                    MainBrand = p.MainBrand, 
                })
                .AsQueryable();

            #region Filter

            if (filter.CountryId != null && filter.CountryId != 0)
            {
                product = product.Where(p => p.User.SellersPersonalInfos.CountryId == filter.CountryId);
            }

            if (filter.StateId != null && filter.StateId != 0)
            {
                product = product.Where(p => p.User.SellersPersonalInfos.StateId == filter.StateId);
            }

            if (filter.CityId != null && filter.CityId != 0)
            {
                product = product.Where(p => p.User.SellersPersonalInfos.CityId == filter.CityId);
            }

            if (filter.MainBrandId != null && filter.MainBrandId != 0)
            {
                product = product.Where(p => p.MainBrand.Id == filter.MainBrandId);
            }

            if (filter.SellerType != null)
            {
                if (filter.SellerType == Domain.Enums.SellerType.SellerType.UPC)
                {
                    product = product.Where(p => p.User.SellersPersonalInfos.SellerType == Domain.Enums.SellerType.SellerType.UPC);
                }
                if (filter.SellerType == Domain.Enums.SellerType.SellerType.Aluminium)
                {
                    product = product.Where(p => p.User.SellersPersonalInfos.SellerType == Domain.Enums.SellerType.SellerType.Aluminium);
                }
            }

            #endregion

            await filter.Paging(product);

            return filter;

        }

        #endregion
    }
}