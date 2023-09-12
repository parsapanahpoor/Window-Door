﻿using Microsoft.AspNetCore.Mvc;
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

            return model;
        }

        #endregion
    }
}
