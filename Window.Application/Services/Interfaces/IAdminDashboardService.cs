using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;

namespace Window.Application.Services.Interfaces
{
    public interface IAdminDashboardService 
    {
        Task<int> CountOfActiveUsers();

        Task<int> CountOfActiveMarkets();

        Task<int> CountOfDisActiveMarkets();

        Task<int> CountOFTodayRegisterUsers();

        Task<List<MarketChargeInfo>?> GetListOfMarketsThatDisActiveToday();

        Task<List<MarketChargeInfo>?> GetListOfMarketsThatDisActiveIn3Day();
    }
}
