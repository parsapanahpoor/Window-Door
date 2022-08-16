﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Admin.PersonalInfo;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Application.Services.Interfaces
{
    public interface ISellerService
    {
        #region Seller Panel

        Task<bool> PayAccountChargeTariff(ulong userId, int price);

        Task<bool> ChargeUserWallet(ulong userId, int price);

        Task<bool> ChargeAccount(ulong marketId, ulong userId);

        Task<Market?> GetMarketByUserId(ulong userId);

        Task<int?> GetMarketAccountChargeTariff(ulong userId);

        Task<bool> HasMarketStateForPayAccountCharge(ulong userId);

        Task<MarketChargeInfoViewModel?> FillMarketChargeInfoViewModel(ulong userId);

        Task<InformationOfSellerStateViewModel> FillInformationOfSellerStateViewModel(ulong sellerId);
        
        Task<string> GetSellersInfosState(ulong userId);

        Task<bool> IsSellerMaster(ulong masterId);

        Task<AddSellerPersonalInfoResul> AddSellerPersonalInfo(AddSellerPersonalInfoViewModel info , ulong userId);

        Task<bool> IsExistSellerPersonalInformations(ulong userId);

        Task<AddSellerLinksResult> AddSellerLinksFromSeller(AddSellerLinksViewModel links);

        Task<List<AddSellerLinksViewModel>> GetSellerLinksForShowInAddSellerLinksPage(ulong userId);

        Task<bool> IsExistAnySellerInfo(ulong userId);

        Task<bool> DeleteSellerLink(ulong linkId, ulong userId);

        Task<List<AddSellerWorkSampleViewModel>> GetSellerWorkSampleForShowInAddSellerLinksPage(ulong userId);

        Task<AddSellerWorkSampleResult> AddSellerWorkSample(AddSellerWorkSampleViewModel workSample, IFormFile workSampleImage);

        Task<bool> DeleteSellerWorkSample(ulong workSampleId, ulong userId);

        Task<bool> IsUserFillAllOfPersonalInfoFiles(ulong userId);

        Task<bool> IsUserJustFillUserPersonalInfo(ulong userId);

        Task<bool> IsUserJustFillUserPersonalLinks(ulong userId);

        Task<ListOfPersonalInfoViewModel> FillListOfPersonalInfoViewModel(ulong userId);

        Task UpdateSellerStateAfterEditPersonalInfo(ulong userId);

        Task<bool> AddSellerWorkSampleInModal(AddSellerWorkSampleViewModel workSample);

        Task<UpdateSellerPersonalInfoResul> UpdateSellerPersonalInfoFromSellerPanel(ListOfPersonalInfoViewModel info, ulong userId);

        Task<ListOfSellersInfoViewModel> FilterPersonalInfo(ListOfSellersInfoViewModel filter);

        Task<bool> CheckUserCharge(ulong UserId);

        #endregion

        #region Admin Panel

        Task<Market?> GetMarketByMarketId(ulong marketId);

        Task<bool> ChangeMarketStateFromAdminPanel(ListOfPersonalInfoViewModel model, ulong marketId);

        #endregion

        #region Site Side

        //Update Seller Activation Tariff After Seen Seller Profile By User 
        Task UpdateSellerActivationTariff(ulong userId);

        #endregion
    }
}
