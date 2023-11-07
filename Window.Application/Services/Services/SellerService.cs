using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Extensions;
using Window.Application.Generators;
using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Application.StaticTools;
using Window.Application.StticTools;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.Wallet;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Admin.Log;
using Window.Domain.ViewModels.Admin.PersonalInfo;
using Window.Domain.ViewModels.Seller.PersonalInfo;
using Window.Domain.ViewModels.Site.Inquiry;
using Window.Domain.ViewModels.User;

namespace Window.Application.Services.Services
{
    public class SellerService : ISellerService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        private readonly IUserService _userService;

        private readonly IWalletRepository _walletRepository;

        private readonly ISMSService _smsService;

        public SellerService(WindowDbContext context, IUserService userService, IWalletRepository walletRepository, ISMSService smsService)
        {
            _context = context;
            _userService = userService;
            _walletRepository = walletRepository;
            _smsService = smsService;
        }

        #endregion

        #region Seller Panel

        public async Task<int> CountOfTodayUserInInquiry(ulong userId)
        {
            #region Get Market By User Id 

            var marketPersons = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketPersons == null) return 0;

            var market = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketPersons.MarketId);
            if (market == null) return 0;

            #endregion

            return await _context.LogForVisitSellerProfiles.Where(p => !p.IsDelete && p.UserId == market.UserId
                            && p.CreateDate.Year == DateTime.Now.Year && p.CreateDate.DayOfYear == DateTime.Now.DayOfYear).CountAsync();
        }

        public async Task<int> CountOfMonthUserInInquiry(ulong userId)
        {
            #region Get Market By User Id 

            var marketPersons = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketPersons == null) return 0;

            var market = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketPersons.MarketId);
            if (market == null) return 0;

            #endregion

            return await _context.LogForVisitSellerProfiles.Where(p => !p.IsDelete && p.UserId == market.UserId
                                     && p.CreateDate.Year == DateTime.Now.Year && p.CreateDate.Month == DateTime.Now.Month).CountAsync();
        }

        public async Task<int> CountOfYearUserInInquiry(ulong userId)
        {
            #region Get Market By User Id 

            var marketPersons = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketPersons == null) return 0;

            var market = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketPersons.MarketId);
            if (market == null) return 0;

            #endregion

            return await _context.LogForVisitSellerProfiles.Where(p => !p.IsDelete && p.UserId == market.UserId
                                     && p.CreateDate.Year == DateTime.Now.Year).CountAsync();
        }

        public async Task<bool> PayAccountChargeTariff(ulong userId, int price)
        {
            if (!await _context.Users.AnyAsync(p => !p.IsDelete && p.Id == userId))
            {
                return false;
            }

            var wallet = new Wallet
            {
                UserId = userId,
                TransactionType = TransactionType.Withdraw,
                GatewayType = GatewayType.Zarinpal,
                PaymentType = PaymentType.Buy,
                Price = price,
                Description = "Pay Account Charge Tariff",
                IsFinally = true
            };

            await _walletRepository.CreateWalletAsync(wallet);
            return true;
        }

        public async Task<bool> ChargeUserWallet(ulong userId, int price)
        {
            if (!await _context.Users.AnyAsync(p => !p.IsDelete && p.Id == userId))
            {
                return false;
            }

            var wallet = new Wallet
            {
                UserId = userId,
                TransactionType = TransactionType.Deposit,
                GatewayType = GatewayType.Zarinpal,
                PaymentType = PaymentType.ChargeWallet,
                Price = price,
                Description = "Charge User Wallet For Pay Account Charge Tarrif",
                IsFinally = true
            };

            await _walletRepository.CreateWalletAsync(wallet);
            return true;
        }

        public async Task<bool> PayHomeVisitTariff(ulong userId, int price)
        {
            var wallet = new Wallet
            {
                UserId = userId,
                TransactionType = TransactionType.Withdraw,
                GatewayType = GatewayType.Zarinpal,
                PaymentType = PaymentType.Buy,
                Price = price,
                Description = "پرداخت مبلغ ",
                IsFinally = true,
            };

            await _walletRepository.CreateWalletAsync(wallet);
            return true;
        }

        public async Task<bool> ChargeAccount(ulong marketId, ulong userId)
        {
            #region Get MArket 

            var market = await GetMarketByMarketId(marketId);
            if (market == null) return false;

            #endregion

            #region Get User Last Charge 

            var lastCharge = await _context.MarketChargeInfo.Where(p => !p.IsDelete && p.MarketId == market.Id)
                                    .OrderByDescending(p => p.CreateDate).FirstOrDefaultAsync();

            #endregion

            //For The First Time 
            if (lastCharge == null)
            {
                #region Add Record For Charge Information 

                MarketChargeInfo charge = new MarketChargeInfo()
                {
                    CreateDate = DateTime.Now,
                    CurrentAccountCharge = true,
                    EndDate = DateTime.Now.AddDays(30),
                    StartDate = DateTime.Now,
                    IsDelete = false,
                    MarketId = market.Id,
                    Price = market.ActivationTariff,
                    UserId = userId
                };

                await _context.MarketChargeInfo.AddAsync(charge);
                await _context.SaveChangesAsync();

                #endregion
            }

            //For The Seconde Time 
            else
            {
                #region Add Record For Charge Information 

                MarketChargeInfo charge = new MarketChargeInfo()
                {
                    CreateDate = DateTime.Now,
                    CurrentAccountCharge = true,
                    EndDate = lastCharge.EndDate.AddDays(30),
                    StartDate = lastCharge.EndDate,
                    IsDelete = false,
                    MarketId = market.Id,
                    Price = market.ActivationTariff,
                    UserId = userId
                };

                await _context.MarketChargeInfo.AddAsync(charge);
                await _context.SaveChangesAsync();

                #endregion
            }

            #region Update Market State 

            market.MarketPersonalsInfoState = MarketPersonalsInfoState.ActiveMarketAccount;
            market.ActivationTariff = 0;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        public async Task<Market?> GetMarketByUserId(ulong userId)
        {
            #region Get Market

            var marketUser = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketUser == null) return null;

            var market = await GetMarketByMarketId(marketUser.MarketId);
            if (market == null) return null;

            #endregion

            return market;
        }

        public async Task<int?> GetMarketAccountChargeTariff(ulong userId)
        {
            #region Get Market

            var marketUser = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketUser == null) return null;

            var market = await GetMarketByMarketId(marketUser.MarketId);
            if (market == null) return null;

            #endregion

            return market.ActivationTariff;
        }

        public async Task<bool> HasMarketStateForPayAccountCharge(ulong userId)
        {
            #region Get Market

            var marketUser = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketUser == null) return false;

            var market = await GetMarketByMarketId(marketUser.MarketId);
            if (market == null) return false;

            if (market.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForConfirmPersonalInformations
                || market.MarketPersonalsInfoState == MarketPersonalsInfoState.ActiveMarketAccount
                || market.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller
                || market.MarketPersonalsInfoState == MarketPersonalsInfoState.Rejected
                || market.MarketPersonalsInfoState == MarketPersonalsInfoState.Rejected)
            {
                return false;
            }

            #endregion

            #region Check Account Charge Status

            var accountCharge = await _context.MarketChargeInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.MarketId == market.Id && p.CurrentAccountCharge);
            if (accountCharge != null) return false;

            #endregion

            return true;
        }

        public async Task<MarketChargeInfoViewModel?> FillMarketChargeInfoViewModel(ulong userId)
        {
            #region Get Market

            var marketUser = await _context.MarketUser.Include(p => p.Market).FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
            if (marketUser == null) return null;

            var market = await GetMarketByMarketId(marketUser.MarketId);
            if (market == null) return null;

            #endregion

            #region Fill View Model

            MarketChargeInfoViewModel model = new MarketChargeInfoViewModel()
            {
                ChargePrice = market.ActivationTariff,
                marketId = market.Id,
                MarketName = market.MarketName,
                MarketPersonalsInfoState = market.MarketPersonalsInfoState,
            };

            #region Day Of Charge

            var charge = await _context.MarketChargeInfo.FirstOrDefaultAsync(p => p.MarketId == market.Id && !p.IsDelete && p.CurrentAccountCharge);

            if (charge != null)
            {
                if (DateTime.Now.DayOfYear >= charge.EndDate.DayOfYear)
                {
                    model.DaysOfCharge = charge.EndDate.DayOfYear + (365 - DateTime.Now.DayOfYear);
                }
                else
                {
                    model.DaysOfCharge = charge.EndDate.DayOfYear - DateTime.Now.DayOfYear;
                }
            }

            #endregion

            #endregion

            return model;
        }

        public async Task<InformationOfSellerStateViewModel> FillInformationOfSellerStateViewModel(ulong sellerId)
        {
            InformationOfSellerStateViewModel model = new InformationOfSellerStateViewModel();

            //Get market
            var market = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == sellerId);

            //Reject Note
            var rejectNote = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.MarketId == market.Id);

            if (rejectNote != null)
            {
                if (!string.IsNullOrEmpty(rejectNote.RejectDescription))
                {
                    model.RejectNote = rejectNote.RejectDescription;
                }
            }

            model.MarketPersonalsInfoState = market.MarketPersonalsInfoState;

            return model;
        }

        public async Task<bool> IsSellerMaster(ulong masterId)
        {
            return await _context.Market.AnyAsync(p => !p.IsDelete && p.UserId == masterId);
        }

        public async Task<string> GetSellersInfosState(ulong userId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion

            //If Seller Registers Now
            if (!await _context.Market.AnyAsync(p => !p.IsDelete && p.UserId == market.UserId)) return "NewUser";

            //If Seller State Is WatingForConfirm Informations
            if (await _context.Market.AnyAsync(p => p.UserId == market.UserId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.WaitingForConfirmPersonalInformations)) return "WatingForConfirmInformations";

            //If Seller State Is Waiting For Payed Money From Seller
            if (await _context.Market.AnyAsync(p => p.UserId == market.UserId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.WaitingForPyedFromSeller)) return "WaitingForPyedFromSeller";

            //If Seller State Is Accepted And Active
            if (await _context.Market.AnyAsync(p => p.UserId == market.UserId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)) return "ActiveSellerAccount";

            //If Seller Informations State Is Accepted 
            if (await _context.Market.AnyAsync(p => p.UserId == market.UserId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.AcceptedPersonalInformation)) return "AcceptedPersonalInformation";

            //If Seller Informations State Is DisActived 
            if (await _context.Market.AnyAsync(p => p.UserId == market.UserId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.Rejected)) return "Rejected";

            //If Seller State Is DisActived
            if (await _context.Market.AnyAsync(p => p.UserId == market.UserId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.DisAcctiveMarketAccount)) return "DisAcctiveSellerAccount";

            return "NewUser";
        }

        public async Task<AddSellerPersonalInfoResul> AddSellerPersonalInfo(AddSellerPersonalInfoViewModel info, ulong userId)
        {
            #region Data Validations

            if (info.CompanyLogo == null || !info.CompanyLogo.IsImage())
            {
                return AddSellerPersonalInfoResul.ImagesNotFound;
            }

            if (info.PhotoOfNationalCode == null || !info.PhotoOfNationalCode.IsImage())
            {
                return AddSellerPersonalInfoResul.ImagesNotFound;
            }

            if (info.PhotoOfBusinessLicense == null || !info.PhotoOfBusinessLicense.IsImage())
            {
                return AddSellerPersonalInfoResul.ImagesNotFound;
            }

            if (!await _context.Users.AnyAsync(p => p.Id == userId && !p.IsDelete))
            {
                return AddSellerPersonalInfoResul.Faild;
            }

            if (await _context.MarketPersonalInfo.AnyAsync(p => p.UserId == userId && !p.IsDelete))
            {
                return AddSellerPersonalInfoResul.Faild;
            }

            #endregion

            #region Add Personal Info 

            #region Get Market 

            var market = await GetMarket(userId);

            #endregion

            #region Fill Entities

            MarketPersonalInfo marketPersonalInfos = new MarketPersonalInfo
            {
                NationalCode = info.NationalCode,
                CompanyName = info.CompanyName,
                Resume = info.Resume,
                PeriodTimesOfWork = info.PeriodTimesOfWork,
                WorkAddress = info.WorkAddress,
                MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller,
                UserId = userId,
                MarketId = market.Id,
                SellerType = info.SellerType,
                CountryId = info.CountryId,
                CityId = info.CityId,
                StateId = info.StateId,
            };

            #endregion

            #region Add Images


            var companylogo = Guid.NewGuid() + Path.GetExtension(info.CompanyLogo.FileName);
            info.CompanyLogo.AddImageToServer(companylogo, FilePaths.SellerInfoPathServer, 400, 300, FilePaths.SellerInfoPathThumbServer);
            marketPersonalInfos.CompanyLogo = companylogo;

            var PhotoOfBusinessLicense = Guid.NewGuid() + Path.GetExtension(info.PhotoOfBusinessLicense.FileName);
            info.PhotoOfBusinessLicense.AddImageToServer(PhotoOfBusinessLicense, FilePaths.SellerInfoPathServer, 400, 300, FilePaths.SellerInfoPathThumbServer);
            marketPersonalInfos.PhotoOfBusinessLicense = PhotoOfBusinessLicense;

            var imageName = Guid.NewGuid() + Path.GetExtension(info.PhotoOfNationalCode.FileName);
            info.PhotoOfNationalCode.AddImageToServer(imageName, FilePaths.SellerInfoPathServer, 400, 300, FilePaths.SellerInfoPathThumbServer);
            marketPersonalInfos.PhotoOfNationalCode = imageName;

            #endregion

            await _context.MarketPersonalInfo.AddAsync(marketPersonalInfos);
            await _context.SaveChangesAsync();

            #endregion

            #region Update MArket 

            market.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion

            return AddSellerPersonalInfoResul.Success;
        }

        public async Task<bool> IsExistSellerPersonalInformations(ulong userId)
        {
            var user = await _context.Users.AnyAsync(p => p.Id == userId && !p.IsDelete);
            if (user == null) return false;

            var userPersonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (userPersonalInfo == null) return false;

            return true;
        }

        public async Task<AddSellerLinksResult> AddSellerLinksFromSeller(AddSellerLinksViewModel links)
        {
            #region Model State Validations

            if (!await _context.Users.AnyAsync(p => p.Id == links.UserId && !p.IsDelete))
            {
                return AddSellerLinksResult.Faild;
            }

            #endregion

            #region Get Market 

            var market = await GetMarket(links.UserId.Value);
            if (market == null) return AddSellerLinksResult.Faild;

            #endregion

            #region Add Links Method 

            MarketLinks addSellerLinksViewModel = new MarketLinks
            {
                UserId = links.UserId.Value,
                LinkTitle = links.LinkTitle,
                LinkValue = links.LinkValue,
                MarketId = market.Id,
            };

            await _context.MarketLinks.AddAsync(addSellerLinksViewModel);
            await _context.SaveChangesAsync();

            #endregion

            return AddSellerLinksResult.Success;
        }

        public async Task UpdateSellerStateAfterEditPersonalInfo(ulong userId)
        {
            #region Update Market Info 

            var market = await GetMarket(userId);

            market.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion

            #region Edit Method

            var sellerInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);

            sellerInfo.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;

            _context.MarketPersonalInfo.Update(sellerInfo);
            await _context.SaveChangesAsync();

            #endregion
        }

        public async Task<List<AddSellerLinksViewModel>> GetSellerLinksForShowInAddSellerLinksPage(ulong userId)
        {
            return await _context.MarketLinks.Where(p => !p.IsDelete && p.UserId == userId)
                                    .Select(p => new AddSellerLinksViewModel
                                    {
                                        UserId = p.UserId,
                                        LinkTitle = p.LinkTitle,
                                        LinkValue = p.LinkValue,
                                        Id = p.Id
                                    }).ToListAsync();
        }

        public async Task<bool> IsExistAnySellerInfo(ulong userId)
        {
            return await _context.MarketPersonalInfo.AnyAsync(p => p.UserId == userId && !p.IsDelete);
        }

        public async Task<bool> DeleteSellerLink(ulong linkId, ulong userId)
        {
            var link = await _context.MarketLinks.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId && p.Id == linkId);
            if (link == null) return false;

            link.IsDelete = true;

            _context.MarketLinks.Update(link);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<AddSellerWorkSampleViewModel>> GetSellerWorkSampleForShowInAddSellerLinksPage(ulong userId)
        {
            return await _context.MarketWorkSamle.Where(p => !p.IsDelete && p.UserId == userId)
                                    .Select(p => new AddSellerWorkSampleViewModel
                                    {
                                        UserId = p.UserId,
                                        WorkSampleImage = p.WorkSampleImage,
                                        WorkSampleTitle = p.WorkSampleTitle,
                                        Id = p.Id
                                    }).ToListAsync();
        }

        public async Task<AddSellerWorkSampleResult> AddSellerWorkSample(AddSellerWorkSampleViewModel workSample, IFormFile workSampleImage)
        {
            #region Data Validations 

            if (!await _context.Users.AnyAsync(p => !p.IsDelete && p.Id == workSample.UserId))
            {
                return AddSellerWorkSampleResult.Faild;
            }

            if (workSampleImage == null || !workSampleImage.IsImage())
            {
                return AddSellerWorkSampleResult.ImageNotFound;
            }

            #endregion

            #region Get Market 

            var market = await GetMarket(workSample.UserId.Value);
            if (market == null) return AddSellerWorkSampleResult.Faild;

            #endregion

            #region Add Work Sample 

            MarketWorkSamle marketWorkSample = new MarketWorkSamle
            {
                UserId = workSample.UserId.Value,
                WorkSampleTitle = workSample.WorkSampleTitle,
                MarketId = market.Id,
            };

            #region Add Image

            if (workSampleImage != null && workSampleImage.IsImage())
            {
                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(workSampleImage.FileName);
                workSampleImage.AddImageToServer(imageName, FilePaths.SellerInfoPathServer, 270, 270, FilePaths.SellerInfoPathThumbServer);
                marketWorkSample.WorkSampleImage = imageName;
            }

            #endregion

            await _context.MarketWorkSamle.AddAsync(marketWorkSample);
            await _context.SaveChangesAsync();

            #endregion

            #region Update Seller State 

            var seller = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == workSample.UserId);

            seller.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;

            _context.MarketPersonalInfo.Update(seller);
            await _context.SaveChangesAsync();

            #endregion

            #region Update market State 

            market.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion

            return AddSellerWorkSampleResult.Success;
        }

        public async Task<bool> DeleteSellerWorkSample(ulong workSampleId, ulong userId)
        {
            var workSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId && p.Id == workSampleId);
            if (workSample == null) return false;

            workSample.IsDelete = true;

            _context.MarketWorkSamle.Update(workSample);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Market?> GetMarket(ulong userId)
        {
            return await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
        }

        public async Task<bool> IsUserFillAllOfPersonalInfoFiles(ulong userId)
        {
            var pesonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState != MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (pesonalInfo == null) return false;

            var sellerLinks = await _context.MarketLinks.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerLinks == null) return false;

            var sellerWorkSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerWorkSample == null) return false;

            return true;
        }

        public async Task<bool> IsUserJustFillUserPersonalInfo(ulong userId)
        {
            var pesonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (pesonalInfo == null) return false;

            var sellerLinks = await _context.MarketLinks.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerLinks != null) return false;

            var sellerWorkSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerWorkSample != null) return false;

            return true;
        }

        public async Task<bool> IsUserJustFillUserPersonalLinks(ulong userId)
        {
            var pesonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller); ;
            if (pesonalInfo == null) return false;

            var sellerLinks = await _context.MarketLinks.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerLinks == null) return false;

            var sellerWorkSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerWorkSample != null) return false;

            return true;
        }

        public async Task<bool> SendSMSForSellerForSeenProfile(ulong userId)
        {
            #region Validation User

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId && !p.IsDelete);
            if (user == null) return false;

            var sellerPersonalInfo = await _context.MarketPersonalInfo.Include(p => p.Market).FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState != MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (sellerPersonalInfo == null) return false;

            var sellerLinks = await _context.MarketLinks.Where(p => p.UserId == userId && !p.IsDelete).ToListAsync();
            if (sellerLinks == null) return false;

            var sellerWorkSamples = await _context.MarketWorkSamle.Where(p => p.UserId == userId && !p.IsDelete).ToListAsync();
            if (sellerWorkSamples == null) return false;

            #endregion

            #region Get Allet Text Message

            var text = await _context.SiteSettings.FirstOrDefaultAsync();
            if (text != null)
            {
                var message = text.AlertForSellerForSeenProfile;
                await _smsService.SendSimpleSMS(user.Mobile, message);
            }

            #endregion

            return true;
        }

        public async Task<bool> SendSMSForDisActiveUsers(ulong marketId, bool threetoday, bool day, bool fifteenday)
        {
            #region Validation Market

            var market = await _context.Market.Include(p => p.User).FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketId);
            if (market == null) return false;

            #endregion

            #region Get Allet Text Message

            var text = await _context.SiteSettings.FirstOrDefaultAsync();
            if (text != null)
            {

                if (threetoday == true)
                {
                    var message = text.SMSForDisActiveFrom3Day;
                    await _smsService.SendSimpleSMS(market.User.Mobile, message);
                }

                if (day == true)
                {
                    var message = text.SMSForDisActiveFromToday;
                    await _smsService.SendSimpleSMS(market.User.Mobile, message);
                }

                if (fifteenday == true)
                {
                    var message = text.SMSForDisActiveFrom15Day;
                    await _smsService.SendSimpleSMS(market.User.Mobile, message);
                }

            }

            #endregion

            return true;
        }


        public async Task LogForSellerVisitProfile(ulong userId)
        {
            #region Fill Model

            LogForVisitSellerProfile model = new LogForVisitSellerProfile()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                UserId = userId
            };

            #endregion

            #region Add Method

            await _context.LogForVisitSellerProfiles.AddAsync(model);
            await _context.SaveChangesAsync();

            #endregion
        }

        //Check That Is Exist Any Market By This Seller Id
        public async Task<bool> CheckThatIsExistAnyMarketByThisSellerId(ulong sellerId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == sellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            return true;
        }

        public async Task<ListOfPersonalInfoViewModel> FillListOfPersonalInfoViewModel(ulong userId)
        {
            #region Validation User

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId && !p.IsDelete);
            if (user == null) return null;

            var sellerPersonalInfo = await _context.MarketPersonalInfo
                .Include(p => p.Market)
                .FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState != MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (sellerPersonalInfo == null) return null;

            var sellerLinks = await _context.MarketLinks.Where(p => p.UserId == userId && !p.IsDelete).ToListAsync();
            if (sellerLinks == null) return null;

            var sellerWorkSamples = await _context.MarketWorkSamle.Where(p => p.UserId == userId && !p.IsDelete).ToListAsync();
            if (sellerWorkSamples == null) return null;

            #endregion

            #region Fill View Model 

            ListOfPersonalInfoViewModel model = new ListOfPersonalInfoViewModel
            {
                CompanyLogo = sellerPersonalInfo.CompanyLogo,
                CompanyName = sellerPersonalInfo.CompanyName,
                NationalCode = sellerPersonalInfo.NationalCode,
                PeriodTimesOfWork = sellerPersonalInfo.PeriodTimesOfWork,
                PhotoOfBusinessLicense = sellerPersonalInfo.PhotoOfBusinessLicense,
                PhotoOfNationalCode = sellerPersonalInfo.PhotoOfNationalCode,
                Resume = sellerPersonalInfo.Resume,
                WorkAddress = sellerPersonalInfo.WorkAddress,
                RejectedNote = sellerPersonalInfo.RejectDescription,
                MarketPersonalsInfoState = sellerPersonalInfo.Market.MarketPersonalsInfoState,
                SellerType = sellerPersonalInfo.SellerType,
                CountryId = sellerPersonalInfo.CountryId.Value,
                StateId = sellerPersonalInfo.StateId.Value,
                CityId = sellerPersonalInfo.CityId.Value,
                ActivationTariff = await _context.Market.Where(p => !p.IsDelete && p.UserId == userId).Select(p => p.ActivationTariff).FirstOrDefaultAsync(),
            };

            model.MarketWorkSamples = await _context.MarketWorkSamle.Where(p => !p.IsDelete && p.UserId == userId)
                                        .Select(p => new AddSellerWorkSampleViewModel
                                        {
                                            UserId = userId,
                                            Id = p.Id,
                                            WorkSampleImage = p.WorkSampleImage,
                                            WorkSampleTitle = p.WorkSampleTitle
                                        }).ToListAsync();

            model.MarketLinks = await _context.MarketLinks.Where(p => !p.IsDelete && p.UserId == userId)
                                       .Select(p => new AddSellerLinksViewModel
                                       {
                                           UserId = userId,
                                           Id = p.Id,
                                           LinkTitle = p.LinkTitle,
                                           LinkValue = p.LinkValue
                                       }).ToListAsync();

            model.MarketChargeInfos = await _context.MarketChargeInfo.Where(p => !p.IsDelete && p.UserId == userId).ToListAsync();

            #region Initial Scores

            AddScoreToTheSellerViewModel score = new AddScoreToTheSellerViewModel();

            #region Keyfiate Kar

            var sellerScores = await _context.ScoreForMarkets
                                       .AsNoTracking()
                                       .Where(p => !p.IsDelete && p.UserId == userId)
                                       .ToListAsync();

            if (sellerScores != null && sellerScores.Any())
            {
                var sum = sellerScores.Sum(p => p.Score);
                var count = sellerScores.Count();
                int res = sum / count;

                score.KeyfiateKar = res;
            }

            #endregion

            #region sehat

            var sehat = await _context.SehateEtelaAt
                                                .AsNoTracking()
                                                .Where(p => !p.IsDelete && p.UserId == userId)
                                                .ToListAsync();

            if (sehat != null && sehat.Any())
            {
                score.SehateEtelaAt = sehat.Sum(p => p.Score) / sehat.Count();
            }

            #endregion

            #region pasazForosh

            var pasazForosh = await _context.KhadamatePasAzForosh
                                                       .AsNoTracking()
                                                       .Where(p => !p.IsDelete && p.UserId == userId)
                                                       .ToListAsync();

            if (pasazForosh != null && pasazForosh.Any())
            {
                score.KhadamatePasAzForosh = pasazForosh.Sum(p => p.Score) / pasazForosh.Count();
            }

            #endregion

            #region pasokh Goie

            var pasokhGoie = await _context.PasokhGoie
                                         .AsNoTracking()
                                         .Where(p => !p.IsDelete && p.UserId == userId)
                                         .ToListAsync();

            if (pasokhGoie != null && pasokhGoie.Any())
            {
                score.PasokhGoie = pasokhGoie.Sum(p => p.Score) / pasokhGoie.Count();
            }

            #endregion

            #region taAhode Zaman

            var taAhodeZaman = await _context.TaAhodeZamaneTahvil
                                                      .AsNoTracking()
                                                      .Where(p => !p.IsDelete && p.UserId == userId)
                                                      .ToListAsync();

            if (taAhodeZaman != null && taAhodeZaman.Any())
            {
                score.TaAhodZamaneTahvil = taAhodeZaman.Sum(p => p.Score) / taAhodeZaman.Count();
            }

            #endregion

            if (score != null)
            {
                model.Score = score;
            }

            #endregion

            #endregion

            return model;
        }

        public async Task<ListOfPersonalInfoForInquiryViewModel> FillListOfPersonalInfoForInquiryViewModel(ulong userId)
        {
            #region Validation User

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId && !p.IsDelete);
            if (user == null) return null;

            var sellerPersonalInfo = await _context.MarketPersonalInfo.Include(p => p.Market).FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState != MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (sellerPersonalInfo == null) return null;

            var sellerLinks = await _context.MarketLinks.Where(p => p.UserId == userId && !p.IsDelete).ToListAsync();
            if (sellerLinks == null) return null;

            var sellerWorkSamples = await _context.MarketWorkSamle.Where(p => p.UserId == userId && !p.IsDelete).ToListAsync();
            if (sellerWorkSamples == null) return null;

            #endregion

            #region Fill View Model 

            ListOfPersonalInfoForInquiryViewModel model = new ListOfPersonalInfoForInquiryViewModel
            {
                CompanyLogo = sellerPersonalInfo.CompanyLogo,
                CompanyName = sellerPersonalInfo.CompanyName,
                PeriodTimesOfWork = sellerPersonalInfo.PeriodTimesOfWork,
                PhotoOfBusinessLicense = sellerPersonalInfo.PhotoOfBusinessLicense,
                PhotoOfNationalCode = sellerPersonalInfo.PhotoOfNationalCode,
                Resume = sellerPersonalInfo.Resume,
                WorkAddress = sellerPersonalInfo.WorkAddress,
            };

            model.MarketWorkSamples = await _context.MarketWorkSamle.Where(p => !p.IsDelete && p.UserId == userId)
                                        .Select(p => new AddSellerWorkSampleViewModel
                                        {
                                            UserId = userId,
                                            Id = p.Id,
                                            WorkSampleImage = p.WorkSampleImage,
                                            WorkSampleTitle = p.WorkSampleTitle
                                        }).ToListAsync();

            model.MarketLinks = await _context.MarketLinks.Where(p => !p.IsDelete && p.UserId == userId)
                                       .Select(p => new AddSellerLinksViewModel
                                       {
                                           UserId = userId,
                                           Id = p.Id,
                                           LinkTitle = p.LinkTitle,
                                           LinkValue = p.LinkValue
                                       }).ToListAsync();

            #endregion

            return model;
        }


        public async Task<bool> AddSellerWorkSampleInModal(AddSellerWorkSampleViewModel workSample)
        {
            #region Data Validations 

            if (!await _context.Users.AnyAsync(p => !p.IsDelete && p.Id == workSample.UserId))
            {
                return false;
            }

            #endregion

            #region Get market 

            var market = await GetMarket(workSample.UserId.Value);
            if (market == null) return false;

            #endregion

            #region Add Work Sample 

            MarketWorkSamle sellerWorkSample = new MarketWorkSamle
            {
                UserId = workSample.UserId.Value,
                WorkSampleTitle = workSample.WorkSampleTitle,
                WorkSampleImage = workSample.WorkSampleImage,
                MarketId = market.Id,
            };

            await _context.MarketWorkSamle.AddAsync(sellerWorkSample);
            await _context.SaveChangesAsync();

            #endregion

            #region Update Seller State 

            var seller = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == workSample.UserId);

            seller.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;

            _context.MarketPersonalInfo.Update(seller);
            await _context.SaveChangesAsync();

            #endregion

            #region Update Market 

            market.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        public async Task<UpdateSellerPersonalInfoResul> UpdateSellerPersonalInfoFromSellerPanel(ListOfPersonalInfoViewModel info, ulong userId)
        {
            #region Data Validations

            if (!await _context.Users.AnyAsync(p => p.Id == userId && !p.IsDelete))
            {
                return UpdateSellerPersonalInfoResul.Faild;
            }

            #endregion

            #region update Personal Info 

            #region Get Personal Info 

            var sellerPersonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerPersonalInfo == null) return UpdateSellerPersonalInfoResul.NotFound;

            #endregion

            #region Fill Entities

            sellerPersonalInfo.NationalCode = info.NationalCode;
            sellerPersonalInfo.CompanyName = info.CompanyName;

            sellerPersonalInfo.Resume = info.Resume;
            sellerPersonalInfo.PeriodTimesOfWork = info.PeriodTimesOfWork;
            sellerPersonalInfo.WorkAddress = info.WorkAddress;
            sellerPersonalInfo.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForConfirmPersonalInformations;
            sellerPersonalInfo.SellerType = info.SellerType;
            sellerPersonalInfo.CountryId = info.CountryId;
            sellerPersonalInfo.CityId = info.CityId;
            sellerPersonalInfo.StateId = info.StateId;

            #endregion

            #region Add Images

            if (info.ImageCompanyLogo != null && info.ImageCompanyLogo.IsImage())
            {
                if (!string.IsNullOrEmpty(sellerPersonalInfo.CompanyLogo))
                {
                    sellerPersonalInfo.CompanyLogo.DeleteImage(FilePaths.SellerInfoPathServer, FilePaths.SellerInfoPathThumbServer);
                }

                var companylogo = Guid.NewGuid() + Path.GetExtension(info.ImageCompanyLogo.FileName);
                info.ImageCompanyLogo.AddImageToServer(companylogo, FilePaths.SellerInfoPathServer, 400, 300, FilePaths.SellerInfoPathThumbServer);
                sellerPersonalInfo.CompanyLogo = companylogo;
            }

            if (info.ImageOfBusinessLicense != null && info.ImageOfBusinessLicense.IsImage())
            {
                if (!string.IsNullOrEmpty(sellerPersonalInfo.PhotoOfBusinessLicense))
                {
                    sellerPersonalInfo.PhotoOfBusinessLicense.DeleteImage(FilePaths.SellerInfoPathServer, FilePaths.SellerInfoPathThumbServer);
                }

                var PhotoOfBusinessLicense = Guid.NewGuid() + Path.GetExtension(info.ImageOfBusinessLicense.FileName);
                info.ImageOfBusinessLicense.AddImageToServer(PhotoOfBusinessLicense, FilePaths.SellerInfoPathServer, 400, 300, FilePaths.SellerInfoPathThumbServer);
                sellerPersonalInfo.PhotoOfBusinessLicense = PhotoOfBusinessLicense;
            }

            if (info.ImageOfNationalCode != null && info.ImageOfNationalCode.IsImage())
            {
                if (!string.IsNullOrEmpty(sellerPersonalInfo.PhotoOfNationalCode))
                {
                    sellerPersonalInfo.PhotoOfNationalCode.DeleteImage(FilePaths.SellerInfoPathServer, FilePaths.SellerInfoPathThumbServer);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(info.ImageOfNationalCode.FileName);
                info.ImageOfNationalCode.AddImageToServer(imageName, FilePaths.SellerInfoPathServer, 400, 300, FilePaths.SellerInfoPathThumbServer);
                sellerPersonalInfo.PhotoOfNationalCode = imageName;
            }

            #endregion

            _context.MarketPersonalInfo.Update(sellerPersonalInfo);
            await _context.SaveChangesAsync();

            #endregion

            return UpdateSellerPersonalInfoResul.Success;
        }

        public async Task<bool> CheckUserCharge(ulong UserId)
        {
            #region Get Market By User Id 

            var market = await _context.MarketUser.Include(p => p.Market).Where(p => !p.IsDelete && p.UserId == UserId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            #region Check User Charge 

            if (market.MarketPersonalsInfoState == MarketPersonalsInfoState.ActiveMarketAccount)
            {
                var marketChargeInfo = await _context.MarketChargeInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.CurrentAccountCharge && p.MarketId == market.Id);
                if (marketChargeInfo.EndDate < DateTime.Now)
                {
                    #region Update Market Charge Info

                    marketChargeInfo.CurrentAccountCharge = false;

                    _context.MarketChargeInfo.Update(marketChargeInfo);
                    await _context.SaveChangesAsync();

                    #endregion

                    if (DateTime.Now.Month - marketChargeInfo.EndDate.Month >= 3)
                    {
                        //Delete Market Info 
                        //var marketInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.MarketId == market.Id);
                        //if (marketInfo != null)
                        //{
                        //    marketInfo.IsDelete = true;
                        //    _context.MarketPersonalInfo.Update(marketInfo);
                        //    await _context.SaveChangesAsync();
                        //}

                        //Delete Personal Sample
                        //var marketWorkSamples = await _context.MarketWorkSamle.Where(p => !p.IsDelete && p.MarketId == market.Id).ToListAsync();
                        //if (marketWorkSamples != null)
                        //{
                        //    foreach (var item in marketWorkSamples)
                        //    {
                        //        item.IsDelete = true;
                        //        _context.MarketWorkSamle.Update(item);
                        //        await _context.SaveChangesAsync();
                        //    }
                        //}

                        //Delete Links 
                        //var marketLinks = await _context.MarketLinks.Where(p => !p.IsDelete && p.MarketId == market.Id).ToListAsync();
                        //if (marketLinks != null)
                        //{
                        //    foreach (var item in marketLinks)
                        //    {
                        //        item.IsDelete = true;
                        //        _context.MarketLinks.Update(item);
                        //        await _context.SaveChangesAsync();
                        //    }
                        //}

                        #region Update Market Info 

                        market.MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller;

                        _context.Market.Update(market);
                        await _context.SaveChangesAsync();

                        #endregion
                    }

                    else
                    {
                        #region Update Market Info 

                        market.MarketPersonalsInfoState = MarketPersonalsInfoState.DisAcctiveMarketAccount;

                        _context.Market.Update(market);
                        await _context.SaveChangesAsync();

                        #endregion
                    }
                }
            }

            #endregion

            return true;
        }

        #endregion

        #region Admin Side

        public async Task<Market?> GetMarketByMarketId(ulong marketId)
        {
            return await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketId);
        }

        public async Task<bool> ChangeMarketStateFromAdminPanel(ListOfPersonalInfoViewModel model, ulong marketId)
        {
            #region Model State Validation 

            if (string.IsNullOrEmpty(model.RejectedNote) && model.MarketPersonalsInfoState == MarketPersonalsInfoState.Rejected)
            {
                return false;
            }

            if (model.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller)
            {
                return false;
            }

            #endregion

            #region Get Market 

            var market = await GetMarketByMarketId(marketId);
            if (market == null) return false;

            market.MarketPersonalsInfoState = model.MarketPersonalsInfoState;
            market.ActivationTariff = model.ActivationTariff;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion

            #region Update Market Personal Info State 

            var info = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => !p.IsDelete && p.MarketId == market.Id);
            if (info == null) return false;

            info.MarketPersonalsInfoState = model.MarketPersonalsInfoState;
            info.RejectDescription = model.RejectedNote;

            _context.MarketPersonalInfo.Update(info);
            await _context.SaveChangesAsync();

            return true;

            #endregion
        }

        public async Task<List<MarketPersonalInfo>?> WaitingForAcceptSellerPErsonalInfos()
        {
            return await _context.MarketPersonalInfo
                .Include(u => u.User)
                .Include(p => p.Market)
                .Where(p => !p.IsDelete && p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForConfirmPersonalInformations)
                .OrderByDescending(p => p.CreateDate)
                .ToListAsync();
        }


        public async Task<ListOfSellersInfoViewModel> FilterPersonalInfo(ListOfSellersInfoViewModel filter)
        {
            var query = _context.MarketPersonalInfo
                .Include(u => u.User)
                .Include(p => p.Market)
                .Where(p => !p.IsDelete)
                .OrderByDescending(p => p.CreateDate)
                .AsQueryable();

            #region Order By State 

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForPyedFromSeller)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForPyedFromSeller);
            }

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.DisAcctiveMarketAccount)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.DisAcctiveMarketAccount);
            }

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            }

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.ActiveMarketAccount)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.ActiveMarketAccount);
            }

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.AcceptedPersonalInformation)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.AcceptedPersonalInformation);
            }

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.Rejected)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.Rejected);
            }

            if (filter.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForConfirmPersonalInformations)
            {
                query = query.Where(p => p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForConfirmPersonalInformations);
            }

            #endregion

            switch (filter.OrderType)
            {
                case FilterUserViewModel.FilterUserOrderType.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
                case FilterUserViewModel.FilterUserOrderType.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;
            }

            #region filter

            if ((!string.IsNullOrEmpty(filter.Mobile)))
            {
                query = query.Where(u => u.User.Mobile.Contains(filter.Mobile));
            }
            if ((!string.IsNullOrEmpty(filter.Username)))
            {
                query = query.Where(u => u.User.Username.Contains(filter.Username));
            }

            if (!string.IsNullOrEmpty(filter.FromDate))
            {
                var spliteDate = filter.FromDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate >= fromDate);
            }

            if (!string.IsNullOrEmpty(filter.ToDate))
            {
                var spliteDate = filter.ToDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate <= toDate);
            }

            if (filter.CountryId.HasValue)
            {
                query = query.Where(p => p.CountryId == filter.CountryId);
            }

            if (filter.StateId.HasValue)
            {
                query = query.Where(p => p.StateId == filter.StateId);
            }

            if (filter.CityId.HasValue)
            {
                query = query.Where(p => p.CityId == filter.CityId);
            }

            #endregion

            #region paging

            await filter.Paging(query);

            #endregion

            return filter;
        }

        public async Task<LogForBrandsViewModel> FilterLogForBrands(LogForBrandsViewModel filter)
        {
            var query = _context.LogForBrands
           .Include(u => u.MainBrand)
           .Where(p => !p.IsDelete)
           .OrderByDescending(p => p.CreateDate)
           .AsQueryable();

            switch (filter.OrderType)
            {
                case FilterUserViewModel.FilterUserOrderType.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
                case FilterUserViewModel.FilterUserOrderType.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;
            }

            #region filter

            if ((!string.IsNullOrEmpty(filter.BrandName)))
            {
                query = query.Where(u => u.MainBrand.BrandName.Contains(filter.BrandName));
            }

            if (!string.IsNullOrEmpty(filter.FromDate))
            {
                var spliteDate = filter.FromDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate >= fromDate);
            }

            if (!string.IsNullOrEmpty(filter.ToDate))
            {
                var spliteDate = filter.ToDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate <= toDate);
            }

            if (filter.CountryId.HasValue)
            {
                query = query.Where(p => p.CountryId == filter.CountryId);
            }

            if (filter.StateId.HasValue)
            {
                query = query.Where(p => p.StateId == filter.StateId);
            }

            if (filter.CityId.HasValue)
            {
                query = query.Where(p => p.CityId == filter.CityId);
            }

            #endregion

            #region paging

            await filter.Paging(query);

            #endregion

            return filter;
        }


        public async Task<FilterLogVisitSellerProfileViewModel> FilterLogVisitSellerProfile(FilterLogVisitSellerProfileViewModel filter)
        {
            var query = _context.LogForVisitSellerProfiles
           .Include(u => u.User)
           .ThenInclude(p => p.SellersPersonalInfos)
           .Where(p => !p.IsDelete)
           .OrderByDescending(p => p.CreateDate)
           .AsQueryable();

            switch (filter.OrderType)
            {
                case FilterUserViewModel.FilterUserOrderType.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
                case FilterUserViewModel.FilterUserOrderType.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;
            }

            #region filter

            if ((!string.IsNullOrEmpty(filter.SellerMobile)))
            {
                query = query.Where(u => u.User.Mobile.Contains(filter.SellerMobile));
            }

            if ((!string.IsNullOrEmpty(filter.Username)))
            {
                query = query.Where(u => u.User.Username.Contains(filter.Username));
            }

            if (filter.SellerId.HasValue)
            {
                #region Get Market By User Id 

                var marketPersons = await _context.MarketUser.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == filter.SellerId);

                var market = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == marketPersons.MarketId);

                #endregion

                query = query.Where(u => u.User.Id == market.UserId);
            }

            if (!string.IsNullOrEmpty(filter.FromDate))
            {
                var spliteDate = filter.FromDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate >= fromDate);
            }

            if (!string.IsNullOrEmpty(filter.ToDate))
            {
                var spliteDate = filter.ToDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate <= toDate);
            }

            if (filter.CountryId.HasValue)
            {
                query = query.Where(p => p.User.SellersPersonalInfos.CountryId == filter.CountryId);
            }

            if (filter.StateId.HasValue)
            {
                query = query.Where(p => p.User.SellersPersonalInfos.StateId == filter.StateId);
            }

            if (filter.CityId.HasValue)
            {
                query = query.Where(p => p.User.SellersPersonalInfos.CityId == filter.CityId);
            }

            #endregion

            #region paging

            await filter.Paging(query);

            #endregion

            return filter;
        }

        public async Task<PersonalInfoDetailAdminSideViewModel> PersonalInfoDetail(ulong id)
        {
            #region Get Personal Info

            var personalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);

            #endregion

            #region Fill View Model

            PersonalInfoDetailAdminSideViewModel model = new PersonalInfoDetailAdminSideViewModel
            {
                UserId = personalInfo.UserId,
                User = await _context.Users.FirstOrDefaultAsync(p => p.Id == personalInfo.UserId && !p.IsDelete),
                CompanyLogo = personalInfo?.CompanyLogo,
                CompanyName = personalInfo?.CompanyName,
                NationalCode = personalInfo.NationalCode,
                PeriodTimesOfWork = personalInfo?.PeriodTimesOfWork,
                PhotoOfBusinessLicense = personalInfo?.PhotoOfBusinessLicense,
                PhotoOfNationalCode = personalInfo?.PhotoOfNationalCode,
                RejectedNote = personalInfo.RejectDescription,
                Resume = personalInfo.Resume,
                MarketLinks = await _context.MarketLinks.Where(p => p.UserId == personalInfo.UserId && !p.IsDelete).ToListAsync(),
                MarketWorkSamples = await _context.MarketWorkSamle.Where(p => p.UserId == personalInfo.UserId && !p.IsDelete).ToListAsync(),
                MarketPersonalsInfoState = personalInfo.MarketPersonalsInfoState,
                WorkAddress = personalInfo.WorkAddress
            };

            #endregion

            return model;
        }

        public async Task<FilterLogForInquiryViewModel> FilterLogForInquiryViewModel(FilterLogForInquiryViewModel filter)
        {
            var query = _context.LogForInquiry
           .Include(u => u.State)
           .Include(p => p.Country)
           .Include(p => p.City)
           .Where(p => !p.IsDelete)
           .OrderByDescending(p => p.CreateDate)
           .AsQueryable();

            switch (filter.OrderType)
            {
                case FilterUserViewModel.FilterUserOrderType.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
                case FilterUserViewModel.FilterUserOrderType.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;
            }

            #region filter

            if (!string.IsNullOrEmpty(filter.FromDate))
            {
                var spliteDate = filter.FromDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate >= fromDate);
            }

            if (!string.IsNullOrEmpty(filter.ToDate))
            {
                var spliteDate = filter.ToDate.Split('/');
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

                query = query.Where(s => s.CreateDate <= toDate);
            }


            if (filter.CountryId.HasValue)
            {
                query = query.Where(p => p.CountryId == filter.CountryId);
            }

            if (filter.StateId.HasValue)
            {
                query = query.Where(p => p.StateId == filter.StateId);
            }

            if (filter.CityId.HasValue)
            {
                query = query.Where(p => p.CityId == filter.CityId);
            }

            if (filter.SellerType == Domain.Enums.SellerType.SellerType.Aluminium)
            {
                query = query.Where(p => p.SellerType == Domain.Enums.SellerType.SellerType.Aluminium);
            }

            if (filter.SellerType == Domain.Enums.SellerType.SellerType.UPVCAlminium)
            {
                query = query.Where(p => p.SellerType == Domain.Enums.SellerType.SellerType.UPVCAlminium);
            }

            if (filter.SellerType == Domain.Enums.SellerType.SellerType.UPC)
            {
                query = query.Where(p => p.SellerType == Domain.Enums.SellerType.SellerType.UPC);
            }

            #endregion

            #region paging

            await filter.Paging(query);

            #endregion

            return filter;
        }

        #endregion

        #region Site Side 

        //Update Seller Activation Tariff After Seen Seller Profile By User 
        public async Task UpdateSellerActivationTariff(ulong userId, bool listOfInquiry, bool sellerDetail)
        {
            var tariffValue = 0;

            #region Get Market By Seller Id 

            var market = await _context.Market
                                       .FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);

            #endregion

            #region Update Activation Tariff

            if (listOfInquiry == true)
            {
                var listOfInquiryTariff = await _context.SiteSettings
                                                        .AsNoTracking()
                                                        .Where(p => !p.IsDelete)
                                                        .Select(p => p.ChargeTariffAboutListOfInquiry)
                                                        .FirstOrDefaultAsync();
                if (listOfInquiryTariff != 0)
                {
                    tariffValue = listOfInquiryTariff;
                }
            }

            if (sellerDetail == true)
            {
                var sellerDetailTariff = await _context.SiteSettings
                                                        .AsNoTracking()
                                                        .Where(p => !p.IsDelete)
                                                        .Select(p => p.ChargeTariffAboutSellerDetail)
                                                        .FirstOrDefaultAsync();
                if (sellerDetailTariff != 0)
                {
                    tariffValue = sellerDetailTariff;
                }
            }

            market.ActivationTariff = market.ActivationTariff + tariffValue;

            _context.Market.Update(market);
            await _context.SaveChangesAsync();

            #endregion
        }

        #endregion
    }
}
