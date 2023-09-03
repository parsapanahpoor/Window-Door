using Microsoft.AspNetCore.Mvc;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Site.Home;

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

        Task<List<MarketChargeInfo>?> GetListOfMarketsThatDisActiveIn15Day();

        #region Site Side 

        //Fill Home Index ViewModel
        Task<IndexViewModel> FillHomeIndexViewModel();

        #endregion
    }
}
