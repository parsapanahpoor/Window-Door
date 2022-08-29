using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Inquiry;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Entities.Sample;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Services
{
    public class InquryService : IInquiryService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public InquryService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Site Side 

        //Log Inquiry For User In Step 1
        public async Task LogInquiryForUserPart1(FilterInquiryViewModel filter)
        {
            #region Check That Current User Has Any Inquiry OR Not 

            var inquiry = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == filter.UserMacAddress);

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
                    ProductKind = filter.ProductKind,
                    ProductType = filter.ProductType,
                    SellerType = filter.SellerType,
                    UserMAcAddress = filter.UserMacAddress,
                    GlassId = filter.GlassId
                };

                await _context.LogInquiryForUsers.AddAsync(log);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Edit Inquiry For This User

            if (inquiry != null)
            {
                inquiry.CountryId = filter.CountryId;
                inquiry.CityId = filter.CityId;
                inquiry.StateId = filter.StateId;
                inquiry.BrandId = filter.MainBrandId;
                inquiry.CreateDate = DateTime.Now;
                inquiry.IsDelete = false;
                inquiry.ProductKind = filter.ProductKind;
                inquiry.ProductType = filter.ProductType;
                inquiry.SellerType = filter.SellerType;
                inquiry.UserMAcAddress = filter.UserMacAddress;
                inquiry.GlassId = filter.GlassId;

                _context.LogInquiryForUsers.Update(inquiry);
                await _context.SaveChangesAsync();

                var logInquiryUserDetail = await _context.logInquiryForUserDetails.Where(p => !p.IsDelete && p.LogInquiryForUserId == inquiry.Id)
                                                    .ToListAsync();
                if (logInquiryUserDetail != null)
                {
                    _context.logInquiryForUserDetails.RemoveRange(logInquiryUserDetail);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion
        }

        //Log Inquiry For User In Step 2
        public async Task<bool> LogInquiryForUserPart2(ulong sampleId, int width, int height, string userMacAddress)
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
            };

            await _context.logInquiryForUserDetails.AddAsync(logDetail);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        //Update User Inqury In Last Step For Update Brand 
        public async Task<bool> UpdateUserInquryInLastStep(string userMacAddress, string brandTitle)
        {
            #region Get Brand By Title 

            var brand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.BrandName == brandTitle);
            if (brand == null) return false;

            #endregion

            #region Get User Inqury

            var userInqury = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);
            if (userInqury == null) return false;

            userInqury.BrandId = brand.Id;

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

        public async Task<int?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int height, int width, ulong userId , ulong glassId)
        {
            #region return Model 

            var totalPrice = 0;

            #endregion

            #region Get User Segments Price 

            var userSegments = await _context.SegmentPricings.Include(p => p.Product).Where(p => !p.IsDelete && p.Product.UserId == userId && p.Product.MainBrandId == brandId).ToListAsync();
            if (userSegments == null) return null;

            #endregion

            #region Get Glasses Pricing

            var glassPricing = await _context.GlassPricings.FirstOrDefaultAsync(p => !p.IsDelete && p.GlassId == glassId && p.UserId == userId);
            if (glassPricing == null) return null;

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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (width + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 6) != null)
                {
                    //لنگه ی بازشوی پنجره
                    totalPrice = totalPrice + (((2 * width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 6).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی شش لنگه ساده

            if (sample.Id == 14)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region درب لولایی درب سوییچی شیشه یکپارچه UPVC 

            if (sample.Id == 22)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = totalPrice +  (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - 60)));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 12) != null)
                {
                    //لنگه ی درب
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - 120)));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 12).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 13) != null)
                {
                    //گالوانیزه ی دربی
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 13).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - 60)));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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
                    totalPrice = totalPrice + ((2 * width) / 3) * (userSegments.FirstOrDefault(p => p.SegmentId == 26).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
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
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
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

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #endregion

            return totalPrice;
        }

        //Calculate Seller Score 
        public async Task<int> CalculateSellerScore(ulong userId)
        {
            #region MyRegion

            var sellerScores = await _context.ScoreForMarkets.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

            #endregion

            //If Score Dosent Exist Yet
            if (sellerScores == null || !sellerScores.Any())
            {
                return 0;
            }

            var score = sellerScores.Sum(p => p.Score) / sellerScores.Count();

            return score;
        }

        public async Task<List<InquiryViewModel>?> ListOfInquiry(string userMacAddress)
        {
            #region Get User log 

            var log = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p => p.UserMAcAddress == userMacAddress && !p.IsDelete);
            if (log == null) return null;

            var logDetail = await _context.logInquiryForUserDetails.Where(p => !p.IsDelete && p.LogInquiryForUserId == log.Id).ToListAsync();
            if (logDetail == null) return null;

            #endregion

            #region Initial Model 

            List<InquiryViewModel> model = new List<InquiryViewModel>();

            #endregion

            #region Get Brand 

            var brand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == log.BrandId);
            if (brand == null) return null;

            #endregion

            #region Get Sellers

            var sellers = await _context.MarketPersonalInfo.Include(p => p.User).Include(p=> p.Market)
                                    .Where(p => !p.IsDelete && p.CityId == log.CityId && p.CountryId == log.CountryId
                                    && p.StateId == log.StateId && p.SellerType == log.SellerType && p.Market.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)
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
                model2.UserId = seller.Id;
                model2.Price = 0;
                model2.Score = await CalculateSellerScore(seller.Id);

                foreach (var sample in logDetail)
                {
                    model2.Price = model2.Price + await InitialTotalSamplePrice(brand.Id, sample.SampleId.Value, sample.Width.Value, sample.Height.Value, seller.Id , log.GlassId.Value);
                }

                model.Add(model2);
            }

            #endregion

            return model;
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

        #endregion
    }
}
