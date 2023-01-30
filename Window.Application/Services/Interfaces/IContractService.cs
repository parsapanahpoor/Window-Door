using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Contract;
using Window.Domain.ViewModels.Seller.Contract;

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

        #region Seller Side

        //Count Of User Waiting Contract Request 
        Task<int> CountOfUserWaitingContractRequest(ulong sellerId);

        //List Of Seller Contracts Request 
        Task<FilterContractRequestSellerSideViewModel> FilterContractRequestSellerSide(FilterContractRequestSellerSideViewModel filter);

        //Decline Request From Seller
        Task<bool> DeclineRequestFromSeller(ulong requestId, ulong sellerId);

        //Accept Request From Seller
        Task<bool> AcceptRequestFromSeller(ulong requestId, ulong sellerId);

        #endregion
    }
}
