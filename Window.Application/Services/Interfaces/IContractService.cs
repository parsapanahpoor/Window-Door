using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Comment;
using Window.Domain.Entities.Contract;
using Window.Domain.ViewModels.Admin.Contract;
using Window.Domain.ViewModels.Seller.Contract;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Interfaces
{
    public interface IContractService
    {
        #region Site Side

        //Check That Is Exist Any Market By This Seller Id
        Task<bool> CheckThatIsExistAnyMarketByThisSellerId(ulong sellerId);

        //Add Comment From User
        Task<bool> AddCommentFromUser(AddCommentSiteSideViewModel comment, ulong userId);

        //Can User Insert Comment For Seller
        Task<RequestForContract?> CanUserInsertCommentForSeller(ulong userId, ulong sellerId);

        //Create Contract Request From User
        Task<bool> CreateContractRequestFromUser(ulong userId, ulong sellerId);

        #endregion

        #region Seller Side

        //List Of seller Comments For Show
        Task<List<Comment>?> ListOfSellerCommentsForShow(ulong sellerId);

        //Count Of User Waiting Contract Request 
        Task<int> CountOfUserWaitingContractRequest(ulong sellerId);

        //List Of Seller Contracts Request 
        Task<FilterContractRequestSellerSideViewModel> FilterContractRequestSellerSide(FilterContractRequestSellerSideViewModel filter);

        //Decline Request From Seller
        Task<bool> DeclineRequestFromSeller(ulong requestId, ulong sellerId);

        //Accept Request From Seller
        Task<bool> AcceptRequestFromSeller(ulong requestId, ulong sellerId);

        //Get Request By Request Id 
        Task<RequestForContract?> GetRequestByRequestId(ulong requestId);

        #endregion

        #region Admin Side 

        //Filter Contract From Admin Panel 
        Task<FiltreContractAdminSideViewModel> FiltreContractAdminSide(FiltreContractAdminSideViewModel filter);

        #endregion
    }
}
