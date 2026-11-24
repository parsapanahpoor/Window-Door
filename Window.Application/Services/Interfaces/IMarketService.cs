using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Application.Services.Interfaces
{
    public interface IMarketService
    {
        #region Site Side

        Task<bool> AddMarket(ulong userId, string titleName);

        /// <summary>
        /// Adds the market, its owner link and the owner role to the current unit of work
        /// without saving. Callers that must also create the user atomically use this method
        /// and call SaveChanges once inside their transaction.
        /// </summary>
        void AddMarketEntities(ulong userId, string titleName);

        bool IsValidMarketName(string? titleName);

        #endregion
    }
}
