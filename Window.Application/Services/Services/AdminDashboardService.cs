using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Market;

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

        #endregion
    }
}
