using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Inquiry;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Entities.Sample;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Services
{
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
        public async Task<bool> LogInquiryForUserPart2(ulong sampleId, int width, int height, int? KatibeSize, string userMacAddress , int productCount)
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
        public async Task<bool> UpdateUserInquiryItrm(ulong inquiryDetailId , ulong sampleId , int width , int height , int? katibe , string macAddress , int? SampleCount)
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

            var userInqury = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);
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

        public async Task<double?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int incomeheight, int incomewidth , int productCount , int? katibeSizes, ulong userId, ulong glassId)
        {
            #region Proccess Height And Width

            double height = (double)incomeheight / (double)100;
            double width = (double)incomewidth / (double)100;
            double? katibeSize = 0;
            if (katibeSizes.HasValue)
            {
                katibeSize = (double)katibeSizes / (double)100;
            }

            #endregion

            #region return Model 

            double totalPrice = 0;

            #endregion

            #region Get User Segments Price 

            var userSegments = await _context.SegmentPricings.Include(p => p.Product).Where(p => !p.IsDelete && p.Product.UserId == userId && p.Product.MainBrandId == brandId).ToListAsync();
            if (userSegments == null || !userSegments.Any()) return null;

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

            #region پنجره لولایی آلومینیومی فیکس 

            if (sample.Id == 10)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی دریچه 

            if (sample.Id == 9)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  تک لنگه 

            if (sample.Id == 8)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  دولنگه 

            if (sample.Id == 34)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی سه لنگه 

            if (sample.Id == 35)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی  چهار لنگه 

            if (sample.Id == 36)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی آلومینیومی لولایی شش لنگه 

            if (sample.Id == 37)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (6 * ((height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 55).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 47) != null)
                {
                    //قیمت زهوار دوجداره آلومینیومی
                    totalPrice = totalPrice + (6 * ((height)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 47).Price)));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 40) != null)
                {
                    //یراق پنجره لولایی تک حالته آلومینیومی
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 40).Price));
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC فیکس 

            if (sample.Id == 33)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC دریچه

            if (sample.Id == 32)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC لولایی تک لنگه

            if (sample.Id == 31)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC لولایی  دولنگه

            if (sample.Id == 30)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC لولایی سه لنگه

            if (sample.Id == 29)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (((2* width) / 3) + (2 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC لولایی  چهار لنگه

            if (sample.Id == 28)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region پنجره لولایی UPVC لولایی  شش لنگه 

            if (sample.Id == 21)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
                {
                    //قیمت فریم لولایی upvc
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 34).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    //قیمت زهوار دوجداره  لولایی upvc  
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 302).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    //گالوانیزه فریم لولایی upvc
                    totalPrice = totalPrice + (2 * (width + height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
                {
                    //لنگه لولایی upvc
                    totalPrice = totalPrice + (( 2 * width / 3) + (4 * height)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
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
                    totalPrice = totalPrice + (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 5).Price));
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region درب لولایی سوییچی شیشه یکپارچه UPVC (id = 20)

            if (sample.Id == 20)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region  درب لولایی سرویسی شیشه یکپارچه UPVC  (id = 19)

            if (sample.Id == 19)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region  درب لولایی  بالکنی سوویچی UPVC (id = 18)

            if (sample.Id == 18)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (width * (double)(60 / 100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - (double)(60 / 100))));
                }
            }

            #endregion

            #region   درب لولایی درب سرویسی UPVC (id = 17)

            if (sample.Id == 17)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (width * (double)(120 / 100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 10) != null)
                {
                    //یراق درب سرویسی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 15).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - (double)(120 / 100))));
                }
            }

            #endregion

            #region درب لولایی درب بالکنی دوتکه شیشه یکپارچه سوویچیUPVC  (id = 16)

            if (sample.Id == 16 && katibeSizes.HasValue)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region  درب لولایی بالکنی دوتکه پنل دار سوویچیUPVC (id = 15)

            if (sample.Id == 15 && katibeSizes.HasValue)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (width * (double)(60 / 100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 22).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 11) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 11).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - (double)(60 / 100))));
                }
            }

            #endregion

            #region درب لولایی سوییچی شیشه یکپارچه Alminum (id = 38)

            if (sample.Id == 38)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region  درب لولایی سرویسی شیشه یکپارچه Alminum  (id = 39)

            if (sample.Id == 39)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }
            }

            #endregion

            #region  درب لولایی  بالکنی سوویچی Alminum (id = 40)

            if (sample.Id == 40)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (width * (double)(60 / 100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 48).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 38) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 38).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - (double)(60 / 100))));
                }
            }

            #endregion

            #region   درب لولایی درب سرویسی Alminum (id = 41)

            if (sample.Id == 41)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (width * (double)(120 / 100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 48).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 37) != null)
                {
                    //یراق درب سرویسی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 37).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - (double)(120 / 100))));
                }
            }

            #endregion

            #region  درب لولایی  بالکنی سوویچی Alminum (id = 42)

            if (sample.Id == 42)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height)));
                }
            }

            #endregion

            #region  درب لولایی بالکنی دوتکه پنل دار سوویچیUPVC (id = 43)

            if (sample.Id == 43 && katibeSize.HasValue)
            {
                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

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
                    totalPrice = totalPrice + (width * (double)(60 / 100)) * (userSegments.FirstOrDefault(p => p.SegmentId == 48).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 38) != null)
                {
                    //یراق درب سویئچی
                    totalPrice = totalPrice + (userSegments.FirstOrDefault(p => p.SegmentId == 38).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * (height - (double)(60 / 100))));
                }
            }

            #endregion


            #region پنجره ی کشویی دولنگه UPVC    

            if (sample.Id == 55)
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

            if (sample.Id == 54)
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

            if (sample.Id == 53)
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

            #region  قیمت گذاری پنجره ھای کشویی کتیبھ دار آلومینیومی دولنگه       

            if (sample.Id == 52)
            {
                if (katibeSize == null)
                {
                    return null;
                }

                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
                {
                    // لنگه کشویی
                    totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    // نوار مویی
                    totalPrice = totalPrice + ((2 * width) + (4 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
                {
                    // اینترلاک
                    totalPrice = totalPrice + ((2 * (height - katibeSize.Value))) * ((userSegments.FirstOrDefault(p => p.SegmentId == 31).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
                {
                    // ریل کشویی
                    totalPrice = totalPrice + (width) * ((userSegments.FirstOrDefault(p => p.SegmentId == 27).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
                {
                    // فریم لولایی
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    // قیمت زهوار دوجدار
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
                {
                    //قیمت یراق کشویی
                    totalPrice = totalPrice + ((userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price);
                }
            }

            #endregion

            #region  قیمت گذاری پنجره ھای کشویی کتیبھ دار آلومینیومی سه لنگه       

            if (sample.Id == 51)
            {
                if (katibeSize == null)
                {
                    return null;
                }

                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
                {
                    // لنگه کشویی
                    totalPrice = totalPrice + ((2 * width) + (6 * (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + ((2 * width) + (6 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    // نوار مویی
                    totalPrice = totalPrice + ((2 * width) + (6 * (height - katibeSize.Value))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
                {
                    // اینترلاک
                    totalPrice = totalPrice + ((4 * (height - katibeSize.Value))) * ((userSegments.FirstOrDefault(p => p.SegmentId == 31).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
                {
                    // ریل کشویی
                    totalPrice = totalPrice + (width) * ((userSegments.FirstOrDefault(p => p.SegmentId == 27).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
                {
                    // فریم لولایی
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    // قیمت زهوار دوجدار
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
                {
                    //قیمت یراق کشویی
                    totalPrice = totalPrice + (2 * ((userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
                {
                    //قیمت وادار لولایی
                    totalPrice = totalPrice + (katibeSize.Value * ((userSegments.FirstOrDefault(p => p.SegmentId == 33)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (katibeSize.Value * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + (katibeSize.Value * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price));
                }
            }

            #endregion

            #region  قیمت گذاری پنجره ھای کشویی کتیبھ دار آلومینیومی چهار لنگه       

            if (sample.Id == 50)
            {
                if (katibeSize == null)
                {
                    return null;
                }

                //Get Sample Segments
                var simpleFixAluminumhIngedWindow = await _context.SampleSelectedSegments.Include(p => p.Segment).Where(p => !p.IsDelete && p.SampleId == sample.Id).Select(p => p.Segment).ToListAsync();

                if (userSegments.FirstOrDefault(p => p.SegmentId == 1) != null)
                {
                    //قیمت فریم
                    totalPrice = totalPrice + (2 * (width + (height - katibeSize.Value))) * (userSegments.FirstOrDefault(p => p.SegmentId == 1).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 19) != null)
                {
                    // لنگه کشویی
                    totalPrice = totalPrice + ((2 * width) + (8 * (height))) * (userSegments.FirstOrDefault(p => p.SegmentId == 19).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + ((2 * width) + (8 * (height))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 21) != null)
                {
                    // نوار مویی
                    totalPrice = totalPrice + ((2 * width) + (8 * (height))) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 21).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 31) != null)
                {
                    // اینترلاک
                    totalPrice = totalPrice + ((4 * (height - katibeSize.Value))) * ((userSegments.FirstOrDefault(p => p.SegmentId == 31).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 27) != null)
                {
                    // ریل کشویی
                    totalPrice = totalPrice + (width) * ((userSegments.FirstOrDefault(p => p.SegmentId == 27).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 32) != null)
                {
                    // فریم لولایی
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 32).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    // قیمت زهوار دوجدار
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (userSegments.FirstOrDefault(p => p.SegmentId == 2).Price);
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + (2 * (katibeSize.Value + width)) * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price);
                }

                if (glassPricing != null)
                {
                    //قیمت شیشه
                    totalPrice = totalPrice + (glassPricing.Price * (width * height));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 28) != null)
                {
                    //قیمت یراق کشویی
                    totalPrice = totalPrice + (2 * ((userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 33) != null)
                {
                    //قیمت وادار لولایی
                    totalPrice = totalPrice + (katibeSize.Value * ((userSegments.FirstOrDefault(p => p.SegmentId == 33)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 2) != null)
                {
                    //قیمت زهوار دوجداره
                    totalPrice = totalPrice + (katibeSize.Value * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 2)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 30) != null)
                {
                    // لاستیک فشاري
                    totalPrice = totalPrice + (katibeSize.Value * (2 * (userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price));
                }

                if (userSegments.FirstOrDefault(p => p.SegmentId == 34) != null)
                {
                    // قفل چهارگانه
                    totalPrice = totalPrice + ((height - katibeSize.Value) * (userSegments.FirstOrDefault(p => p.SegmentId == 30)).Price);
                }
            }

            #endregion

            #endregion

            //Product Count
            if (productCount >= 1)
            {
                totalPrice = totalPrice * productCount;
            }

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

            #region List Of Brand 

            List<MainBrand> brands = new List<MainBrand>();

            #endregion

            #region Get Brands

            if (log.BrandId.HasValue)
            {
                var brand = await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == log.BrandId);
                if (brand == null) return null;

                brands.Add(brand);
            }

            if (log.BrandId == null)
            {
                var getBrands = await _context.MainBrands.Where(p => !p.IsDelete).ToListAsync();

                brands.AddRange(getBrands);
            }

            #endregion

            #region Get Sellers

            var sellers = await _context.MarketPersonalInfo.Include(p => p.User).Include(p => p.Market)
                                    .Where(p => !p.IsDelete && p.CityId == log.CityId && p.CountryId == log.CountryId
                                    && p.StateId == log.StateId && p.Market.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)
                                    .Select(p => p.User).ToListAsync();

            #endregion

            #region Log For Brands

            foreach (var brand in brands)
            {
                InquiryViewModel model2 = new InquiryViewModel();

                #region Add Log For Brands

                LogForBrands logForBrands = new LogForBrands()
                {
                    MainBrandId = brand.Id,
                    CountryId = log.CountryId,
                    CityId = log.CityId,
                    StateId = log.StateId
                };

                await _context.LogForBrands.AddAsync(logForBrands);

                #endregion
            }

            #region Log For Inquiry

            if (log.CountryId.HasValue && log.CityId.HasValue && log.StateId.HasValue && log.SellerType.HasValue)
            {
                LogForInquiry logForInquiry = new LogForInquiry()
                {
                    CityId = log.CityId.Value,
                    StateId = log.StateId.Value,
                    CountryId = log.CountryId.Value,
                    SellerType = log.SellerType.Value
                };

                await _context.LogForInquiry.AddAsync(logForInquiry);
            }

            #endregion

            await _context.SaveChangesAsync();

            #endregion

            #region Get Samples 

            foreach (var seller in sellers)
            {
                await _sellerService.UpdateSellerActivationTariff(seller.Id, true, false);

                foreach (var brand in brands)
                {
                    InquiryViewModel model2 = new InquiryViewModel();

                    model2.BrandName = brand.BrandName;
                    model2.BrandImage = brand.BrandLogo;

                    model2.UserName = seller.Username;
                    model2.UserAvatar = seller.Avatar;
                    model2.UserId = seller.Id;
                    model2.Price = 0;
                    model2.Score = await CalculateSellerScore(seller.Id);


                    foreach (var sample in logDetail)
                    {
                        model2.Price = model2.Price + await InitialTotalSamplePrice(brand.Id, sample.SampleId.Value ,  sample.Height.Value, sample.Width.Value,sample.CountOfSample ,  sample.KatibeSize, seller.Id, log.GlassId.Value);
                    }

                    if (model2.Price.HasValue && model2.Price.Value != 0)
                    {
                        model.Add(model2);
                    }
                }
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
        public async Task<bool> DeleteUserLastestInquiryDetail(ulong inquiryDetailId , string macAddress)
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

        #endregion
    }
}
