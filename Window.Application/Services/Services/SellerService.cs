using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Extensions;
using Window.Application.Generators;
using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Data.Context;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.Wallet;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Admin.PersonalInfo;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Application.Services.Services
{
    public class SellerService : ISellerService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        private readonly IUserService _userService;

        private readonly IWalletRepository _walletRepository;

        public SellerService(WindowDbContext context , IUserService userService , IWalletRepository walletRepository)
        {
            _context = context;
            _userService = userService;
            _walletRepository = walletRepository;
        }

        #endregion

        #region Seller Panel

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
            if (!await _context.Users.AnyAsync(p=> !p.IsDelete && p.Id == userId))
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

        public async Task<bool> ChargeAccount(ulong marketId , ulong userId)
        {
            #region Get MArket 

            var market = await GetMarketByMarketId(marketId);
            if (market == null) return false;

            #endregion

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

            var marketUser = await  _context.MarketUser.Include(p => p.Market).FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);
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
                model.DaysOfCharge = charge.EndDate.DayOfYear - charge.StartDate.DayOfYear;
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
            //If Seller Registers Now
            if (!await _context.Market.AnyAsync(p => !p.IsDelete && p.UserId == userId)) return "NewUser";

            //If Seller State Is WatingForConfirm Informations
            if (await _context.Market.AnyAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.WaitingForConfirmPersonalInformations)) return "WatingForConfirmInformations";

            //If Seller State Is Waiting For Payed Money From Seller
            if (await _context.Market.AnyAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.WaitingForPyedFromSeller)) return "WaitingForPyedFromSeller";

            //If Seller State Is Accepted And Active
            if (await _context.Market.AnyAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.ActiveMarketAccount)) return "ActiveSellerAccount";

            //If Seller Informations State Is Accepted 
            if (await _context.Market.AnyAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.AcceptedPersonalInformation)) return "AcceptedPersonalInformation";

            //If Seller Informations State Is DisActived 
            if (await _context.Market.AnyAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.Rejected)) return "Rejected";

            //If Seller State Is DisActived
            if (await _context.Market.AnyAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == Domain.Entities.Market.MarketPersonalsInfoState.DisAcctiveMarketAccount)) return "DisAcctiveSellerAccount";

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

            if (await _context.MarketPersonalInfo.AnyAsync(p=> p.UserId == userId && !p.IsDelete))
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

            var sellerLinks = await _context.MarketLinks.FirstOrDefaultAsync(p =>p.UserId == userId && !p.IsDelete);
            if (sellerLinks == null) return false;

            var sellerWorkSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerWorkSample == null) return false;

            return true;
        }

        public async Task<bool> IsUserJustFillUserPersonalInfo(ulong userId)
        {
            var pesonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p =>p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            if (pesonalInfo == null) return false;

            var sellerLinks = await _context.MarketLinks.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerLinks != null) return false;

            var sellerWorkSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete);
            if (sellerWorkSample != null) return false;

            return true;
        }

        public async Task<bool> IsUserJustFillUserPersonalLinks(ulong userId)
        {
            var pesonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p =>  p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState == MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller); ;
            if (pesonalInfo == null) return false;

            var sellerLinks = await _context.MarketLinks.FirstOrDefaultAsync(p =>  p.UserId == userId && !p.IsDelete);
            if (sellerLinks == null) return false;

            var sellerWorkSample = await _context.MarketWorkSamle.FirstOrDefaultAsync(p =>  p.UserId == userId && !p.IsDelete);
            if (sellerWorkSample != null) return false;

            return true;
        }

        public async Task<ListOfPersonalInfoViewModel> FillListOfPersonalInfoViewModel(ulong userId)
        {
            #region Validation User

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId && !p.IsDelete);
            if (user == null) return null;

            var sellerPersonalInfo = await _context.MarketPersonalInfo.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDelete && p.MarketPersonalsInfoState != MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller);
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
                MarketPersonalsInfoState = sellerPersonalInfo.MarketPersonalsInfoState,
                SellerType = sellerPersonalInfo.SellerType,
                CountryId = sellerPersonalInfo.CountryId.Value,
                StateId = sellerPersonalInfo.StateId.Value,
                CityId = sellerPersonalInfo.CityId.Value,
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

        public async Task<ListOfSellersInfoViewModel> FilterPersonalInfo(ListOfSellersInfoViewModel filter)
        {
            var query = _context.MarketPersonalInfo
                .Include(u => u.User)
                .Include(p=>p.Market)
                .Where(p=>!p.IsDelete)
                .OrderByDescending(p=>p.CreateDate)
                .AsQueryable();

            #region Order By State 

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.WaitingForPyedFromSeller)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.WaitingForPyedFromSeller);
            //}

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.DisAcctiveSellerAccount)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.DisAcctiveSellerAccount);
            //}

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.WaitingForCompleteInfoFromSeller)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.WaitingForCompleteInfoFromSeller);
            //}

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.ActiveSellerAccount)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.ActiveSellerAccount);
            //}

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.AcceptedPersonalInformation)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.AcceptedPersonalInformation);
            //}

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.Rejected)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.Rejected);
            //}

            //if (filter.SellersPersonalsInfoState == SellersPersonalsInfoState.WaitingForConfirmPersonalInformations)
            //{
            //    query = query.Where(p => p.SellersPersonalsInfoState == SellersPersonalsInfoState.WaitingForConfirmPersonalInformations);
            //}

            #endregion

            #region filter

            if ((!string.IsNullOrEmpty(filter.Email)))
            {
                query = query.Where(u => u.User.Email.Contains(filter.Email));
            }
            if ((!string.IsNullOrEmpty(filter.Mobile)))
            {
                query = query.Where(u => u.User.Mobile.Contains(filter.Mobile));
            }
            if ((!string.IsNullOrEmpty(filter.Username)))
            {
                query = query.Where(u => u.User.Username.Contains(filter.Username));
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
                MarketWorkSamples = await _context.MarketWorkSamle.Where(p=> p.UserId == personalInfo.UserId && !p.IsDelete).ToListAsync(),
                MarketPersonalsInfoState = personalInfo.MarketPersonalsInfoState,
                WorkAddress = personalInfo.WorkAddress
            };

            #endregion

            return model;
        }

        #endregion
    }
}
