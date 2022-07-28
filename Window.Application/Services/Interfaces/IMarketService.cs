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

        #endregion
    }
}
