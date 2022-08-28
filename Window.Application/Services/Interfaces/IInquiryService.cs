using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Sample;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Interfaces
{
    public interface IInquiryService
    {
        #region Site Side

        //Log Inquiry For User In Step 1
        Task LogInquiryForUserPart1(FilterInquiryViewModel filter);

        //Log Inquiry For User In Step 2
        Task<bool> LogInquiryForUserPart2(ulong sampleId, int width, int height, string userMacAddress);

        Task<List<InquiryViewModel>?> ListOfInquiry(string userMacAddress);

        Task<int?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int height, int width, ulong userId , ulong glassId);

        Task<int?> InitializeSamplesPrice(List<Sample?> samples, User user, int height, int width);

        //Update User Inqury In Last Step For Update Brand 
        Task<bool> UpdateUserInquryInLastStep(string userMacAddress, string brandTitle);

        //Calculate Seller Score 
        Task<int> CalculateSellerScore(ulong userId);

        //Check Is User Scored To Seller 
        Task<bool> checkIsUserScoredToSeller(string macAddress, ulong sellerId);

        //Add Score For Seller
        Task<bool> AddScoreForSeller(int score, ulong sellerId, string userMacAddress);

        #endregion
    }
}
