using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Contract;

namespace Window.Application.Services.Interfaces
{
    public interface IContractService
    {
        #region Site Side

        //Can User Insert Comment For Seller
        Task<RequestForContract?> CanUserInsertCommentForSeller(ulong userId, ulong sellerId);

        //Create Contract Request From User
        Task<bool> CreateContractRequestFromUser(ulong userId, ulong sellerId);

        #endregion
    }
}
