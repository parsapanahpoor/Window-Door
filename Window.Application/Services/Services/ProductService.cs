using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.Sample;
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

        //Get All Glasses
        public async Task<List<SelectListViewModel>> GetAllGlasses()
        {
            return await _context.Glasses.Where(s => !s.IsDelete)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.GlassName
                }).ToListAsync();
        }

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

            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return 0;

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
                UserId = market.UserId,
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

            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == filter.UserId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion

            #region Filter By Properties

            if (market.UserId != null && market.UserId != 0)
            {
                query = query.Where(p => p.UserId == market.UserId);
            }

            #endregion

            await filter.Paging(query);

            return filter;

        }

        public async Task<bool> GetSellerTypeForValidAddProduct(ulong userId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            #region Get seller informations 

            var sellerInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == market.UserId);
            if (sellerInfo == null) return false;

            #endregion

            #region Get User Product Count 

            var count = await _context.Products.CountAsync(p => !p.IsDelete && p.UserId == market.UserId);

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
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion

            #region Get Product 

            var product = await _context.Products.Include(p=> p.MainBrand).FirstOrDefaultAsync(p => p.Id == productId && p.UserId == market.UserId && !p.IsDelete);
            if (product == null) return null;

            #endregion

            #region Get Segments

            var segments = _context.Segments.Where(p => !p.IsDelete).AsQueryable();

            #endregion

            #region Fill View Model

            SegmentPricingViewModel model = new SegmentPricingViewModel()
            {
                ProductId = productId,
                ProductName = product.MainBrand.BrandName,
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
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            #region Get Product 

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId && p.UserId == market.UserId && !p.IsDelete);
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
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

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
                    UserId = market.UserId
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
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion

            #region Get Product 

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.UserId == market.UserId && !p.IsDelete);
            if (product == null) return null;

            #endregion

            return await _context.SegmentPricings.Where(p => !p.IsDelete && p.ProductId == productId).ToListAsync();
        }

        public async Task<List<GlassPricing>?> FillGlassPricing(ulong userId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion

            return await _context.GlassPricings.Where(p => !p.IsDelete && p.UserId == market.UserId).ToListAsync();
        }

        #endregion

        #region Site Side

        public async Task<List<Sample?>> GetSamplesById(List<ulong?> samplesId)
        {
            List<Sample> sampleList = new List<Sample>();

            if (samplesId != null && samplesId.Any())
            {
                foreach (var item in samplesId)
                {
                    var sample = await _context.Samples.Include(p => p.SampleSelectedSegment).FirstOrDefaultAsync(p => !p.IsDelete && p.Id == item);
                    sampleList.Add(sample);
                }
            }

            return sampleList;
        }

        public async Task<int?> InitializeSamplesPrice(List<Sample?> samples, User user, int height, int width)
        {
            var model = 0;

            var samplesize = 2 * (height + width);

            #region Get User Segments Price 

            var segments = await _context.SegmentPricings.Include(p => p.Product).Where(p => !p.IsDelete && p.Product.UserId == user.Id).ToListAsync();
            if (segments == null) return null;

            #endregion

            #region pricing 

            if (samples == null)
            {
                return null;
            }
            else
            {
                foreach (var item in samples)
                {
                    var sampleSegments = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == item.Id).Select(p => p.Segment).ToListAsync();

                    foreach (var seg in segments)
                    {
                        if (sampleSegments.Any(p => p.Id == seg.Segment.Id))
                        {
                            model = model + (seg.Price * samplesize);
                        }
                    }
                }
            }

            return model;

            #endregion
        }

        public async Task<int?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int height, int width, ulong userId)
        {
            #region return Model 

            var totalPrice = 0;

            #endregion

            #region Get User Segments Price 

            var userSegments = await _context.SegmentPricings.Include(p => p.Product).Where(p => !p.IsDelete && p.Product.UserId == userId && p.Product.MainBrandId == brandId).ToListAsync();
            if (userSegments == null) return null;

            #endregion

            #region pricing & Get Samples

            #region Get Sample By Id 

            var sample = await _context.Samples.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sampleId);
            if (sample == null) return null;

            #endregion

            #region پنجره لولایی آلومینیومی فیکس ساده

            if (sample.Id == 8)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی دریچه ساده

            if (sample.Id == 9)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 3) != null)
                {
                    //قیمت لنگه
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 3).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 4) != null)
                {
                    //یراق ملغی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 4).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  تک لنگه ساده

            if (sample.Id == 10)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 3) != null)
                {
                    //قیمت لنگه
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 3).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  دولنگه ساده

            if (sample.Id == 11)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (width + ( 2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی سه لنگه ساده

            if (sample.Id == 12)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (((2 * width) / 3)+ (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + ((2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  چهار لنگه ساده

            if (sample.Id == 13)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (width + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + ((3 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + ((3 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  شش لنگه ساده

            if (sample.Id == 14)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (((2 * width) / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (5 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (5 * ((height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی فیکس 

            if (sample.Id == 15)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //قیمت گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی دریچه

            if (sample.Id == 16)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 3) != null)
                {
                    //قیمت لنگه
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 3).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //گالوانیزه ی لنگه
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 4) != null)
                {
                    //یراق ملغی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 4).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی تک لنگه

            if (sample.Id == 17)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 3) != null)
                {
                    //قیمت لنگه
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 3).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //گالوانیزه ی لنگه
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  دولنگه

            if (sample.Id == 18)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //گالوانیزه ی لنگه
                    totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
                {
                    //گالوانیزه ی مولیون
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی سه لنگه

            if (sample.Id == 19)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (((width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //گالوانیزه ی لنگه
                    totalPrice = totalPrice + (((width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + ((2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
                {
                    //گالوانیزه ی مولیون
                    totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price);
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  چهار لنگه

            if (sample.Id == 20)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + ((width / 2) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //گالوانیزه ی لنگه
                    totalPrice = totalPrice + ((width / 2) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + ((3 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + ((3 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
                {
                    //گالوانیزه ی مولیون
                    totalPrice = totalPrice + ((3 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  شش لنگه 

            if (sample.Id == 21)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + ((width / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //گالوانیزه ی لنگه
                    totalPrice = totalPrice + ((width / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (5 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (5 * ((height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
                {
                    //گالوانیزه ی مولیون
                    totalPrice = totalPrice + (5 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 5) != null)
                {
                    //یراق تک حالته
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price));
                }
            }

            #endregion

            #region درب لولایی  درب سوییچی شیشه یکپارچه UPVC 

            if (sample.Id == 22)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 14) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 14).Price);
                }
            }

            #endregion

            #region  درب لولایی درب سوییچی شیشه یکپارچه UPVC  

            if (sample.Id == 23)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
                {
                    //یراق درب سرویسی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
                }
            }

            #endregion

            #region  درب لولایی  بالکنی سوویچی UPVC

            if (sample.Id == 24)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 16) != null)
                {
                    //گالوانیزه ی لولایی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 16).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //زهوار دوجداره
                    totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
                {
                    //پنل
                    totalPrice = totalPrice + (width * 60) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 14) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 14).Price);
                }
            }

            #endregion

            #region   درب لولایی درب سرویسی UPVC 

            if (sample.Id == 25)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 16) != null)
                {
                    //گالوانیزه ی لولایی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 16).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //زهوار دوجداره
                    totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
                {
                    //پنل
                    totalPrice = totalPrice + (width * 120) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
                {
                    //یراق درب سرویسی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
                }
            }

            #endregion

            #region درب لولایی درب بالکنی دوتکه شیشه یکپارچه سوویچیUPVC  

            if (sample.Id == 26)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
                {
                    //گالوانیزه ی مولیون لولایی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //زهوار دوجداره
                    totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
                {
                    //یراق درب سرویسی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
                }
            }

            #endregion

            #region  درب لولایی درب بالکنی دوتکه پنل دار سوویچیUPVC   

            if (sample.Id == 27)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 7) != null)
                {
                    //مولیون لولایی
                    totalPrice = totalPrice + (width + height) * (userSegments.FirstOrDefault(p => p.SegmentId == 7).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
                {
                    //گالوانیزه ی مولیون لولایی
                    totalPrice = totalPrice + (width + height) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //زهوار دوجداره
                    totalPrice = totalPrice + (width + height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
                {
                    //پنل
                    totalPrice = (width * 60) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 14) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 14).Price);
                }
            }

            #endregion

            #region پنجره ی کشویی دولنگه UPVC    

            if (sample.Id == 28)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
                {
                    //لنگه ی کشویی
                    totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
                } 
                
                if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
                {
                    //گالوانیزه ی لنگه ی کشویی
                    totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    //نوار مویی
                    totalPrice = totalPrice + (width + (2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 22) != null)
                {
                    //کاور لنگه ی کشویی
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
                {
                    //مولوین کشویی
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
                {
                    //گالوانیزه ی مولوین کشویی
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price);
                }
                
                if (userSegments.FirstOrDefault(p => p.SegmentId == 25) != null)
                {
                    //کاور مولوین کشویی
                    totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 25).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
                {
                    //کاور بارانگیر
                    totalPrice = totalPrice + (height + width) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
                {
                    //ریل کشویی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
                {
                    //یراق کشویی
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 28).Price));
                }

            }

            #endregion

            #region  پنجره ی کشویی سه لنگه UPVC     

            if (sample.Id == 29)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
                {
                    //لنگه ی کشویی
                    totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
                {
                    //گالوانیزه ی لنگه ی کشویی
                    totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    //نوار مویی
                    totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 22) != null)
                {
                    //کاور لنگه ی کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
                {
                    //مولوین کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
                {
                    //گالوانیزه ی مولوین کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 25) != null)
                {
                    //کاور مولوین کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 25).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + 2 * ((height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
                {
                    //کاور بارانگیر
                    totalPrice = totalPrice + ((2 * width)/3) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
                {
                    //ریل کشویی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
                {
                    //یراق کشویی
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 28).Price));
                }
            }

            #endregion

            #region  پنجره ی کشویی چهار لنگه UPVC      

            if (sample.Id == 30)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
                {
                    //گالوانیزه ی فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
                {
                    //لنگه ی کشویی
                    totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
                {
                    //گالوانیزه ی لنگه ی کشویی
                    totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    //نوار مویی
                    totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 22) != null)
                {
                    //کاور لنگه ی کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
                {
                    //مولوین کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
                {
                    //گالوانیزه ی مولوین کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 25) != null)
                {
                    //کاور مولوین کشویی
                    totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 25).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + 2 * ((height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
                {
                    //کاور بارانگیر
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
                {
                    //ریل کشویی
                    totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
                {
                    //یراق کشویی
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 28).Price));
                }
            }

            #endregion

            #endregion

            return totalPrice;
        }

        public async Task<FilterInquiryViewModel> FilterInquiryViewModel(FilterInquiryViewModel filter)
        {
            #region Get Product

            var product = _context.Products
                .Include(p => p.MainBrand)
                .Include(p => p.User)
                .ThenInclude(p => p.SellersPersonalInfos)
                .Where(p => !p.IsDelete)
                .Select(p => new InquiryViewModel()
                {
                })
                .ToListAsync();

            #endregion

            return filter;
        }

        public async Task<List<InquiryViewModel>?> ListOfInquiry(List<SampleSizeViewModel> sampleSize, SellersFieldFitreViewModel sellerInfo)
        {
            #region Initial Model 

            List<InquiryViewModel> model = new List<InquiryViewModel>();

            #endregion

            #region Get Brand 

            var brand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sellerInfo.BrandId);
            if (brand == null) return null;

            #endregion

            #region Get Sellers

            var sellers = await _context.MarketPersonalInfo.Include(p => p.User)
                                    .Where(p => !p.IsDelete && p.CityId == sellerInfo.CityId && p.CountryId == sellerInfo.CountryId
                                    && p.StateId == sellerInfo.StateId && p.SellerType == sellerInfo.SellerType)
                                    .Select(p => p.User).ToListAsync();

            #endregion

            #region Get Samples 

            foreach (var seller in sellers)
            {
                InquiryViewModel model2 = new InquiryViewModel();

                model2.UserName = seller.Username;
                model2.UserAvatar = seller.Avatar;
                model2.BrandName = brand.BrandName;
                model2.BrandImage = brand.BrandLogo;
                model2.Price = 0;

                foreach (var sample in sampleSize)
                {
                    model2.Price = model2.Price + await InitialTotalSamplePrice(brand.Id, sample.SampleId, sample.Width, sample.Height, seller.Id);
                }

                model.Add(model2);
            }

            #endregion

            return model;
        }

        #endregion
    }
}