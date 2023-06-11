#region Usings

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Inquiry;
using Window.Domain.Entities.Sample;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Interfaces;

#endregion

public interface IInquiryService
{
    #region Site Side

    //Update Log User Inquiry Request
    Task UpdateLogUserInquiryRequest(string userMacAddress, ulong brandId);

    //Check Log Result User Inquiry
    Task<int> CheckLogResultUserInquiry(string userMacAddress);

    //Log Inquiry For User In Step 1
    Task LogInquiryForUserPart1(FilterInquiryViewModel filter);

    //Log Inquiry For User In Step 2
    Task<bool> LogInquiryForUserPart2(ulong sampleId, int width, int height, int? KatibeSize, string userMacAddress, int productCount);

    Task<List<InquiryViewModel>?> ListOfInquiry(string userMacAddress , ulong userId);

    //Initial Result Of User Inquiry
    Task<bool> InitialResultOfUserInquiry(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, ulong UserId, string userMacAddress);

    Task<double?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int height, int width, int productCount , int? katibeSizes, ulong userId , ulong glassId);

    Task<int?> InitializeSamplesPrice(List<Sample?> samples, User user, int height, int width);

    //Update User Inqury In Last Step For Update Brand 
    Task<bool> UpdateUserInquryInLastStep(string userMacAddress, string? brandTitle);

    //Calculate Seller Score 
    Task<int> CalculateSellerScore(ulong userId);

    //Check Is User Scored To Seller 
    Task<bool> checkIsUserScoredToSeller(string macAddress, ulong sellerId);

    //Add Score For Seller
    Task<bool> AddScoreForSeller(int score, ulong sellerId, string userMacAddress);

    //Add Score For Seller
    Task<bool> AddScoreForSeller(AddScoreToTheSellerViewModel model, string userMacAddress);

    //Get Count Of Inquiry In Cities
    Task<int> GetCountOfInquiryInCities(string cityName);

    //Get Count Of Inquiry In State 
    Task<int> CountOfInquiryInState(string stateName);

    //Get User Lastest Inquiry 
    Task<List<LogInquiryForUserDetail>?> GetUserLastestInquiryDetailForChange(string macAddress);

    //Delete User Lastest Inquiry Detail 
    Task<bool> DeleteUserLastestInquiryDetail(ulong inquiryDetailId, string macAddress);

    //Update User Inquiry Detail
    Task<bool> UpdateUserInquiryItrm(ulong inquiryDetailId , ulong sampleId, int width, int height, int? katibe, string macAddress, int? SampleCount);

    #endregion

    #region Admin Side 

    Task<int?> CountOfTodayInquiry();

    Task<int?> CountOfMonthInquiry();

    Task<int?> CountOfYearInquiry();

    #endregion
}
