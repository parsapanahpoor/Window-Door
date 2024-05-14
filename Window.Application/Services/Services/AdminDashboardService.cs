using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Site.Home;

namespace Window.Application.Services.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public AdminDashboardService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Dashboard 

        public async Task<int> CountOfActiveUsers()
        {
            return await _context.Users.CountAsync(p => !p.IsDelete);
        }

        public async Task<int> CountOFTodayRegisterUsers()
        {
            return await _context.Users.CountAsync(p => !p.IsDelete && p.CreateDate.Year == DateTime.Now.Year && p.CreateDate.DayOfYear == DateTime.Now.DayOfYear);
        }

        public async Task<int> CountOfActiveMarkets()
        {
            return await _context.Market.CountAsync(p => !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount);
        }

        public async Task<int> CountOfDisActiveMarkets()
        {
            return await _context.Market.CountAsync(p => !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.DisAcctiveMarketAccount);
        }

        public async Task<List<MarketChargeInfo>?> GetListOfMarketsThatDisActiveToday()
        {
            return await _context.MarketChargeInfo.Include(p => p.MArket).ThenInclude(p => p.User)
                               .Where(p => !p.IsDelete && p.CurrentAccountCharge && p.EndDate.Year == DateTime.Now.Year
                                      && p.EndDate.DayOfYear == DateTime.Now.DayOfYear).ToListAsync();
        }

        public async Task<List<MarketChargeInfo>?> GetListOfMarketsThatDisActiveIn3Day()
        {
            return await _context.MarketChargeInfo.Include(p => p.MArket).ThenInclude(p => p.User)
                               .Where(p => !p.IsDelete && p.CurrentAccountCharge && p.EndDate.Year == DateTime.Now.Year
                                      && (DateTime.Now.DayOfYear - p.EndDate.DayOfYear <= 3) && (DateTime.Now.DayOfYear - p.EndDate.DayOfYear >= 1)).ToListAsync();
        }

        public async Task<List<MarketChargeInfo>?> GetListOfMarketsThatDisActiveIn15Day()
        {
            return await _context.MarketChargeInfo.Include(p => p.MArket).ThenInclude(p => p.User)
                               .Where(p => !p.IsDelete && p.CurrentAccountCharge && p.EndDate.Year == DateTime.Now.Year
                                      && (DateTime.Now.DayOfYear - p.EndDate.DayOfYear <= 15) && (DateTime.Now.DayOfYear - p.EndDate.DayOfYear >= 4)).ToListAsync();
        }

        #endregion

        #region Site Side 

        //Fill Home Index ViewModel
        public async Task<IndexViewModel> FillHomeIndexViewModel()
        {
            IndexViewModel model = new IndexViewModel();

            #region Tazmin DarKharid 

            model.TazminDarKharid = await  _context.TechnicalIssues.Where(p => !p.IsDelete).FirstOrDefaultAsync();

            #endregion

            #region Free Consultant

            model.FreeConsultant = await _context.Counselings.Where(p => !p.IsDelete).ToListAsync();

            #endregion

            #region Articles

            model.Articles = await _context.Articles.Where(p => !p.IsDelete).Take(5).ToListAsync();

            #endregion

            #region Main Brands

            model.MainBrands = await _context.MainBrands
                                             .AsNoTracking()
                                             .Where(p=> !p.IsDelete && !string.IsNullOrEmpty(p.BrandSite))
                                             .OrderByDescending(p=> p.Priority)
                                             .Take(30)
                                             .ToListAsync();
            #endregion

            #region Site Setting1

            model.SiteSetting1 = await _context.SiteSetting1.FirstOrDefaultAsync(p=> !p.IsDelete);

            #endregion

            #region Color Full 

            model.ColorFullSiteSettings = await _context.ColorFullSiteSetting.Where(p => !p.IsDelete).ToListAsync();

            #endregion

            #region Mohasebeye Online

            model.MohasebeyeOnlineGheymat = await _context.mohasebeyeOnlineGheymat.FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region Free Consultant

            model.FreeConsultants = await _context.FreeConsultants.Where(p => !p.IsDelete).ToListAsync();

            #endregion

            #region Tazmin Dar Kharid 

            model.TazminKharid = await _context.TazminDarKharid.FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region Lastest Component

            model.LastestComponents = await _context.LastestComponents.Where(p => !p.IsDelete).ToListAsync();

            #endregion

            return model;
        }

        #endregion
    }
}
