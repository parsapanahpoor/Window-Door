#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.ConditionalFormatting;
using System.Linq.Expressions;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Inquiry;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Entities.Sample;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Services;

#endregion

public class InquryService : IInquiryService
{
    #region Ctor

    private readonly WindowDbContext _context;

    private readonly ISellerService _sellerService;

    public InquryService(WindowDbContext context, ISellerService sellerService)
    {
        _context = context;
        _sellerService = sellerService;
    }

    #endregion

    #region Site Side 

    //Update Log User Inquiry Request
    public async Task UpdateLogUserInquiryRequest(string userMacAddress , ulong brandId)
    {
        //Get Brand 
        var brand = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p=> !p.IsDelete && p.UserMAcAddress == userMacAddress);

        if (brand is not null)
        {
            brand.BrandId = brandId;

            //Update User Log 
            _context.LogInquiryForUsers.Update(brand);
            await _context.SaveChangesAsync();
        }
    }

    //Check Log Result User Inquiry
    public async Task<int> CheckLogResultUserInquiry(string userMacAddress)
    {
        #region Check That Current User Has Any Inquiry OR Not 

        var inquiry = await _context.LogInquiryForUsers
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);

        #endregion

        return await _context.logInquiryForUserDetails
                             .AsNoTracking()
                             .CountAsync(p => !p.IsDelete && p.LogInquiryForUserId == inquiry.Id);
    }

    //Log Inquiry For User In Step 1
    public async Task LogInquiryForUserPart1(FilterInquiryViewModel filter)
    {
        #region Check That Current User Has Any Inquiry OR Not 

        var inquiry = await _context.LogInquiryForUsers
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == filter.UserMacAddress);

        #endregion

        #region Add Inquiry For This User  

        if (inquiry == null)
        {
            LogInquiryForUser log = new LogInquiryForUser()
            {
                CountryId = filter.CountryId,
                CityId = filter.CityId,
                StateId = filter.StateId,
                BrandId = null,
                CreateDate = DateTime.Now,
                IsDelete = false,
                SellerType = filter.SellerType,
                UserMAcAddress = filter.UserMacAddress,
                GlassId = filter.GlassId
            };

            await _context.LogInquiryForUsers.AddAsync(log);
        }

        #endregion

        #region Edit Inquiry For This User

        if (inquiry != null)
        {
            //Delete Log Inquiry For Users
            inquiry.IsDelete = true;
            _context.LogInquiryForUsers.Update(inquiry);

            //Add New Log For Inquiry User
            LogInquiryForUser log = new LogInquiryForUser()
            {
                CountryId = filter.CountryId,
                CityId = filter.CityId,
                StateId = filter.StateId,
                BrandId = filter.MainBrandId,
                CreateDate = DateTime.Now,
                IsDelete = false,
                SellerType = filter.SellerType,
                UserMAcAddress = filter.UserMacAddress,
                GlassId = filter.GlassId
            };

            await _context.LogInquiryForUsers.AddAsync(log);

            var logInquiryUserDetail = await _context.logInquiryForUserDetails
                                                     .Where(p => !p.IsDelete && p.LogInquiryForUserId == inquiry.Id)
                                                     .ToListAsync();
            if (logInquiryUserDetail != null && logInquiryUserDetail.Any())
            {
                _context.logInquiryForUserDetails.RemoveRange(logInquiryUserDetail);
            }

            var logResultOfUser = await _context.LogResultOfUserInquiryWithSellersInfos
                                                .Where(p => !p.IsDelete && p.LogInquiryForUserId == inquiry.Id)
                                                .ToListAsync();

            if (logResultOfUser != null && logResultOfUser.Any())
            {
                _context.LogResultOfUserInquiryWithSellersInfos.RemoveRange(logResultOfUser);
            }
        }

        #endregion

        #region Get Brands

        List<MainBrand> brands = new List<MainBrand>();

        if (filter.MainBrandId.HasValue)
        {
            var brand = await _context.MainBrands
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(p => !p.IsDelete && p.Id == filter.MainBrandId);

            brands.Add(brand);
        }

        if (filter.MainBrandId == null)
        {
            if (filter.SellerType == Domain.Enums.SellerType.SellerType.UPC)
            {
                var getBrands = await _context.MainBrands
                                              .AsNoTracking()
                                              .Where(p => !p.IsDelete && p.UPVC)
                                              .ToListAsync();

                brands.AddRange(getBrands);
            }
            if (filter.SellerType == Domain.Enums.SellerType.SellerType.Aluminium)
            {
                var getBrands = await _context.MainBrands
                                              .AsNoTracking()
                                              .Where(p => !p.IsDelete && p.Alominum)
                                              .ToListAsync();

                brands.AddRange(getBrands);
            }
        }

        #endregion

        #region Log For Brands

        foreach (var brand in brands)
        {
            InquiryViewModel model2 = new InquiryViewModel();

            #region Add Log For Brands

            LogForBrands logForBrands = new LogForBrands()
            {
                MainBrandId = brand.Id,
                CountryId = filter.CountryId,
                CityId = filter.CityId,
                StateId = filter.StateId
            };

            await _context.LogForBrands.AddAsync(logForBrands);

            #endregion
        }

        #region Log For Inquiry

        if (filter.CountryId.HasValue && filter.CityId.HasValue && filter.StateId.HasValue && filter.SellerType.HasValue)
        {
            LogForInquiry logForInquiry = new LogForInquiry()
            {
                CityId = filter.CityId.Value,
                StateId = filter.StateId.Value,
                CountryId = filter.CountryId.Value,
                SellerType = filter.SellerType.Value
            };

            await _context.LogForInquiry.AddAsync(logForInquiry);
        }

        #endregion

        await _context.SaveChangesAsync();

        #endregion
    }

    //Log Inquiry For User In Step 2
    public async Task<bool> LogInquiryForUserPart2(ulong sampleId, int width, int height, int? KatibeSize, string userMacAddress, int productCount)
    {
        #region Get User Log By User Mac Address

        var userLog = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);
        if (userLog == null) return false;

        #endregion

        #region Add Log Detail

        LogInquiryForUserDetail logDetail = new LogInquiryForUserDetail()
        {
            CreateDate = DateTime.Now,
            Height = height,
            IsDelete = false,
            LogInquiryForUserId = userLog.Id,
            SampleId = sampleId,
            Width = width,
            KatibeSize = KatibeSize,
            CountOfSample = productCount
        };

        await _context.logInquiryForUserDetails.AddAsync(logDetail);
        await _context.SaveChangesAsync();

        #endregion

        return true;
    }

    //Update User Inquiry Item 
    public async Task<bool> UpdateUserInquiryItrm(ulong inquiryDetailId, ulong sampleId, int width, int height, int? katibe, string macAddress, int? SampleCount)
    {
        #region Get User Inquiry Item 

        var userInquiry = await _context.logInquiryForUserDetails.Include(p => p.LogInquiryForUser)
                                .FirstOrDefaultAsync(p => p.Id == inquiryDetailId && !p.IsDelete && p.SampleId == sampleId && p.LogInquiryForUser.UserMAcAddress == macAddress);

        #endregion

        if (userInquiry == null) return false;

        #region Update Iqnuiry Item

        userInquiry.Width = width;
        userInquiry.Height = height;
        userInquiry.KatibeSize = katibe;
        userInquiry.CountOfSample = SampleCount.Value;

        #endregion

        _context.logInquiryForUserDetails.Update(userInquiry);
        await _context.SaveChangesAsync();

        return true;
    }

    //Update User Inqury In Last Step For Update Brand 
    public async Task<bool> UpdateUserInquryInLastStep(string userMacAddress, string? brandTitle)
    {
        #region Get User Inqury

        var userInqury = await _context.LogInquiryForUsers
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);

        if (userInqury == null) return false;

        if (brandTitle == "All")
        {
            userInqury.BrandId = null;
        }
        else
        {
            #region Get Brand By Title 

            var brand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.BrandName == brandTitle);
            if (brand == null) return false;

            #endregion

            userInqury.BrandId = brand.Id;
        }

        #endregion

        #region Update User Inquiry

        _context.LogInquiryForUsers.Update(userInqury);
        await _context.SaveChangesAsync();

        #endregion

        return true;
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

    //Calculate Seller Score 
    public async Task<int> CalculateSellerScore(ulong userId)
    {
        #region MyRegion

        var sellerScores = await _context.ScoreForMarkets.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

        var sehat = await _context.SehateEtelaAt.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

        var pasazForosh = await _context.KhadamatePasAzForosh.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

        var pasokhGoie = await _context.PasokhGoie.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

        var taAhodeZaman = await _context.TaAhodeZamaneTahvil.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

        #endregion

        //If Score Dosent Exist Yet
        if (sellerScores == null || !sellerScores.Any()
            || sehat == null || !sehat.Any()
            || pasazForosh == null || !pasazForosh.Any()
            || pasokhGoie == null || !pasokhGoie.Any()
            || taAhodeZaman == null || !taAhodeZaman.Any())
        {
            return 0;
        }

        var score = (sellerScores.Sum(p => p.Score) + sehat.Sum(p => p.Score) + pasazForosh.Sum(p => p.Score) + pasokhGoie.Sum(p => p.Score) + taAhodeZaman.Sum(p => p.Score))
            / (sellerScores.Count() + sehat.Count() + pasazForosh.Count() + pasokhGoie.Count() + taAhodeZaman.Count());

        return score;
    }

    //Calculate Seller Score With As No Tracking
    public async Task<int> CalculateSellerScoreWithAsNoTracking(ulong userId)
    {
        #region MyRegion

        var sellerScores = await _context.ScoreForMarkets
                                         .AsNoTracking()
                                         .Where(p => !p.IsDelete && p.UserId == userId)
                                         .ToListAsync();

        var sehat = await _context.SehateEtelaAt
                                  .AsNoTracking()
                                  .Where(p => !p.IsDelete && p.UserId == userId)
                                  .ToListAsync();

        var pasazForosh = await _context.KhadamatePasAzForosh
                                        .AsNoTracking()
                                        .Where(p => !p.IsDelete && p.UserId == userId)
                                        .ToListAsync();

        var pasokhGoie = await _context.PasokhGoie
                                       .AsNoTracking()
                                       .Where(p => !p.IsDelete && p.UserId == userId)
                                       .ToListAsync();

        var taAhodeZaman = await _context.TaAhodeZamaneTahvil
                                         .AsNoTracking()
                                         .Where(p => !p.IsDelete && p.UserId == userId)
                                         .ToListAsync();

        #endregion

        //If Score Dosent Exist Yet
        if (sellerScores == null || !sellerScores.Any()
            || sehat == null || !sehat.Any()
            || pasazForosh == null || !pasazForosh.Any()
            || pasokhGoie == null || !pasokhGoie.Any()
            || taAhodeZaman == null || !taAhodeZaman.Any())
        {
            return 0;
        }

        var score = (sellerScores.Sum(p => p.Score)
                    + sehat.Sum(p => p.Score)
                    + pasazForosh.Sum(p => p.Score)
                    + pasokhGoie.Sum(p => p.Score)
                    + taAhodeZaman.Sum(p => p.Score))
                    /
                    (sellerScores.Count()
                    + sehat.Count()
                    + pasazForosh.Count()
                    + pasokhGoie.Count()
                    + taAhodeZaman.Count());

        return score;
    }

    public async Task<List<InquiryViewModel>?> ListOfInquiry(string userMacAddress, ulong userId)
    {
        #region Get User log 

        var log = await _context.LogInquiryForUsers
                                .AsNoTracking()
                                .Where(p => p.UserMAcAddress == userMacAddress && !p.IsDelete)
                                .Select(p => p.Id)
                                .FirstOrDefaultAsync();

        if (log == null) return null;

        var userInquiryResults = await _context.LogResultOfUserInquiryWithSellersInfos
                                               .AsNoTracking()
                                               .Where(p => p.UserId == userId && p.LogInquiryForUserId == log)
                                               .ToListAsync();

        #endregion

        #region Fill Return Model

        List<InquiryViewModel> model = new List<InquiryViewModel>();

        if (userInquiryResults != null && userInquiryResults.Any())
        {
            foreach (var userInquiryResult in userInquiryResults)
            {
                //Update Sellers Activation Tarrifs
                await _sellerService.UpdateSellerActivationTariff(userInquiryResult.SellerUserId, true, false);

                InquiryViewModel modelChilds = new InquiryViewModel()
                {
                    BrandImage = await _context.MainBrands
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.BrandId)
                                               .Select(p => p.BrandLogo)
                                               .FirstOrDefaultAsync(),
                    BrandName = await _context.MainBrands
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.BrandId)
                                               .Select(p => p.BrandName)
                                               .FirstOrDefaultAsync(),
                    Price = userInquiryResult.Price,
                    Score = userInquiryResult.SellerScore,
                    ShopName = userInquiryResult.SellerShopName,
                    UserAvatar = await _context.Users
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.SellerUserId)
                                               .Select(p => p.Avatar)
                                               .FirstOrDefaultAsync(),
                    UserName = await _context.Users
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.SellerUserId)
                                               .Select(p => p.Username)
                                               .FirstOrDefaultAsync(),
                    UserId = userId
                };

                model.Add(modelChilds);
            }
        }

        #endregion

        return model;
    }

    //Initial Result Of User Inquiry
    public async Task<bool> InitialResultOfUserInquiry(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, ulong UserId, string userMacAddress)
    {
        #region Get User Log By User Mac Address

        var userLog = await _context.LogInquiryForUsers
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);
        if (userLog == null) return false;

        #endregion

        #region Add Log Detail

        LogInquiryForUserDetail logDetail = new LogInquiryForUserDetail()
        {
            CreateDate = DateTime.Now,
            Height = height,
            IsDelete = false,
            LogInquiryForUserId = userLog.Id,
            SampleId = sampleId,
            Width = width,
            KatibeSize = katibeSize,
            CountOfSample = SampleCount
        };

        await _context.logInquiryForUserDetails.AddAsync(logDetail);

        #endregion

        #region Initial Inquiry Result

        #region Get Sellers

        List<ulong> sellersUserId = await _context.MarketPersonalInfo
                                  .AsNoTracking()
                                  .Include(p => p.Market)
                                  .Where(p => !p.IsDelete && p.CityId == userLog.CityId && p.CountryId == userLog.CountryId && p.StateId == userLog.StateId
                                            && p.Market.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)
                                  .Select(p => p.UserId)
                                  .ToListAsync();

        #endregion

        #region Get Brands

        List<ulong> brands = new List<ulong>();

        if (userLog.BrandId.HasValue)
        {
            var brand = await _context.MainBrands
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete && p.Id == userLog.BrandId)
                                      .Select(p => p.Id)
                                      .FirstOrDefaultAsync();

            brands.Add(brand);
        }

        if (userLog.BrandId == null)
        {
            if (userLog.SellerType == Domain.Enums.SellerType.SellerType.UPC)
            {
                var getBrands = await _context.MainBrands
                                              .AsNoTracking()
                                              .Where(p => !p.IsDelete && p.UPVC)
                                              .Select(p => p.Id)
                                              .ToListAsync();

                brands.AddRange(getBrands);
            }
            if (userLog.SellerType == Domain.Enums.SellerType.SellerType.Aluminium)
            {
                var getBrands = await _context.MainBrands
                                              .AsNoTracking()
                                              .Where(p => !p.IsDelete && p.Alominum)
                                              .Select(p => p.Id)
                                              .ToListAsync();

                brands.AddRange(getBrands);
            }
        }

        #endregion

        #region Get Samples 

        foreach (var sellerUserId in sellersUserId)
        {
            foreach (var brandId in brands)
            {
                //Initial Inquiry Price
                double? inquiryPrice = await InitialTotalSamplePriceWithAsNoTracking(brandId, sampleId, height, width, SampleCount, katibeSize, sellerUserId, userLog.GlassId.Value);

                if (inquiryPrice.HasValue && inquiryPrice.Value != 0)
                {
                    var lastUserInquiryResult = await _context.LogResultOfUserInquiryWithSellersInfos
                                                              .AsNoTracking()
                                                              .FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == UserId && p.SellerUserId == sellerUserId
                                                              && p.BrandId == brandId && p.LogInquiryForUserId == userLog.Id);

                    if (lastUserInquiryResult != null)
                    {
                        lastUserInquiryResult.Price = lastUserInquiryResult.Price + inquiryPrice.Value;

                        _context.LogResultOfUserInquiryWithSellersInfos.Update(lastUserInquiryResult);
                    }

                    else
                    {
                        #region Fill Log Result Of User Inquiry With Sellers Info

                        LogResultOfUserInquiryWithSellersInfo logResuly = new LogResultOfUserInquiryWithSellersInfo()
                        {
                            BrandId = brandId,
                            CreateDate = DateTime.Now,
                            IsDelete = false,
                            LogInquiryForUserId = userLog.Id,
                            SellerShopName = await _context.Market
                                                           .AsNoTracking()
                                                           .Where(p => !p.IsDelete && p.UserId == sellerUserId)
                                                           .Select(p => p.MarketName)
                                                           .FirstOrDefaultAsync(),
                            SellerUserId = sellerUserId,
                            UserId = UserId,
                            SellerScore = 0,
                            Price = inquiryPrice.Value,
                        };

                        #endregion

                        await _context.LogResultOfUserInquiryWithSellersInfos.AddAsync(logResuly);
                    }

                }
            }
        }

        await _context.SaveChangesAsync();

        #endregion

        #endregion

        return true;
    }

    //Initial Total Sample Price With As No Tracking
    public async Task<double?> InitialTotalSamplePriceWithAsNoTracking(ulong brandId, ulong sampleId, int incomeheight, int incomewidth, int productCount, int? katibeSizes, ulong userId, ulong glassId)
    {
        #region Proccess Height And Width

        double height = (double)incomeheight / (double)100;
        double width = (double)incomewidth / (double)100;
        double? katibeSize = 0;

        if (katibeSizes.HasValue) katibeSize = (double)katibeSizes / (double)100;

        #endregion

        #region return Model 

        double totalPrice = 0;

        #endregion

        #region Get User Segments Price 

        var userSegments = await _context.SegmentPricings
                                         .AsNoTracking()
                                         .Where(p => !p.IsDelete && p.Product.UserId == userId && p.Product.MainBrandId == brandId)
                                         .ToListAsync();

        if (userSegments == null || !userSegments.Any()) return null;

        #endregion

        #region Get Glasses Pricing

        var glassPricing = await _context.GlassPricings
                                         .AsNoTracking()
                                         .Where(p => !p.IsDelete && p.GlassId == glassId && p.UserId == userId)
                                         .Select(p => p.Price)
                                         .FirstOrDefaultAsync();

        if (glassPricing == null) return null;

        #endregion

        #region pricing & Get Samples

        #region Get Sample By Id 

        var samplesId = await _context.Samples
                                     .AsNoTracking()
                                     .Where(p => !p.IsDelete && p.Id == sampleId)
                                     .Select(p => p.Id)
                                     .FirstOrDefaultAsync();

        if (sampleId == null) return null;

        #endregion

        #region پنجره لولایی آلومینیومی فیکس 

        if (samplesId == 10)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی آلومینیومی دریچه 

        if (samplesId == 9)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                //قیمت لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 41) != null)
            {
                // یراق دریچه آلومینیومی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 41).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی آلومینیومی لولایی  تک لنگه 

        if (samplesId == 8)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                //قیمت لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 40).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی آلومینیومی لولایی  دولنگه 

        if (samplesId == 34)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 40).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی آلومینیومی لولایی سه لنگه 

        if (samplesId == 35)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + ((2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 40).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی آلومینیومی لولایی  چهار لنگه 

        if (samplesId == 36)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (width + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + ((3 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + ((3 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 40).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی آلومینیومی لولایی شش لنگه 

        if (samplesId == 37)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + (5 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (5 * ((height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price)));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 40).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC فیکس 

        if (samplesId == 33)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC دریچه

        if (samplesId == 32)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                //قیمت لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 14) != null)
            {
                //یراق دریچه لولایی upvc
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 14).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC لولایی تک لنگه

        if (samplesId == 31)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                //قیمت لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC لولایی  دولنگه

        if (samplesId == 30)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                //لنگه لولایی upvc
                totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC لولایی سه لنگه

        if (samplesId == 29)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                //لنگه ی لنگه لولایی upvc
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + ((2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC لولایی  چهار لنگه

        if (samplesId == 28)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                //لنگه لولایی upvc
                totalPrice = totalPrice + ((width) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + ((width) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + ((3 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc
                totalPrice = totalPrice + ((3 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + ((3 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره لولایی UPVC لولایی  شش لنگه 

        if (samplesId == 21)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                //لنگه لولایی upvc
                totalPrice = totalPrice + ((2 * width / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + ((2 * width / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + (5 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (5 * ((height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price)));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + (5 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region درب لولایی سوییچی شیشه یکپارچه UPVC (id = 20)

        if (samplesId == 20)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 31).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region  درب لولایی سرویسی شیشه یکپارچه UPVC  (id = 19)

        if (samplesId == 19)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 31).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region  درب لولایی  بالکنی سوویچی UPVC (id = 18)

        if (samplesId == 18)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 31).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 22) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height - ((double)60 / (double)100))));
            }
        }

        #endregion

        #region   درب لولایی درب سرویسی UPVC (id = 17)

        if (samplesId == 17)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 31).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه ی لولایی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 22) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)120 / (double)100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 10).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height - ((double)120 / (double)100))));
            }
        }

        #endregion

        #region درب لولایی درب بالکنی دوتکه شیشه یکپارچه سوویچیUPVC  (id = 16)

        if (samplesId == 16 && katibeSizes.HasValue)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه ی مولیون لولایی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 31).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
            {
                //یراق درب سوییچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region  درب لولایی بالکنی دوتکه پنل دار سوویچیUPVC (id = 15)

        if (samplesId == 15 && katibeSizes.HasValue)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width + height) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه ی مولیون لولایی
                totalPrice = totalPrice + (width + height) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width + height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 31).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 18).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 22) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height - ((double)60 / (double)100))));
            }
        }

        #endregion

        #region درب لولایی سوییچی شیشه یکپارچه Alminum (id = 38)

        if (samplesId == 38)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 49).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 38).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region  درب لولایی سرویسی شیشه یکپارچه Alminum  (id = 39)

        if (samplesId == 39)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 49).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 37) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 37).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region  درب لولایی  بالکنی سوویچی Alminum (id = 40)

        if (samplesId == 40)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 49).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 48) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 48).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 38).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height - ((double)60 / (double)100))));
            }
        }

        #endregion

        #region   درب لولایی درب سرویسی Alminum (id = 41)

        if (samplesId == 41)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 49).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 48) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)120 / (double)100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 48).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 37) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 37).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height - ((double)120 / (double)100))));
            }
        }

        #endregion

        #region  درب لولایی  بالکنی سوویچیدرب لولایی آلومینیومی بالکنی دوتکه  شیشه  یکپارچه سوویچی (id = 42)

        if (samplesId == 42)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 49).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 38).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height)));
            }
        }

        #endregion

        #region  درب لولایی بالکنی دوتکه پنل دار سوویچیUPVC (id = 43 checked)

        if (samplesId == 43 && katibeSize.HasValue)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width + height) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width + height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 49).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 48) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 48).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 38).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * (height - ((double)60 / (double)100))));
            }
        }

        #endregion

        #region پنجره های کتیبه دار (checked)

        #region پنجره ی لولایی کتیبه دار یو پی وی سی 

        #region پنجره ی فیکس لولایی کتیبه دار یو پی وی سی (id = 27)

        if (samplesId == 27)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره ی دریچه لولایی کتیبه دار یو پی وی سی (id = 26)

        if (samplesId == 26)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 14) != null)
            {
                // -یراق دریچه لولایی upvc  
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 14)).Price);
            }

        }

        #endregion

        #region پنجره لولایی تک لنگه کتیبه دار یو پی وی سی (id = 25)

        if (samplesId == 25)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                // -یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 13)).Price);
            }

        }

        #endregion

        #region پنجره لولایی پنجره لولایی دولنگه کتیبه دار یو پی وی سی (id = 24)

        if (samplesId == 24)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + ((width) + (2 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + ((width) + (2 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width) + (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width) + (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * ((width) + (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 13)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی سه کتیبه دار یو پی وی سی (id = 23)

        if (samplesId == 23)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 2 * (2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 13)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی چهار کتیبه دار یو پی وی سی (id = 22)

        if (samplesId == 22)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + ((width) + (4 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + ((width) + (4 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 3 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 3 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 3 * (2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + ((width + (katibeSize.Value)) * 2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + 2 * (((userSegments.FirstOrDefault(p => p.SegmentId == 13)).Price));
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی شش کتیبه دار یو پی وی سی (id = 47)

        if (samplesId == 47)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 5 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 5 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 5 * (2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 3 * ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 3 * ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 3 * (2 * ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + 2 * (((userSegments.FirstOrDefault(p => p.SegmentId == 13)).Price));
            }
        }

        #endregion

        #endregion

        #region پنجره ی لولایی کتیبه دار آلمینیوم 

        #region پنجره ی فیکس لولایی کتیبه دار آلمینیوم (id = 54)

        if (samplesId == 54)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (4 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region پنجره ی دریچه ی لولایی کتیبه دار آلمینیوم (id = 53)

        if (samplesId == 53)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + ((width)) * (4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 41) != null)
            {
                // یراق درسچه آلومینیومی   
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 41)).Price);
            }

        }

        #endregion

        #region پنجره لولایی تک لنگه کتیبه دار آلمینیوم (id = 52)

        if (samplesId == 52)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + ((width)) * (4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی  
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 40)).Price);
            }

        }

        #endregion

        #region پنجره لولایی پنجره لولایی دولنگه کتیبه دار آلمینیوم (id = 51)

        if (samplesId == 51)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + ((width) + (2 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width) + (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + (2 * ((width) + (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (4 * ((width) + (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 40)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی سه لنگه کتیبه دار آلمینیوم (id = 50)

        if (samplesId == 50)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + 2 * (2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 2 * (4 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 40)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی چهار لنگه کتیبه دار آلمینیوم (id = 49)

        if (samplesId == 49)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی   
                totalPrice = totalPrice + ((width) + (4 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 3 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + 3 * (2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 3 * (4 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی    
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک    
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + 2 * (((userSegments.FirstOrDefault(p => p.SegmentId == 40)).Price));
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی شش لنگه کتیبه دار آلمینیوم (id = 48)

        if (samplesId == 48)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی   
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * ((height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 50).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + ((width)) * 2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه 
                totalPrice = totalPrice + ((width)) * 4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 5 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + 5 * (2 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 5 * (4 * ((height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 3 * ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + 3 * (2 * ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 3 * (4 * ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + 2 * (((userSegments.FirstOrDefault(p => p.SegmentId == 40)).Price));
            }
        }

        #endregion

        #endregion

        #endregion

        #region پنجره ھای کشویی کتیبھدار

        #region  پنجره ھای کشویی کتیبھدار آلومینیومی 

        #region   پنجره ی کشویی آلمینیوم دولنگه ی کتیبه دار (id = 44)   

        if (samplesId == 44)
        {
            if (katibeSize == null)
            {
                return null;
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 46) != null)
            {
                // فریم کشویی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 46).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 45) != null)
            {
                // لنگه کشویی آلومینیومی 
                totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 45).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                // نوار مویی
                totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 44) != null)
            {
                // اینترلاک
                totalPrice = totalPrice + ((2 * (height - katibeSize.Value))) * ((userSegments.FirstOrDefault(p => p.SegmentId == 44).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 43) != null)
            {
                // ریل کشویی آلومینیومی 
                totalPrice = totalPrice + (width) * ((userSegments.FirstOrDefault(p => p.SegmentId == 43).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                // فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54)).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 39) != null)
            {
                //قیمت یراق کشویی پنجره آلومینیومی 
                totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 39)).Price);
            }
        }

        #endregion

        #region   پنجره ی کشویی آلمینیوم سه لنگه ی کتیبه دار (id = 45)   

        if (samplesId == 45)
        {
            if (katibeSize == null)
            {
                return null;
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 46) != null)
            {
                // فریم کشویی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 46).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 45) != null)
            {
                // لنگه کشویی آلومینیومی 
                totalPrice = totalPrice + (((4 * width) / 3) + (4 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 45).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (((4 * width) / 3) + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                // نوار مویی
                totalPrice = totalPrice + (((4 * width) / 3) + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 44) != null)
            {
                // اینترلاک
                totalPrice = totalPrice + ((4 * (height - katibeSize.Value))) * ((userSegments.FirstOrDefault(p => p.SegmentId == 44).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 43) != null)
            {
                // ریل کشویی آلومینیومی 
                totalPrice = totalPrice + (width) * ((userSegments.FirstOrDefault(p => p.SegmentId == 43).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                // فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54)).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                // مولیون لولایی آلمینیوم 
                totalPrice = totalPrice + ((katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + ((katibeSize.Value)) * ((2) * (userSegments.FirstOrDefault(p => p.SegmentId == 47)).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + ((katibeSize.Value)) * (4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54)).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 39) != null)
            {
                //قیمت یراق کشویی پنجره آلومینیومی 
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 39)).Price);
            }
        }

        #endregion

        #region پنجره ی کشویی آلمینیوم چهار لنگه ی کتیبه دار (id = 46)       

        if (samplesId == 46)
        {
            if (katibeSize == null)
            {
                return null;
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 46) != null)
            {
                // فریم کشویی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 46).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 45) != null)
            {
                // لنگه کشویی آلومینیومی 
                totalPrice = totalPrice + ((2 * width) + (8 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 45).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + ((2 * width) + (8 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                // نوار مویی
                totalPrice = totalPrice + ((2 * width) + (8 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 44) != null)
            {
                // اینترلاک
                totalPrice = totalPrice + ((4 * (height - katibeSize.Value))) * ((userSegments.FirstOrDefault(p => p.SegmentId == 44).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 43) != null)
            {
                // ریل کشویی آلومینیومی 
                totalPrice = totalPrice + (width) * ((userSegments.FirstOrDefault(p => p.SegmentId == 43).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 42) != null)
            {
                // قفل چهار لنگه 
                totalPrice = totalPrice + (height - katibeSize.Value) * ((userSegments.FirstOrDefault(p => p.SegmentId == 42).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 52) != null)
            {
                // فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 52).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54)).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 55) != null)
            {
                // مولیون لولایی آلمینیوم 
                totalPrice = totalPrice + (3 * (katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (3 * (katibeSize.Value)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47)).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (3 * (katibeSize.Value)) * (4 * (userSegments.FirstOrDefault(p => p.SegmentId == 54)).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 39) != null)
            {
                //قیمت یراق کشویی پنجره آلومینیومی 
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 39)).Price);
            }
        }

        #endregion

        #endregion

        #region پنجره ھای کشویی کتیبھدار UPVC

        #region پنجره کشویی دو لنگه کتیبھدار UPVC

        if (samplesId == 55)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 29).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 56) != null)
            {
                //کتیبه ی کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 56).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //فریم لولایی 
                totalPrice = totalPrice + (height - katibeSize.Value) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم لولایی  
                totalPrice = totalPrice + (height - katibeSize.Value) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (width + (2 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + ((height - katibeSize.Value) + width) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

        }

        #endregion

        #region پنجره کشویی سه لنگه کتیبھدار UPVC

        if (samplesId == 56)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 29).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 56) != null)
            {
                //کتیبه ی کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 56).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //زهوار دوجدار لولایی 
                totalPrice = totalPrice + (katibeSize.Value) * 2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه ی مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //فریم لولایی 
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم لولایی  
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (2 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (2 * (((2 * width) / 3) + (2 * (height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (2 * (((2 * width) / 3) + (2 * (height - katibeSize.Value)))) * (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (2 * (((2 * width) / 3) + (2 * (height - katibeSize.Value)))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + (4 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + ((2 * width) / 3) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + 2 * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

        }

        #endregion

        #region پنجره کشویی جهار لنگه کتیبھدار UPVC

        if (samplesId == 57)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 29).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 56) != null)
            {
                //کتیبه ی کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 56).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //زهوار دوجدار لولایی 
                totalPrice = totalPrice + (katibeSize.Value) * 6 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 20) != null)
            {
                //گالوانیزه ی مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * 3 * (userSegments.FirstOrDefault(p => p.SegmentId == 20).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
            {
                //مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * 3 * (userSegments.FirstOrDefault(p => p.SegmentId == 33).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
            {
                //فریم لولایی 
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
            {
                //گالوانیزه ی فریم لولایی  
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (2 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (width + (4 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (width + (4 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (width + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + (4 * (height - katibeSize.Value)) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 36) != null)
            {
                //قفل چهارلنگه
                totalPrice = totalPrice + (height - katibeSize.Value) * (userSegments.FirstOrDefault(p => p.SegmentId == 36).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + 2 * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

        }

        #endregion

        #endregion

        #endregion

        #region پنجره ی کشویی 

        #region UPVC

        #region پنجره ی  UPVC (id = 13)

        if (samplesId == 13)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 29).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (width + (2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
            {
                //کاور لنگه ی کشویی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
            {
                //مولوین کشویی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 28).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 16) != null)
            {
                //گالوانیزه ی مولوین کشویی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 16).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 25) != null)
            {
                //کاور مولوین کشویی
                totalPrice = totalPrice + (height) * (userSegments.FirstOrDefault(p => p.SegmentId == 25).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + (height + width) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

        }

        #endregion

        #region   UPVC  (id = 12)

        if (samplesId == 12)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 29).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price)));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
            {
                //کاور لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
            {
                //مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 28).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 16) != null)
            {
                //گالوانیزه ی مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 16).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 25) != null)
            {
                //کاور مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 25).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + 2 * ((height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price)));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + ((2 * width) / 3) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #region  پنجره ی  UPVC (id = 11)     

        if (samplesId == 11)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 29).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 17).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 27).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price)));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 24) != null)
            {
                //کاور لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 24).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
            {
                //مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 28).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 16) != null)
            {
                //گالوانیزه ی مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 16).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 25) != null)
            {
                //کاور مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * (userSegments.FirstOrDefault(p => p.SegmentId == 25).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + 2 * ((height) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price)));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 23).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * (userSegments.FirstOrDefault(p => p.SegmentId == 8).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }
        }

        #endregion

        #endregion

        #region AL

        #region پنجره ی کشویی آلمینیومی دولنگه  (id = 58)

        if (samplesId == 58)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 46) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 46).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 45) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + ((2 * width) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 45).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //لاستیک فشاری
                totalPrice = totalPrice + ((2 * width) + (4 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + ((2 * width) + (4 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 44) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + ((2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 44).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 43) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 43).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 39) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 39).Price);
            }

        }

        #endregion

        #region پنجره ی کشویی آلمینیومی سه لنگه  (id = 59)

        if (samplesId == 59)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 46) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 46).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 45) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + ((2 * width) + (6 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 45).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //لاستیک فشاری
                totalPrice = totalPrice + ((2 * width) + (6 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + ((2 * width) + (6 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 44) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + ((4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 44).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 43) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 43).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 39) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 39).Price);
            }
        }

        #endregion

        #region پنجره ی کشویی آلمینیومی چهار لنگه  (id = 60)

        if (samplesId == 60)
        {
            if (userSegments.FirstOrDefault(p => p.SegmentId == 46) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 46).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 45) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + ((2 * width) + (8 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 45).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 54) != null)
            {
                //لاستیک فشاری
                totalPrice = totalPrice + ((2 * width) + (8 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 54).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + ((2 * width) + (8 * height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 53).Price));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 44) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + ((4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 44).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 43) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + ((width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 43).Price);
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 42) != null)
            {
                //قفل چهار لنگه
                totalPrice = totalPrice + ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 42).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (userSegments.FirstOrDefault(p => p.SegmentId == 39) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 39).Price));
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        //Product Count
        if (productCount >= 1)
        {
            totalPrice = totalPrice * productCount;
        }

        return totalPrice;
    }

    //Check Is User Scored To Seller 
    public async Task<bool> checkIsUserScoredToSeller(string macAddress, ulong sellerId)
    {
        #region MyRegion

        var scored = await _context.ScoreForMarkets.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == sellerId);
        if (scored == null) return false;

        #endregion

        return true;
    }

    //Add Score For Seller
    public async Task<bool> AddScoreForSeller(int score, ulong sellerId, string userMacAddress)
    {
        #region Add Method

        ScoreForMarket model = new ScoreForMarket()
        {
            UserId = sellerId,
            Score = score,
            macAddress = userMacAddress
        };

        await _context.ScoreForMarkets.AddAsync(model);
        await _context.SaveChangesAsync();

        #endregion

        return true;
    }

    //Add Score For Seller
    public async Task<bool> AddScoreForSeller(AddScoreToTheSellerViewModel model, string userMacAddress)
    {
        #region KeyFiate Kar Score

        ScoreForMarket ScoreForMarket = new ScoreForMarket()
        {
            UserId = model.SellerId,
            Score = model.KeyfiateKar,
            macAddress = userMacAddress
        };

        await _context.ScoreForMarkets.AddAsync(ScoreForMarket);
        await _context.SaveChangesAsync();

        #endregion

        #region Pasokh Goie Score

        PasokhGoie pasokhGoie = new PasokhGoie()
        {
            UserId = model.SellerId,
            Score = model.PasokhGoie,
            macAddress = userMacAddress
        };

        await _context.PasokhGoie.AddAsync(pasokhGoie);
        await _context.SaveChangesAsync();

        #endregion

        #region Sehate EtelaAt

        SehateEtelaAt sehateEtelaAt = new SehateEtelaAt()
        {
            UserId = model.SellerId,
            Score = model.SehateEtelaAt,
            macAddress = userMacAddress
        };

        await _context.SehateEtelaAt.AddAsync(sehateEtelaAt);
        await _context.SaveChangesAsync();

        #endregion

        #region TaAhode Zamane Tahvil 

        TaAhodeZamaneTahvil taAhod = new TaAhodeZamaneTahvil()
        {
            UserId = model.SellerId,
            Score = model.TaAhodZamaneTahvil,
            macAddress = userMacAddress
        };

        await _context.TaAhodeZamaneTahvil.AddAsync(taAhod);
        await _context.SaveChangesAsync();

        #endregion

        #region Khadamate Pasaz Forosh Score 

        KhadamatePasAzForosh khadamat = new KhadamatePasAzForosh()
        {
            UserId = model.SellerId,
            Score = model.TaAhodZamaneTahvil,
            macAddress = userMacAddress
        };

        await _context.KhadamatePasAzForosh.AddAsync(khadamat);
        await _context.SaveChangesAsync();

        #endregion

        return true;
    }

    //Get Count Of Inquiry In Cities
    public async Task<int> GetCountOfInquiryInCities(string cityName)
    {
        //Get City By Name 
        var city = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.UniqueName == cityName);
        if (city == null) return 0;

        var model = await _context.LogInquiryForUsers.Where(p => !p.IsDelete && p.CityId == city.Id).ToListAsync();

        return model.Count();
    }

    //Get Count Of Inquiry In State 
    public async Task<int> CountOfInquiryInState(string stateName)
    {
        //Get City By State 
        var state = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.UniqueName == stateName);
        if (state == null) return 0;

        var model = await _context.LogInquiryForUsers.Where(p => !p.IsDelete && p.StateId == state.Id).ToListAsync();

        return model.Count();
    }

    //Get User Lastest Inquiry 
    public async Task<List<LogInquiryForUserDetail>?> GetUserLastestInquiryDetailForChange(string macAddress)
    {
        #region Get User Lastest Inquiry Detail 

        var log = await _context.logInquiryForUserDetails.Include(p => p.Sample).Include(p => p.LogInquiryForUser)
                                                        .Where(p => !p.IsDelete &&
                                                                p.LogInquiryForUser.UserMAcAddress == macAddress).ToListAsync();
        #endregion

        return log;
    }

    //Delete User Lastest Inquiry Detail 
    public async Task<bool> DeleteUserLastestInquiryDetail(ulong inquiryDetailId, string macAddress)
    {
        #region Get User Lastest Inquiry Detail 

        var lastestUserInquiryDetail = await _context.logInquiryForUserDetails.Include(p => p.LogInquiryForUser)
                                                .FirstOrDefaultAsync(p => !p.IsDelete && p.LogInquiryForUser.UserMAcAddress == macAddress
                                                                        && p.Id == inquiryDetailId);

        if (lastestUserInquiryDetail == null)
        {
            return false;
        }

        #endregion

        #region Remove User Inquiry Detail

        _context.logInquiryForUserDetails.Remove(lastestUserInquiryDetail);
        await _context.SaveChangesAsync();

        #endregion

        return true;
    }

    #endregion

    #region Admin side

    public async Task<int?> CountOfTodayInquiry()
    {
        return await _context.LogForInquiry.Where(p => !p.IsDelete && p.CreateDate.Year == DateTime.Now.Year
                                                           && p.CreateDate.DayOfYear == DateTime.Now.DayOfYear).CountAsync();
    }

    public async Task<int?> CountOfMonthInquiry()
    {
        return await _context.LogForInquiry.Where(p => !p.IsDelete && p.CreateDate.Year == DateTime.Now.Year
                                                           && p.CreateDate.Month == DateTime.Now.Month).CountAsync();
    }

    public async Task<int?> CountOfYearInquiry()
    {
        return await _context.LogForInquiry.Where(p => !p.IsDelete && p.CreateDate.Year == DateTime.Now.Year).CountAsync();
    }

    public Task<double?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int height, int width, int productCount, int? katibeSizes, ulong userId, ulong glassId)
    {
        throw new NotImplementedException();
    }

    #endregion
}
