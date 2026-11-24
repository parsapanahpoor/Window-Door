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
using Window.Domain.Entities.Product;
using Window.Domain.Enums.SellerType;
using Window.Domain.Entities.Score;
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
    public async Task UpdateLogUserInquiryRequest(string userMacAddress, ulong brandId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var inquiry = await LockInquiry(userMacAddress);
        if (inquiry == null) return;

        inquiry.BrandId = brandId;
        await _context.SaveChangesAsync();
        await RebuildInquiryResults(inquiry, userMacAddress);
        await transaction.CommitAsync();
    }

    private async Task<LogInquiryForUser?> LockInquiry(string token)
    {
        var query = _context.LogInquiryForUsers.Where(inquiry => !inquiry.IsDelete && inquiry.UserMAcAddress == token);
        if (await query.ExecuteUpdateAsync(setters => setters.SetProperty(inquiry => inquiry.IsDelete, false)) != 1)
            return null;

        return await query.SingleAsync();
    }

    public async Task<int> CheckLogResultUserInquiry(string userMacAddress)
    {
        #region Check That Current User Has Any Inquiry OR Not 

        var inquiry = await _context.LogInquiryForUsers
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);

        if (inquiry == null) return 0;

        #endregion

        return await _context.logInquiryForUserDetails
                             .AsNoTracking()
                             .CountAsync(p => !p.IsDelete && p.LogInquiryForUserId == inquiry.Id);
    }

    //Attach a guest inquiry to the signed-in user.
    public Task<bool> IsUnownedInquiry(string inquiryToken) =>
        _context.LogInquiryForUsers.AnyAsync(p => !p.IsDelete && p.UserMAcAddress == inquiryToken && p.OwnerUserId == null);

    public async Task<bool> ClaimInquiry(string inquiryToken, ulong userId)
    {
        if (userId == 0 || string.IsNullOrWhiteSpace(inquiryToken)) return false;

        var updated = await _context.LogInquiryForUsers
            .Where(p => !p.IsDelete && p.UserMAcAddress == inquiryToken && p.OwnerUserId == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.OwnerUserId, (ulong?)userId));

        return updated == 1 || await InquiryBelongsToUser(inquiryToken, userId);
    }

    public async Task<bool> InquiryBelongsToUser(string inquiryToken, ulong userId)
    {
        if (string.IsNullOrWhiteSpace(inquiryToken) || userId == 0) return false;

        return await _context.LogInquiryForUsers
                             .AsNoTracking()
                             .AnyAsync(p => !p.IsDelete && p.UserMAcAddress == inquiryToken && p.OwnerUserId == userId);
    }

    public async Task<string?> GetLatestInquiryTokenForUser(ulong userId)
    {
        if (userId == 0) return null;

        return await _context.LogInquiryForUsers
                             .AsNoTracking()
                             .Where(p => !p.IsDelete && p.OwnerUserId == userId)
                             .OrderByDescending(p => p.Id)
                             .Select(p => p.UserMAcAddress)
                             .FirstOrDefaultAsync();
    }

    //Log Inquiry For User In Step 1
    public async Task LogInquiryForUserPart1(FilterInquiryViewModel filter, ulong? ownerUserId = null)
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
                BrandId = filter.MainBrandId,
                CreateDate = DateTime.Now,
                IsDelete = false,
                SellerType = filter.SellerType,
                UserMAcAddress = filter.UserMacAddress,
                OwnerUserId = ownerUserId,
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
                OwnerUserId = ownerUserId,
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
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var inquiry = await LockInquiry(macAddress);
        if (inquiry == null) return false;
        #region Get User Inquiry Item

        var userInquiry = await _context.logInquiryForUserDetails.Include(p => p.LogInquiryForUser)
                                .FirstOrDefaultAsync(p => p.Id == inquiryDetailId && !p.IsDelete && p.SampleId == sampleId && p.LogInquiryForUserId == inquiry.Id);

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
        await RebuildInquiryResults(inquiry, macAddress);
        await transaction.CommitAsync();

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

    public async Task<List<InquiryViewModel>?> ListOfInquiry(string userMacAddress, string userId)
    {
        #region Get User log

        var userInquiryResults = await _context.LogResultOfUserInquiryWithSellersInfos
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.UserId == userId && _context.LogInquiryForUsers.Any(inquiry => !inquiry.IsDelete && inquiry.Id == p.LogInquiryForUserId && inquiry.UserMAcAddress == userMacAddress))
                                               .ToListAsync();

        #endregion

        #region Fill Return Model

        List<InquiryViewModel> model = new List<InquiryViewModel>();

        if (userInquiryResults != null && userInquiryResults.Any())
        {
            foreach (var userInquiryResult in userInquiryResults)
            {

                var brand = await _context.MainBrands
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.BrandId)
                                               .Select(p => new brandInquiryViewModel()
                                               {
                                                   brandName = p.BrandName,
                                                   BreandLogo = p.BrandLogo
                                               })
                                               .FirstOrDefaultAsync();

                var user = await _context.Users
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.SellerUserId)
                                               .Select(p => new UserInquiryViewModel()
                                               {
                                                   UserAvatar = p.Avatar,
                                                   Username = p.Username,
                                               })
                                               .FirstOrDefaultAsync();

                if (brand == null || user == null) continue;

                InquiryViewModel modelChilds = new InquiryViewModel()
                {
                    BrandImage = brand.BreandLogo,
                    BrandName = brand.brandName,
                    Price = userInquiryResult.Price,
                    Score = userInquiryResult.SellerScore,
                    ShopName = userInquiryResult.SellerShopName,
                    UserAvatar = user.UserAvatar,
                    UserName = user.Username,
                    UserId = userInquiryResult.SellerUserId,
                    SellerPersonalBanner = await _context.SelersPersonalVideos
                                                         .AsNoTracking()
                                                         .Where(p=> !p.IsDelete && p.UserId == userInquiryResult.SellerUserId)
                                                         .Select(p=> p.BanerImage)
                                                         .FirstOrDefaultAsync(),
                    SellerPersonalVideo = await _context.SelersPersonalVideos
                                                         .AsNoTracking()
                                                         .Where(p => !p.IsDelete && p.UserId == userInquiryResult.SellerUserId)
                                                         .Select(p => p.Videos)
                                                         .FirstOrDefaultAsync(),
                };

                model.Add(modelChilds);
            }
        }

        #endregion

        return model.OrderByDescending(p => p.Price).ToList();
    }

    public async Task<List<InquiryViewModel>?> ListOfInquiryLast6(string userMacAddress, string userId)
    {
        #region Get User log

        var userInquiryResults = await _context.LogResultOfUserInquiryWithSellersInfos
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.UserId == userId && _context.LogInquiryForUsers.Any(inquiry => !inquiry.IsDelete && inquiry.Id == p.LogInquiryForUserId && inquiry.UserMAcAddress == userMacAddress))
                                               .Take(10)
                                               .ToListAsync();

        #endregion

        #region Fill Return Model

        List<InquiryViewModel> model = new List<InquiryViewModel>();

        if (userInquiryResults != null && userInquiryResults.Any())
        {
            foreach (var userInquiryResult in userInquiryResults)
            {

                var brand = await _context.MainBrands
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.BrandId)
                                               .Select(p => new brandInquiryViewModel()
                                               {
                                                   brandName = p.BrandName,
                                                   BreandLogo = p.BrandLogo
                                               })
                                               .FirstOrDefaultAsync();

                var user = await _context.Users
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete && p.Id == userInquiryResult.SellerUserId)
                                               .Select(p => new UserInquiryViewModel()
                                               {
                                                   UserAvatar = p.Avatar,
                                                   Username = p.Username,
                                               })
                                               .FirstOrDefaultAsync();

                if (brand == null || user == null) continue;

                InquiryViewModel modelChilds = new InquiryViewModel()
                {
                    BrandImage = brand.BreandLogo,
                    BrandName = brand.brandName,
                    Price = userInquiryResult.Price,
                    Score = userInquiryResult.SellerScore,
                    ShopName = userInquiryResult.SellerShopName,
                    UserAvatar = user.UserAvatar,
                    UserName = user.Username,
                    UserId = userInquiryResult.SellerUserId
                };

                model.Add(modelChilds);
            }
        }

        #endregion

        return model.OrderByDescending(p => p.Price).ToList();
    }

    //Initial Result Of User Inquiry
    public async Task<bool> InitialResultOfUserInquiry(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, string UserId, string userMacAddress, Guid? clientRequestId = null)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var userLog = await LockInquiry(userMacAddress);
        if (userLog?.GlassId == null) return false;

        if (clientRequestId.HasValue && await _context.logInquiryForUserDetails.AnyAsync(
            item => item.LogInquiryForUserId == userLog.Id && item.ClientRequestId == clientRequestId))
            return true;

        var itemCount = await _context.logInquiryForUserDetails
            .CountAsync(item => item.LogInquiryForUserId == userLog.Id && !item.IsDelete);
        if (itemCount >= 5) return false;

        #region Add Log Detail

        LogInquiryForUserDetail logDetail = new LogInquiryForUserDetail()
        {
            CreateDate = DateTime.Now,
            Height = height,
            IsDelete = false,
            LogInquiryForUserId = userLog.Id,
            ClientRequestId = clientRequestId,
            SampleId = sampleId,
            Width = width,
            KatibeSize = katibeSize,
            CountOfSample = SampleCount
        };

        await _context.logInquiryForUserDetails.AddAsync(logDetail);

        #endregion

        await _context.SaveChangesAsync();
        await RebuildInquiryResults(userLog, UserId);
        await transaction.CommitAsync();
        return true;
    }

    public async Task CompleteInquiry(string inquiryToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var inquiry = await LockInquiry(inquiryToken);
        if (inquiry == null) return;

        await RebuildInquiryResults(inquiry, inquiryToken);

        inquiry.LastCompletedAtUtc = DateTime.UtcNow;
        inquiry.LastCompletionHasQuote = await _context.LogResultOfUserInquiryWithSellersInfos
            .AnyAsync(result => !result.IsDelete && result.LogInquiryForUserId == inquiry.Id);

        var tariff = await _context.SiteSettings.AsNoTracking().Where(setting => !setting.IsDelete)
            .OrderBy(setting => setting.Id).Select(setting => setting.ChargeTariffAboutListOfInquiry).FirstOrDefaultAsync();
        var sellers = await _context.LogResultOfUserInquiryWithSellersInfos.AsNoTracking()
            .Where(result => !result.IsDelete && result.LogInquiryForUserId == inquiry.Id
                && !_context.InquirySellerExposures.Any(exposure => exposure.InquiryId == inquiry.Id && exposure.SellerId == result.SellerUserId))
            .Select(result => result.SellerUserId).Distinct().OrderBy(id => id).ToListAsync();

        foreach (var sellerId in sellers)
        {
            var market = await _context.Market.Where(item => !item.IsDelete && item.UserId == sellerId
                    && item.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)
                .OrderByDescending(item => item.Id).Select(item => (ulong?)item.Id).FirstOrDefaultAsync();
            if (market == null) continue;

            var amount = Math.Max(0, tariff);
            if (amount > 0)
            {
                var updated = await _context.Market.Where(item => item.Id == market && !item.IsDelete)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(item => item.ActivationTariff, item => item.ActivationTariff + amount));
                if (updated != 1) continue;
            }

            _context.InquirySellerExposures.Add(new InquirySellerExposure
            {
                InquiryId = inquiry.Id,
                SellerId = sellerId,
                CreatedAtUtc = DateTime.UtcNow,
                Tariff = amount
            });
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private async Task RebuildInquiryResults(LogInquiryForUser inquiry, string resultKey)
    {
        await _context.LogResultOfUserInquiryWithSellersInfos
            .Where(result => result.LogInquiryForUserId == inquiry.Id)
            .ExecuteDeleteAsync();

        if (!inquiry.GlassId.HasValue) return;

        var items = await _context.logInquiryForUserDetails.AsNoTracking()
            .Where(item => !item.IsDelete && item.LogInquiryForUserId == inquiry.Id)
            .ToListAsync();
        if (items.Count == 0 || items.Any(item => item.SampleId == null || item.Height is null or <= 0 || item.Width is null or <= 0 || item.CountOfSample <= 0)) return;

        var sampleIds = items.Select(item => item.SampleId!.Value).Distinct().ToArray();
        var activeSamples = await _context.Samples.AsNoTracking()
            .Where(sample => !sample.IsDelete && sampleIds.Contains(sample.Id)).Select(sample => sample.Id).ToListAsync();
        if (activeSamples.Count != sampleIds.Length) return;

        var sellerIds = await _context.MarketPersonalInfo.AsNoTracking()
            .Where(info => !info.IsDelete && info.CityId == inquiry.CityId && info.CountryId == inquiry.CountryId
                && info.StateId == inquiry.StateId && !info.Market.IsDelete
                && info.Market.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)
            .Select(info => info.UserId).Distinct().ToListAsync();
        var brandIds = await _context.MainBrands.AsNoTracking().Where(brand => !brand.IsDelete
            && (inquiry.BrandId != null ? brand.Id == inquiry.BrandId
                : (brand.UPVC && (inquiry.SellerType == SellerType.UPC || inquiry.SellerType == SellerType.UPVCAlminium))
                || (brand.Alominum && (inquiry.SellerType == SellerType.Aluminium || inquiry.SellerType == SellerType.UPVCAlminium))))
            .Select(brand => brand.Id).ToListAsync();

        var rates = await _context.SegmentPricings.AsNoTracking()
            .Where(rate => !rate.IsDelete && !rate.Product.IsDelete && sellerIds.Contains(rate.Product.UserId)
                && brandIds.Contains(rate.Product.MainBrandId))
            .Select(rate => new { rate.Product.UserId, rate.Product.MainBrandId, Rate = rate })
            .ToListAsync();
        var bySellerBrand = rates.ToLookup(row => (row.UserId, row.MainBrandId), row => row.Rate);
        var glassRates = await _context.GlassPricings.AsNoTracking()
            .Where(rate => !rate.IsDelete && rate.GlassId == inquiry.GlassId && !rate.Glass.IsDelete && sellerIds.Contains(rate.UserId))
            .OrderByDescending(rate => rate.Id).ToListAsync();
        var glassBySeller = glassRates.ToLookup(rate => rate.UserId);
        var requiredSegments = (await _context.SampleSelectedSegments.AsNoTracking()
            .Where(part => !part.IsDelete && sampleIds.Contains(part.SampleId))
            .Select(part => new { part.SampleId, part.SegmentId }).ToListAsync())
            .ToLookup(part => part.SampleId, part => part.SegmentId);
        var markets = (await _context.Market.AsNoTracking().Where(market => !market.IsDelete && sellerIds.Contains(market.UserId))
            .OrderByDescending(market => market.Id).ToListAsync()).ToLookup(market => market.UserId);
        var scores = (await _context.CalculatedSellersScores.AsNoTracking().Where(score => !score.IsDelete && sellerIds.Contains(score.UserSellerId))
            .OrderByDescending(score => score.Id).ToListAsync()).ToLookup(score => score.UserSellerId);

        foreach (var sellerId in sellerIds)
        {
            var glassPrice = glassBySeller[sellerId].FirstOrDefault()?.Price;
            if (glassPrice is null or <= 0) continue;

            foreach (var brandId in brandIds)
            {
                var segmentRates = bySellerBrand[(sellerId, brandId)].GroupBy(rate => rate.SegmentId)
                    .Select(group => group.OrderByDescending(rate => rate.Id).First()).Where(rate => rate.Price > 0).ToList();
                if (segmentRates.Count == 0) continue;
                var pricedSegments = segmentRates.Select(rate => rate.SegmentId).ToHashSet();
                if (items.Any(item => requiredSegments[item.SampleId!.Value].Any(segment => !pricedSegments.Contains(segment)))) continue;

                var prices = items.Select(item => CalculateSamplePrice(item.SampleId!.Value, item.Height!.Value, item.Width!.Value,
                    item.CountOfSample, item.KatibeSize, segmentRates, glassPrice.Value)).ToArray();
                if (prices.Any(price => !price.HasValue || !double.IsFinite(price.Value) || price.Value <= 0)) continue;

                _context.LogResultOfUserInquiryWithSellersInfos.Add(new LogResultOfUserInquiryWithSellersInfo
                {
                    BrandId = brandId,
                    CreateDate = DateTime.Now,
                    LogInquiryForUserId = inquiry.Id,
                    SellerShopName = markets[sellerId].FirstOrDefault()?.MarketName,
                    SellerUserId = sellerId,
                    UserId = resultKey,
                    SellerScore = scores[sellerId].FirstOrDefault()?.CalculatedScore ?? 0,
                    Price = prices.Sum(price => price!.Value)
                });
            }
        }

        await _context.SaveChangesAsync();
    }


    private static double? CalculateSamplePrice(ulong samplesId, int incomeheight, int incomewidth,
        int productCount, int? katibeSizes, List<SegmentPricing> userSegments, int glassPricing)
    {
        double height = incomeheight / 100d;
        double width = incomewidth / 100d;
        double? katibeSize = katibeSizes.GetValueOrDefault() / 100d;
        double totalPrice = 0;
        var rates = userSegments.ToDictionary(rate => rate.SegmentId);
        var hasMissingRate = false;

        SegmentPricing? GetRate(ulong segmentId)
        {
            if (rates.TryGetValue(segmentId, out var rate)) return rate;
            hasMissingRate = true;
            return null;
        }

        #region pricing & Get Samples

        #region پنجره لولایی آلومینیومی فیکس 

        if (samplesId == 10)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(50) != null)
            {
                //قیمت لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(41) != null)
            {
                // یراق دریچه آلومینیومی
                totalPrice = totalPrice + ((GetRate(41)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(50) != null)
            {
                //قیمت لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + ((GetRate(40)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (width + (2 * height)) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + (height) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (height) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + ((GetRate(40)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + ((2 * height)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + ((2 * height)) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + ((GetRate(40)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (width + (4 * height)) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + ((3 * height)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + ((3 * height)) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (2 * ((GetRate(40)?.Price ?? 0)));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم لولایی آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(50) != null)
            {
                //لنگه پنجره لولایی آلومینیومی
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * height)) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + (5 * ((height)) * ((GetRate(55)?.Price ?? 0)));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی
                totalPrice = totalPrice + (5 * ((height)) * (2 * ((GetRate(47)?.Price ?? 0))));
            }

            if (GetRate(40) != null)
            {
                //یراق پنجره لولایی تک حالته آلومینیومی
                totalPrice = totalPrice + (2 * ((GetRate(40)?.Price ?? 0)));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                //قیمت لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(14) != null)
            {
                //یراق دریچه لولایی upvc
                totalPrice = totalPrice + ((GetRate(14)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                //قیمت لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + ((GetRate(13)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                //لنگه لولایی upvc
                totalPrice = totalPrice + (width + (2 * height)) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (width + (2 * height)) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + (height) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (height) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + (height) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + ((GetRate(13)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                //لنگه ی لنگه لولایی upvc
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + ((2 * height)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + ((2 * height)) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + ((2 * height)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + ((GetRate(13)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                //لنگه لولایی upvc
                totalPrice = totalPrice + ((width) + (4 * height)) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + ((width) + (4 * height)) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + ((3 * height)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc
                totalPrice = totalPrice + ((3 * height)) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + ((3 * height)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (2 * ((GetRate(13)?.Price ?? 0)));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره  لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                //لنگه لولایی upvc
                totalPrice = totalPrice + ((2 * width / 3) + (4 * height)) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                //گالوانیزه لنگه لولایی upvc
                totalPrice = totalPrice + ((2 * width / 3) + (4 * height)) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی upvc
                totalPrice = totalPrice + (5 * ((height)) * ((GetRate(33)?.Price ?? 0)));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (5 * ((height)) * (2 * ((GetRate(30)?.Price ?? 0))));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + (5 * ((height)) * ((GetRate(20)?.Price ?? 0)));
            }

            if (GetRate(13) != null)
            {
                //یراق تک حالته لولایی upvc
                totalPrice = totalPrice + (2 * ((GetRate(13)?.Price ?? 0)));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(31)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(18)?.Price ?? 0));
            }

            if (GetRate(11) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(11)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(31)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(18)?.Price ?? 0));
            }

            if (GetRate(10) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + ((GetRate(10)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(31)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(18)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه مولیون لولایی upvc
                totalPrice = totalPrice + (width) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(22) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * ((GetRate(22)?.Price ?? 0));
            }

            if (GetRate(11) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(11)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(31)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(18)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه ی لولایی
                totalPrice = totalPrice + (width) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(22) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)120 / (double)100)) * ((GetRate(22)?.Price ?? 0));
            }

            if (GetRate(10) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + ((GetRate(10)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (height) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه ی مولیون لولایی
                totalPrice = totalPrice + (height) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (height) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * ((GetRate(31)?.Price ?? 0));
            }

            if (GetRate(18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * ((GetRate(18)?.Price ?? 0));
            }

            if (GetRate(11) != null)
            {
                //یراق درب سوییچی
                totalPrice = totalPrice + ((GetRate(11)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width + height) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه ی مولیون لولایی
                totalPrice = totalPrice + (width + height) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width + height) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(31) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * ((GetRate(31)?.Price ?? 0));
            }

            if (GetRate(18) != null)
            {
                //گالوانیزه ی دربی
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * ((GetRate(18)?.Price ?? 0));
            }

            if (GetRate(22) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * ((GetRate(22)?.Price ?? 0));
            }

            if (GetRate(11) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(11)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(49)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(38)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(49)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(37) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + ((GetRate(37)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(49)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(48) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * ((GetRate(48)?.Price ?? 0));
            }

            if (GetRate(38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(38)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(49)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(48) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)120 / (double)100)) * ((GetRate(48)?.Price ?? 0));
            }

            if (GetRate(37) != null)
            {
                //یراق درب سرویسی
                totalPrice = totalPrice + ((GetRate(37)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (height) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (height) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * ((GetRate(49)?.Price ?? 0));
            }

            if (GetRate(38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(38)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی
                totalPrice = totalPrice + (width + height) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //زهوار دوجداره
                totalPrice = totalPrice + (width + height) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(49) != null)
            {
                //لنگه ی درب
                totalPrice = totalPrice + (2 * (katibeSize.Value + height)) * ((GetRate(49)?.Price ?? 0));
            }

            if (GetRate(48) != null)
            {
                //پنل
                totalPrice = totalPrice + (width * ((double)60 / (double)100)) * ((GetRate(48)?.Price ?? 0));
            }

            if (GetRate(38) != null)
            {
                //یراق درب سویئچی
                totalPrice = totalPrice + ((GetRate(38)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(30)?.Price ?? 0));
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
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(30)?.Price ?? 0));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(14) != null)
            {
                // -یراق دریچه لولایی upvc  
                totalPrice = totalPrice + ((GetRate(14)).Price);
            }

        }

        #endregion

        #region پنجره لولایی تک لنگه کتیبه دار یو پی وی سی (id = 25)

        if (samplesId == 25)
        {
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * ((width)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(13) != null)
            {
                // -یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + ((GetRate(13)).Price);
            }

        }

        #endregion

        #region پنجره لولایی پنجره لولایی دولنگه کتیبه دار یو پی وی سی (id = 24)

        if (samplesId == 24)
        {
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + ((width) + (2 * ((height - katibeSize.Value)))) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + ((width) + (2 * ((height - katibeSize.Value)))) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width) + (height - katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width) + (height - katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * ((width) + (height - katibeSize.Value)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + ((GetRate(13)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی سه کتیبه دار یو پی وی سی (id = 23)

        if (samplesId == 23)
        {
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * ((height - katibeSize.Value)))) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * ((height - katibeSize.Value)))) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 2 * ((height - katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 2 * ((height - katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 2 * (2 * ((height - katibeSize.Value)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + (width + (katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + ((GetRate(13)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی چهار کتیبه دار یو پی وی سی (id = 22)

        if (samplesId == 22)
        {
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + ((width) + (4 * ((height - katibeSize.Value)))) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + ((width) + (4 * ((height - katibeSize.Value)))) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 3 * ((height - katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 3 * ((height - katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 3 * (2 * ((height - katibeSize.Value)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + (width + (katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + ((width + (katibeSize.Value)) * 2 * ((GetRate(30)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + 2 * (((GetRate(13)).Price));
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی شش کتیبه دار یو پی وی سی (id = 47)

        if (samplesId == 47)
        {
            if (GetRate(34) != null)
            {
                //قیمت فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //قیمت گالوانیزه فریم لولایی upvc
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(32) != null)
            {
                // لنگه لولایی upvc  
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * ((height - katibeSize.Value)))) * ((GetRate(32)?.Price ?? 0));
            }

            if (GetRate(19) != null)
            {
                // گالوانیزه لنگه لولایی upvc   
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * ((height - katibeSize.Value)))) * ((GetRate(19)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + ((width)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + ((width)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + (2 * ((width)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 5 * ((height - katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 5 * ((height - katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 5 * (2 * ((height - katibeSize.Value)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (GetRate(33) != null)
            {
                //قیمت مولیون لولایی upvc
                totalPrice = totalPrice + 3 * ((katibeSize.Value)) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //قیمت گالوانیزه ی مولیون لولایی upvc  
                totalPrice = totalPrice + 3 * ((katibeSize.Value)) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره لولایی upvc  
                totalPrice = totalPrice + 3 * (2 * ((katibeSize.Value)) * ((GetRate(30)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(13) != null)
            {
                // یراق تک حالته لولایی upvc  
                totalPrice = totalPrice + 2 * (((GetRate(13)).Price));
            }
        }

        #endregion

        #endregion

        #region پنجره ی لولایی کتیبه دار آلمینیوم 

        #region پنجره ی فیکس لولایی کتیبه دار آلمینیوم (id = 54)

        if (samplesId == 54)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی
                totalPrice = totalPrice + ((width)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (4 * (width)) * ((GetRate(54)?.Price ?? 0));
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
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + ((width)) * (4 * ((GetRate(54)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(41) != null)
            {
                // یراق درسچه آلومینیومی   
                totalPrice = totalPrice + ((GetRate(41)).Price);
            }

        }

        #endregion

        #region پنجره لولایی تک لنگه کتیبه دار آلمینیوم (id = 52)

        if (samplesId == 52)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + 2 * ((width) + (((height - katibeSize.Value)))) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + ((width)) * (4 * ((GetRate(54)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی  
                totalPrice = totalPrice + ((GetRate(40)).Price);
            }

        }

        #endregion

        #region پنجره لولایی پنجره لولایی دولنگه کتیبه دار آلمینیوم (id = 51)

        if (samplesId == 51)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + ((width) + (2 * ((height - katibeSize.Value)))) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width) + (height - katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + (2 * ((width) + (height - katibeSize.Value)) * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (4 * ((width) + (height - katibeSize.Value)) * ((GetRate(54)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + ((GetRate(40)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی سه لنگه کتیبه دار آلمینیوم (id = 50)

        if (samplesId == 50)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی 
                totalPrice = totalPrice + (((2 * width) / 3) + (2 * ((height - katibeSize.Value)))) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 2 * ((height - katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + 2 * (2 * ((height - katibeSize.Value)) * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 2 * (4 * ((height - katibeSize.Value)) * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + (width + (katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (4 * ((GetRate(54)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + ((GetRate(40)).Price);
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی چهار لنگه کتیبه دار آلمینیوم (id = 49)

        if (samplesId == 49)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی   
                totalPrice = totalPrice + ((width) + (4 * ((height - katibeSize.Value)))) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 3 * ((height - katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + 3 * (2 * ((height - katibeSize.Value)) * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 3 * (4 * ((height - katibeSize.Value)) * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + (width + (katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی    
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (2 * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک    
                totalPrice = totalPrice + (width + (katibeSize.Value)) * (4 * ((GetRate(54)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + 2 * (((GetRate(40)).Price));
            }
        }

        #endregion

        #region پنجره لولایی پنجره لولایی شش لنگه کتیبه دار آلمینیوم (id = 48)

        if (samplesId == 48)
        {
            if (GetRate(52) != null)
            {
                //فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + (2 * (width + height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(50) != null)
            {
                // لنگه پنجره لولایی آلومینیومی   
                totalPrice = totalPrice + (((2 * width) / 3) + (4 * ((height - katibeSize.Value)))) * ((GetRate(50)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + ((width)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + ((width)) * 2 * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه 
                totalPrice = totalPrice + ((width)) * 4 * ((GetRate(54)?.Price ?? 0));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 5 * ((height - katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + 5 * (2 * ((height - katibeSize.Value)) * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 5 * (4 * ((height - katibeSize.Value)) * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(55) != null)
            {
                //مولیون لولایی آلومینیومی 
                totalPrice = totalPrice + 3 * ((katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                //قیمت زهوار دوجداره آلومینیومی   
                totalPrice = totalPrice + 3 * (2 * ((katibeSize.Value)) * ((GetRate(47)?.Price ?? 0)));
            }

            if (GetRate(54) != null)
            {
                //قیمت لاستیک فشاری شیشه  
                totalPrice = totalPrice + 3 * (4 * ((katibeSize.Value)) * ((GetRate(54)?.Price ?? 0)));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(40) != null)
            {
                // یراق پنجره لولایی تک حالته آلومینیومی 
                totalPrice = totalPrice + 2 * (((GetRate(40)).Price));
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

            if (GetRate(46) != null)
            {
                // فریم کشویی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * ((GetRate(46)?.Price ?? 0));
            }

            if (GetRate(45) != null)
            {
                // لنگه کشویی آلومینیومی 
                totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * ((GetRate(45)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                // نوار مویی
                totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(44) != null)
            {
                // اینترلاک
                totalPrice = totalPrice + ((2 * (height - katibeSize.Value))) * (((GetRate(44)?.Price ?? 0)));
            }

            if (GetRate(43) != null)
            {
                // ریل کشویی آلومینیومی 
                totalPrice = totalPrice + (width) * (((GetRate(43)?.Price ?? 0)));
            }

            if (GetRate(52) != null)
            {
                // فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (GetRate(54)).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(39) != null)
            {
                //قیمت یراق کشویی پنجره آلومینیومی 
                totalPrice = totalPrice + ((GetRate(39)).Price);
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

            if (GetRate(46) != null)
            {
                // فریم کشویی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * ((GetRate(46)?.Price ?? 0));
            }

            if (GetRate(45) != null)
            {
                // لنگه کشویی آلومینیومی 
                totalPrice = totalPrice + (((4 * width) / 3) + (4 * (height - katibeSize.Value))) * ((GetRate(45)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (((4 * width) / 3) + (4 * (height - katibeSize.Value))) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                // نوار مویی
                totalPrice = totalPrice + (((4 * width) / 3) + (4 * (height - katibeSize.Value))) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(44) != null)
            {
                // اینترلاک
                totalPrice = totalPrice + ((4 * (height - katibeSize.Value))) * (((GetRate(44)?.Price ?? 0)));
            }

            if (GetRate(43) != null)
            {
                // ریل کشویی آلومینیومی 
                totalPrice = totalPrice + (width) * (((GetRate(43)?.Price ?? 0)));
            }

            if (GetRate(52) != null)
            {
                // فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (GetRate(54)).Price);
            }

            if (GetRate(55) != null)
            {
                // مولیون لولایی آلمینیوم 
                totalPrice = totalPrice + ((katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + ((katibeSize.Value)) * ((2) * (GetRate(47)).Price);
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + ((katibeSize.Value)) * (4 * (GetRate(54)).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(39) != null)
            {
                //قیمت یراق کشویی پنجره آلومینیومی 
                totalPrice = totalPrice + (2 * (GetRate(39)).Price);
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

            if (GetRate(46) != null)
            {
                // فریم کشویی آلومینیومی 
                totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * ((GetRate(46)?.Price ?? 0));
            }

            if (GetRate(45) != null)
            {
                // لنگه کشویی آلومینیومی 
                totalPrice = totalPrice + ((2 * width) + (8 * (height - katibeSize.Value))) * ((GetRate(45)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + ((2 * width) + (8 * (height - katibeSize.Value))) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                // نوار مویی
                totalPrice = totalPrice + ((2 * width) + (8 * (height - katibeSize.Value))) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(44) != null)
            {
                // اینترلاک
                totalPrice = totalPrice + ((4 * (height - katibeSize.Value))) * (((GetRate(44)?.Price ?? 0)));
            }

            if (GetRate(43) != null)
            {
                // ریل کشویی آلومینیومی 
                totalPrice = totalPrice + (width) * (((GetRate(43)?.Price ?? 0)));
            }

            if (GetRate(42) != null)
            {
                // قفل چهار لنگه 
                totalPrice = totalPrice + (height - katibeSize.Value) * (((GetRate(42)?.Price ?? 0)));
            }

            if (GetRate(52) != null)
            {
                // فریم لولایی آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * ((GetRate(52)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * ((GetRate(47)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (GetRate(54)).Price);
            }

            if (GetRate(55) != null)
            {
                // مولیون لولایی آلمینیوم 
                totalPrice = totalPrice + (3 * (katibeSize.Value)) * ((GetRate(55)?.Price ?? 0));
            }

            if (GetRate(47) != null)
            {
                // قیمت زهوار دوجداره آلومینیومی 
                totalPrice = totalPrice + (3 * (katibeSize.Value)) * (2 * (GetRate(47)).Price);
            }

            if (GetRate(54) != null)
            {
                // لاستیک فشاري
                totalPrice = totalPrice + (3 * (katibeSize.Value)) * (4 * (GetRate(54)).Price);
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(39) != null)
            {
                //قیمت یراق کشویی پنجره آلومینیومی 
                totalPrice = totalPrice + (2 * (GetRate(39)).Price);
            }
        }

        #endregion

        #endregion

        #region پنجره ھای کشویی کتیبھدار UPVC

        #region پنجره کشویی دو لنگه کتیبھدار UPVC

        if (samplesId == 55)
        {
            if (GetRate(29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(29)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(56) != null)
            {
                //کتیبه ی کشویی
                totalPrice = totalPrice + (width) * ((GetRate(56)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم کشویی
                totalPrice = totalPrice + (width) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(34) != null)
            {
                //فریم لولایی 
                totalPrice = totalPrice + (height - katibeSize.Value) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم لولایی  
                totalPrice = totalPrice + (height - katibeSize.Value) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * (height - katibeSize.Value))) * ((GetRate(27)?.Price ?? 0));
            }

            if (GetRate(15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * (height - katibeSize.Value))) * ((GetRate(15)?.Price ?? 0));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (width + (2 * (height - katibeSize.Value))) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(24) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * ((GetRate(24)?.Price ?? 0));
            }

            if (GetRate(23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + ((height - katibeSize.Value) + width) * ((GetRate(23)?.Price ?? 0));
            }

            if (GetRate(8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * ((GetRate(8)?.Price ?? 0));
            }

            if (GetRate(12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + ((GetRate(12)?.Price ?? 0));
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
            if (GetRate(29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(29)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(56) != null)
            {
                //کتیبه ی کشویی
                totalPrice = totalPrice + (width) * ((GetRate(56)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم کشویی
                totalPrice = totalPrice + (width) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //زهوار دوجدار لولایی 
                totalPrice = totalPrice + (katibeSize.Value) * 2 * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه ی مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(34) != null)
            {
                //فریم لولایی 
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم لولایی  
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (2 * (height - katibeSize.Value))) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (2 * (((2 * width) / 3) + (2 * (height - katibeSize.Value)))) * ((GetRate(27)?.Price ?? 0));
            }

            if (GetRate(15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (2 * (((2 * width) / 3) + (2 * (height - katibeSize.Value)))) * ((GetRate(15)?.Price ?? 0));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (2 * (((2 * width) / 3) + (2 * (height - katibeSize.Value)))) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(24) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + (4 * (height - katibeSize.Value)) * ((GetRate(24)?.Price ?? 0));
            }

            if (GetRate(23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + ((2 * width) / 3) * ((GetRate(23)?.Price ?? 0));
            }

            if (GetRate(8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * ((GetRate(8)?.Price ?? 0));
            }

            if (GetRate(12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + 2 * ((GetRate(12)?.Price ?? 0));
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
            if (GetRate(29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(29)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(56) != null)
            {
                //کتیبه ی کشویی
                totalPrice = totalPrice + (width) * ((GetRate(56)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم کشویی
                totalPrice = totalPrice + (width) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //زهوار دوجدار لولایی 
                totalPrice = totalPrice + (katibeSize.Value) * 6 * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(20) != null)
            {
                //گالوانیزه ی مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * 3 * ((GetRate(20)?.Price ?? 0));
            }

            if (GetRate(33) != null)
            {
                //مولیون لولایی کتیبه 
                totalPrice = totalPrice + (katibeSize.Value) * 3 * ((GetRate(33)?.Price ?? 0));
            }

            if (GetRate(34) != null)
            {
                //فریم لولایی 
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * ((GetRate(34)?.Price ?? 0));
            }

            if (GetRate(21) != null)
            {
                //گالوانیزه ی فریم لولایی  
                totalPrice = totalPrice + (2 * (height - katibeSize.Value)) * ((GetRate(21)?.Price ?? 0));
            }

            if (GetRate(30) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (2 * (height - katibeSize.Value))) * ((GetRate(30)?.Price ?? 0));
            }

            if (GetRate(27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (width + (4 * (height - katibeSize.Value))) * ((GetRate(27)?.Price ?? 0));
            }

            if (GetRate(15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (width + (4 * (height - katibeSize.Value))) * ((GetRate(15)?.Price ?? 0));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (width + (4 * (height - katibeSize.Value))) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(24) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + (4 * (height - katibeSize.Value)) * ((GetRate(24)?.Price ?? 0));
            }

            if (GetRate(23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + (width) * ((GetRate(23)?.Price ?? 0));
            }

            if (GetRate(8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * ((GetRate(8)?.Price ?? 0));
            }

            if (GetRate(36) != null)
            {
                //قفل چهارلنگه
                totalPrice = totalPrice + (height - katibeSize.Value) * ((GetRate(36)?.Price ?? 0));
            }

            if (GetRate(12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + 2 * ((GetRate(12)?.Price ?? 0));
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
            if (GetRate(29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(29)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * height)) * ((GetRate(27)?.Price ?? 0));
            }

            if (GetRate(15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + (width + (2 * height)) * ((GetRate(15)?.Price ?? 0));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + (width + (2 * height)) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(24) != null)
            {
                //کاور لنگه ی کشویی
                totalPrice = totalPrice + (height) * ((GetRate(24)?.Price ?? 0));
            }

            if (GetRate(28) != null)
            {
                //مولوین کشویی
                totalPrice = totalPrice + (height) * ((GetRate(28)?.Price ?? 0));
            }

            if (GetRate(16) != null)
            {
                //گالوانیزه ی مولوین کشویی
                totalPrice = totalPrice + (height) * ((GetRate(16)?.Price ?? 0));
            }

            if (GetRate(25) != null)
            {
                //کاور مولوین کشویی
                totalPrice = totalPrice + (height) * ((GetRate(25)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (height) * (2 * ((GetRate(26)?.Price ?? 0)));
            }

            if (GetRate(23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + (height + width) * ((GetRate(23)?.Price ?? 0));
            }

            if (GetRate(8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * ((GetRate(8)?.Price ?? 0));
            }

            if (GetRate(12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + ((GetRate(12)?.Price ?? 0));
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
            if (GetRate(29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(29)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * ((GetRate(27)?.Price ?? 0)));
            }

            if (GetRate(15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * ((GetRate(15)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + 2 * ((((2 * width) / 3) + (2 * height)) * (2 * ((GetRate(53)?.Price ?? 0))));
            }

            if (GetRate(24) != null)
            {
                //کاور لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(24)?.Price ?? 0)));
            }

            if (GetRate(28) != null)
            {
                //مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(28)?.Price ?? 0)));
            }

            if (GetRate(16) != null)
            {
                //گالوانیزه ی مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(16)?.Price ?? 0)));
            }

            if (GetRate(25) != null)
            {
                //کاور مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(25)?.Price ?? 0)));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + 2 * ((height) * (2 * ((GetRate(26)?.Price ?? 0))));
            }

            if (GetRate(23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + ((2 * width) / 3) * ((GetRate(23)?.Price ?? 0));
            }

            if (GetRate(8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * ((GetRate(8)?.Price ?? 0));
            }

            if (GetRate(12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (2 * ((GetRate(12)?.Price ?? 0)));
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
            if (GetRate(29) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(29)?.Price ?? 0));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(26)?.Price ?? 0));
            }

            if (GetRate(17) != null)
            {
                //گالوانیزه ی فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(17)?.Price ?? 0));
            }

            if (GetRate(27) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * ((GetRate(27)?.Price ?? 0)));
            }

            if (GetRate(15) != null)
            {
                //گالوانیزه ی لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * ((GetRate(15)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + 2 * ((((width) / 2) + (2 * height)) * (2 * ((GetRate(53)?.Price ?? 0))));
            }

            if (GetRate(24) != null)
            {
                //کاور لنگه ی کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(24)?.Price ?? 0)));
            }

            if (GetRate(28) != null)
            {
                //مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(28)?.Price ?? 0)));
            }

            if (GetRate(16) != null)
            {
                //گالوانیزه ی مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(16)?.Price ?? 0)));
            }

            if (GetRate(25) != null)
            {
                //کاور مولوین کشویی
                totalPrice = totalPrice + 2 * ((height) * ((GetRate(25)?.Price ?? 0)));
            }

            if (GetRate(26) != null)
            {
                //قیمت زهوار دوجداره
                totalPrice = totalPrice + 2 * ((height) * (2 * ((GetRate(26)?.Price ?? 0))));
            }

            if (GetRate(23) != null)
            {
                //کاور بارانگیر
                totalPrice = totalPrice + (width) * ((GetRate(23)?.Price ?? 0));
            }

            if (GetRate(8) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + (width) * ((GetRate(8)?.Price ?? 0));
            }

            if (GetRate(12) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (2 * ((GetRate(12)?.Price ?? 0)));
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
            if (GetRate(46) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(46)?.Price ?? 0));
            }

            if (GetRate(45) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + ((2 * width) + (4 * height)) * ((GetRate(45)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //لاستیک فشاری
                totalPrice = totalPrice + ((2 * width) + (4 * height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + ((2 * width) + (4 * height)) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(44) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + ((2 * height)) * ((GetRate(44)?.Price ?? 0));
            }

            if (GetRate(43) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + ((width)) * ((GetRate(43)?.Price ?? 0));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(39) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + ((GetRate(39)?.Price ?? 0));
            }

        }

        #endregion

        #region پنجره ی کشویی آلمینیومی سه لنگه  (id = 59)

        if (samplesId == 59)
        {
            if (GetRate(46) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(46)?.Price ?? 0));
            }

            if (GetRate(45) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + ((2 * width) + (6 * height)) * ((GetRate(45)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //لاستیک فشاری
                totalPrice = totalPrice + ((2 * width) + (6 * height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + ((2 * width) + (6 * height)) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(44) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + ((4 * height)) * ((GetRate(44)?.Price ?? 0));
            }

            if (GetRate(43) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + ((width)) * ((GetRate(43)?.Price ?? 0));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(39) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + ((GetRate(39)?.Price ?? 0));
            }
        }

        #endregion

        #region پنجره ی کشویی آلمینیومی چهار لنگه  (id = 60)

        if (samplesId == 60)
        {
            if (GetRate(46) != null)
            {
                //قیمت فریم
                totalPrice = totalPrice + (2 * (width + height)) * ((GetRate(46)?.Price ?? 0));
            }

            if (GetRate(45) != null)
            {
                //لنگه ی کشویی
                totalPrice = totalPrice + ((2 * width) + (8 * height)) * ((GetRate(45)?.Price ?? 0));
            }

            if (GetRate(54) != null)
            {
                //لاستیک فشاری
                totalPrice = totalPrice + ((2 * width) + (8 * height)) * (2 * ((GetRate(54)?.Price ?? 0)));
            }

            if (GetRate(53) != null)
            {
                //نوار مویی
                totalPrice = totalPrice + ((2 * width) + (8 * height)) * (2 * ((GetRate(53)?.Price ?? 0)));
            }

            if (GetRate(44) != null)
            {
                //اینترلاک
                totalPrice = totalPrice + ((4 * height)) * ((GetRate(44)?.Price ?? 0));
            }

            if (GetRate(43) != null)
            {
                //ریل کشویی
                totalPrice = totalPrice + ((width)) * ((GetRate(43)?.Price ?? 0));
            }

            if (GetRate(42) != null)
            {
                //قفل چهار لنگه
                totalPrice = totalPrice + ((height)) * ((GetRate(42)?.Price ?? 0));
            }

            if (glassPricing != null)
            {
                //قیمت شیشه
                totalPrice = totalPrice + (glassPricing * (width * height));
            }

            if (GetRate(39) != null)
            {
                //یراق کشویی
                totalPrice = totalPrice + (2 * ((GetRate(39)?.Price ?? 0)));
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

        return hasMissingRate ? null : totalPrice;
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

        #region Calculate Seller Score

        var score = await CalculateSellerScore(model.SellerId);

        //Get Seller Calculated Score 
        var calculatedScore = await _context.CalculatedSellersScores
                                            .FirstOrDefaultAsync(p => !p.IsDelete && p.UserSellerId == model.SellerId);

        if (calculatedScore == null)
        {
            CalculatedSellersScore calScore = new CalculatedSellersScore()
            {
                CalculatedScore = score,
                UserSellerId = model.SellerId
            };

            await _context.CalculatedSellersScores.AddAsync(calScore);
            await _context.SaveChangesAsync();
        }
        else
        {
            calculatedScore.CalculatedScore = score;

            _context.CalculatedSellersScores.Update(calculatedScore);
            await _context.SaveChangesAsync();
        }

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
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var inquiry = await LockInquiry(macAddress);
        if (inquiry == null) return false;
        #region Get User Lastest Inquiry Detail

        var lastestUserInquiryDetail = await _context.logInquiryForUserDetails.Include(p => p.LogInquiryForUser)
                                                .FirstOrDefaultAsync(p => !p.IsDelete && p.LogInquiryForUserId == inquiry.Id
                                                                        && p.Id == inquiryDetailId);

        if (lastestUserInquiryDetail == null)
        {
            return false;
        }

        #endregion

        #region Remove User Inquiry Detail

        lastestUserInquiryDetail.IsDelete = true;
        await _context.SaveChangesAsync();
        await RebuildInquiryResults(inquiry, macAddress);
        await transaction.CommitAsync();

        #endregion

        return true;
    }

    #endregion

    #region Admin side

    public async Task<(int WithQuote, int WithoutQuote)> CountRecordedCompletions()
    {
        var counts = await _context.LogInquiryForUsers.AsNoTracking()
            .Where(inquiry => !inquiry.IsDelete && inquiry.LastCompletionHasQuote != null)
            .GroupBy(inquiry => inquiry.LastCompletionHasQuote!.Value)
            .Select(group => new { HasQuote = group.Key, Count = group.Count() }).ToListAsync();

        return (counts.FirstOrDefault(count => count.HasQuote)?.Count ?? 0,
            counts.FirstOrDefault(count => !count.HasQuote)?.Count ?? 0);
    }

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
