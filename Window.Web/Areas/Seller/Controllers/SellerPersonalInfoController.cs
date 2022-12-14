using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Window.Application.Convertors;
using Window.Application.Extensions;
using Window.Application.Generators;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.DTOs.ZarinPal;
using Window.Domain.Entities.Wallet;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Seller.PersonalInfo;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Seller.Controllers
{
    public class SellerPersonalInfoController : SellerBaseController
    {
        #region Ctor

        private readonly ISellerService _sellerService;

        private readonly IStateService _stateService;

        private readonly IWalletService _walletService;

        public SellerPersonalInfoController(ISellerService sellerService , IStateService stateService , IWalletService walletService)
        {
            _sellerService = sellerService;
            _stateService = stateService;
            _walletService = walletService;
        }

        #endregion

        #region Manage Link For Redirect To Currect Action

        public async Task<IActionResult> ManagePersonalInfoLink()
        {
            #region Is User Fill All Of Datas

            if (await _sellerService.IsUserFillAllOfPersonalInfoFiles(User.GetUserId()))
            {
                return RedirectToAction(nameof(ListOfPersonalInfo));
            }

            #endregion

            #region Is User Fill Just Personal Info

            if (await _sellerService.IsUserJustFillUserPersonalInfo(User.GetUserId()))
            {
                return RedirectToAction(nameof(AddSellerPersonalLinks));
            }

            #endregion

            #region Is User Just Fill Seller Personal Links 

            if (await _sellerService.IsUserJustFillUserPersonalLinks(User.GetUserId()))
            {
                return RedirectToAction(nameof(AddSellerWorkSampleTitle));
            }

            #endregion

            return RedirectToAction(nameof(AddPersonalInfo));
        }

        #endregion

        #region Page Of Seller Information

        public async Task<IActionResult> PageOfManageSellerInformation()
        {
            #region Check User Charge

            var res = await _sellerService.CheckUserCharge(User.GetUserId());
            if (res == false) return NotFound();

            #endregion

            var model = await _sellerService.FillInformationOfSellerStateViewModel(User.GetUserId());
            return View(model) ;
        }

        #endregion

        #region Add Pesonal Informations

        [HttpGet]
        public async Task<IActionResult> AddPersonalInfo()
        {
            #region Is Exist Personal Info

            if (await _sellerService.IsExistSellerPersonalInformations(User.GetUserId()))
            {
                return RedirectToAction();
            }

            #endregion

            ViewData["Countries"] = await _stateService.GetAllCountries();

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPersonalInfo(AddSellerPersonalInfoViewModel info)
        {
            #region Is Exist Personal Info

            if (await _sellerService.IsExistSellerPersonalInformations(User.GetUserId()))
            {
                return RedirectToAction();
            }

            #endregion

            #region Model State Valid

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد .";
                ViewData["Countries"] = await _stateService.GetAllCountries();
                ViewData["States"] = await _stateService.GetStateChildren(info.CountryId);
                ViewData["Cities"] = await _stateService.GetStateChildren(info.StateId);

                return View(info);
            }

            #endregion

            #region Add Personal Info Methods

            var result = await _sellerService.AddSellerPersonalInfo(info, User.GetUserId());

            switch (result)
            {
                case AddSellerPersonalInfoResul.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                    return RedirectToAction(nameof(AddSellerPersonalLinks));

                case AddSellerPersonalInfoResul.Faild:
                    TempData[ErrorMessage] = "عملیات با شکست انجام شده است .";
                    break;

                case AddSellerPersonalInfoResul.ImagesNotFound:
                    TempData[ErrorMessage] = "تصاویر در خواستی باید وارد شوند .";
                    break;
            };

            #endregion

            ViewData["Countries"] = await _stateService.GetAllCountries();
            ViewData["States"] = await _stateService.GetStateChildren(info.CountryId);
            ViewData["Cities"] = await _stateService.GetStateChildren(info.StateId);

            return View(info);
        }

        #endregion

        #region Add Seller Personal Links 

        [HttpGet]
        public async Task<IActionResult> AddSellerPersonalLinks()
        {
            #region Validate User Informations

            if (!await _sellerService.IsExistSellerPersonalInformations(User.GetUserId()))
            {
                TempData[ErrorMessage] = "کاربر عزیز ابتدا باید اطلاعات شخصی را پر کنید .";
                return RedirectToAction(nameof(AddPersonalInfo));
            }

            #endregion

            //Get Seller Links 
            ViewBag.SellerLinks = await _sellerService.GetSellerLinksForShowInAddSellerLinksPage(User.GetUserId());

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSellerPersonalLinks(AddSellerLinksViewModel links)
        {
            //Get Seller Links 
            ViewBag.SellerLinks = await _sellerService.GetSellerLinksForShowInAddSellerLinksPage(User.GetUserId());

            #region Validate User Informations

            if (!await _sellerService.IsExistSellerPersonalInformations(User.GetUserId()))
            {
                TempData[ErrorMessage] = "کاربر عزیز ابتدا باید اطلاعات شخصی را پر کنید .";
                return RedirectToAction(nameof(AddPersonalInfo));
            }

            #endregion

            #region Model State Validations

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد .";
                return View();
            }

            #endregion

            #region Add Link Method 

            links.UserId = User.GetUserId();

            var result = await _sellerService.AddSellerLinksFromSeller(links);

            switch (result)
            {
                case AddSellerLinksResult.Success:
                    TempData[SuccessMessage] = "اطلاعات لینک ما با موفقیت اضافه شده است .";
                    return RedirectToAction(nameof(AddSellerPersonalLinks));

                case AddSellerLinksResult.Faild:
                    TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                    break;
            }

            #endregion

            return View(links);
        }

        #endregion

        #region Delete Seller Personal Links 

        public async Task<IActionResult> DeleteSellersLinks(ulong id)
        {
            var result = await _sellerService.DeleteSellerLink(id, User.GetUserId());

            if (result)
            {
                return ApiResponse.SetResponse(ApiResponseStatus.Success, null, "عملیات با موفقیت انجام شده است .");
            }

            return ApiResponse.SetResponse(ApiResponseStatus.Danger, null, "عملیات با شکست مواجه شده است .");
        }

        #endregion

        #region Add Seller Work Sample Title 

        [HttpGet]
        public async Task<IActionResult> AddSellerWorkSampleTitle()
        {
            //Get Seller Work Samples 
            ViewBag.SellerWorkSamples = await _sellerService.GetSellerWorkSampleForShowInAddSellerLinksPage(User.GetUserId());

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSellerWorkSampleTitle(AddSellerWorkSampleViewModel workSample, IFormFile workSampleImage)
        {
            //Get Seller Work Samples 
            ViewBag.SellerWorkSamples = await _sellerService.GetSellerWorkSampleForShowInAddSellerLinksPage(User.GetUserId());

            #region Model State Validaton 

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد .";
                return View(workSample);
            }

            #endregion

            #region Add Personal Work Sample

            workSample.UserId = User.GetUserId();

            var result = await _sellerService.AddSellerWorkSample(workSample, workSampleImage);

            switch (result)
            {
                case AddSellerWorkSampleResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                    return RedirectToAction(nameof(AddSellerWorkSampleTitle));

                case AddSellerWorkSampleResult.ImageNotFound:
                    TempData[ErrorMessage] = "تصویر باید وارد شود .";
                    break;

                case AddSellerWorkSampleResult.Faild:
                    TempData[ErrorMessage] = "عملیات با شکست مواجه شده است .";
                    break;
            }

            #endregion

            return View(workSample);
        }

        #endregion

        #region Delete Work Sample 

        public async Task<IActionResult> DeleteWorkSample(ulong id)
        {
            var result = await _sellerService.DeleteSellerWorkSample(id, User.GetUserId());

            if (result)
            {
                return ApiResponse.SetResponse(ApiResponseStatus.Success, null, "عملیات با موفقیت انجام شده است .");
            }

            return ApiResponse.SetResponse(ApiResponseStatus.Danger, null, "عملیات با شکست مواجه شده است .");
        }

        #endregion

        #region List Of Personal Info 

        public async Task<IActionResult> ListOfPersonalInfo()
        {
            var model = await _sellerService.FillListOfPersonalInfoViewModel(User.GetUserId());

            if (model == null) return RedirectToAction(nameof(ManagePersonalInfoLink));

            ViewData["Countries"] = await _stateService.GetAllCountries();
            ViewData["States"] = await _stateService.GetStateChildren(model.CountryId);
            ViewData["Cities"] = await _stateService.GetStateChildren(model.StateId);

            return View(model);
        }

        #endregion

        #region Show Personal Link Form In Modal

        [HttpGet("Show-AddPErsonalLink-Modal")]
        public async Task<IActionResult> ShowAddPersonalInfoInModal()
        {
            return PartialView("_ShowAddPersonalInfoInModal");
        }

        #endregion

        #region Add Personal Link In Modal 

        public async Task<IActionResult> AddPersonalInfoInModal(AddSellerLinksViewModel link)
        {
            link.UserId = User.GetUserId();

            var res = await _sellerService.AddSellerLinksFromSeller(link);
            if (res == AddSellerLinksResult.Success)
            {
                await _sellerService.UpdateSellerStateAfterEditPersonalInfo(User.GetUserId());
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Personal Work Sample In Modal

        [HttpGet("Show-AddPersonalWorkSample-Modal")]
        public async Task<IActionResult> ShowAddPersonalWorlSampleInModal()
        {
            return PartialView("_ShowAddPersonalWorkSampleModal");
        }

        #endregion

        #region Upload Work Sample Image In Modal

        [HttpPost]
        public async Task<IActionResult> UploadSellerWorkSampleImgeInModal(IFormFile file)
        {
            if (file != null)
            {
                if (Path.GetExtension(file.FileName) == ".png" || Path.GetExtension(file.FileName) == ".jpeg" || Path.GetExtension(file.FileName) == ".jpg")
                {
                    var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(file.FileName);
                    file.AddImageToServer(imageName, FilePaths.SellerInfoPathServer, 270, 270, FilePaths.SellerInfoPathThumbServer);
                    return new JsonResult(new { status = "Success", imageName = imageName });

                }
                else
                {
                    return new JsonResult(new { status = "Error" });
                }
            }
            else
            {
                return new JsonResult(new { status = "Error" });
            }
        }

        #endregion

        #region Add Personal Work Sample In Modal 

        public async Task<IActionResult> AddPersonalWorkInSampleInModal(AddSellerWorkSampleViewModel link)
        {
            link.UserId = User.GetUserId();

            var res = await _sellerService.AddSellerWorkSampleInModal(link);
            if (res)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Update Seller Personal Info 

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSellerPersonlInfo(ListOfPersonalInfoViewModel model)
        {
            #region Update Personal Info Method

            var res = await _sellerService.UpdateSellerPersonalInfoFromSellerPanel(model , User.GetUserId());

            switch (res)
            {
                case UpdateSellerPersonalInfoResul.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                    return RedirectToAction(nameof(ListOfPersonalInfo));

                case UpdateSellerPersonalInfoResul.Faild:
                    TempData[ErrorMessage] = "عملیات با شکست انجام شده است .";
                    break;

                case UpdateSellerPersonalInfoResul.ImagesNotFound:
                    TempData[ErrorMessage] = "تصاویر در خواستی باید وارد شوند .";
                    break;

                case UpdateSellerPersonalInfoResul.NotFound:
                    TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشده است .";
                    return RedirectToAction(nameof(ManagePersonalInfoLink));
            };

            #endregion

            ViewData["Countries"] = await _stateService.GetAllCountries();
            ViewData["States"] = await _stateService.GetStateChildren(model.CountryId);
            ViewData["Cities"] = await _stateService.GetStateChildren(model.StateId);

            return RedirectToAction(nameof(ListOfPersonalInfo));
        }

        #endregion

        #region Market Charge Information 

        public async Task<IActionResult> MarketChargeInformation()
        {
            #region Check User Charge

            var res = await _sellerService.CheckUserCharge(User.GetUserId());
            if (res == false) return NotFound();

            #endregion

            return View(await _sellerService.FillMarketChargeInfoViewModel(User.GetUserId()));
        }

        #endregion

        #region Charge Account

        [HttpGet]
        public async Task<IActionResult> BankPay()
        {
            #region Check User Account Charge Status

            if (await _sellerService.HasMarketStateForPayAccountCharge(User.GetUserId()) == false)
            {
                TempData[ErrorMessage] = "کاربر عزیز امکان پرداخت وجه برای شما وجود ندارد لطفا با پشتیبانی تماس بگیرید.";
                return RedirectToAction(nameof(MarketChargeInformation));
            }

            #endregion

            #region Get Market 

            var market = await _sellerService.GetMarketByUserId(User.GetUserId());
            if(market == null) return NotFound();

            #endregion

            #region Tarif 

            var tariff = await _sellerService.GetMarketAccountChargeTariff(User.GetUserId());
            if (tariff == null)
            {
                TempData[ErrorMessage] = "کاربر عزیز امکان پرداخت وجه برای شما وجود ندارد لطفا با پشتیبانی تماس بگیرید.";
                return RedirectToAction(nameof(MarketChargeInformation));
            }
            if (tariff == 0)
            {
                //Charge User Wallet
                await _sellerService.ChargeUserWallet(User.GetUserId(), tariff.Value);

                //Pay Account Charge Tariff
                await _sellerService.PayAccountChargeTariff(User.GetUserId(), tariff.Value);

                //Update Market State 
                var result = await _sellerService.ChargeAccount(market.Id, User.GetUserId());

                return RedirectToAction("Index" , "Home" , new { area = "Seller" });
            }

            #endregion

            #region Online Payment

            return RedirectToAction("PaymentMethod", "Payment", new
            {
                area = "",
                gatewayType = GatewayType.Zarinpal,
                amount = tariff.Value,
                description = "شارژ حساب کاربری ",
                returURL = $"{FilePaths.SiteAddress}/ChargeAccount/" + market.Id,
            });

            #endregion
        }

        #endregion

        #region Bank  Payment

        [Route("ChargeAccount/{id}")]
        public async Task<IActionResult> ChargeAccount(ulong id)
        {
            #region Get Market 

            var market = await _sellerService.GetMarketByMarketId(id);
            if (market == null) return NotFound();

            #endregion

            try
            {
                #region Check User Account Charge Status

                if (await _sellerService.HasMarketStateForPayAccountCharge(User.GetUserId()) == false)
                {
                    TempData[ErrorMessage] = "کاربر عزیز امکان پرداخت وجه برای شما وجود ندارد لطفا با پشتیبانی تماس بگیرید.";
                    return RedirectToAction(nameof(MarketChargeInformation));
                }

                #endregion

                #region Fill Parametrs

                VerifyParameters parameters = new VerifyParameters();

                if (HttpContext.Request.Query["Authority"] != "")
                {
                    parameters.authority = HttpContext.Request.Query["Authority"];
                }

                #region Tarif 

                var tariff = await _sellerService.GetMarketAccountChargeTariff(User.GetUserId());
                if (tariff == null)
                {
                    TempData[ErrorMessage] = "کاربر عزیز امکان پرداخت وجه برای شما وجود ندارد لطفا با پشتیبانی تماس بگیرید.";
                    return RedirectToAction(nameof(MarketChargeInformation));
                }

                #endregion

                parameters.amount = tariff.ToString();
                parameters.merchant_id = FilePaths.merchant;

                #endregion

                using (HttpClient client = new HttpClient())
                {
                    #region Verify Payment

                    var json = JsonConvert.SerializeObject(parameters);

                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(URLs.verifyUrl, content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    JObject jodata = JObject.Parse(responseBody);

                    string data = jodata["data"].ToString();

                    JObject jo = JObject.Parse(responseBody);

                    string errors = jo["errors"].ToString();

                    #endregion

                    if (data != "[]")
                    {
                        //Authority Code
                        string refid = jodata["data"]["ref_id"].ToString();

                        //Get Wallet Transaction For Validation 
                        var wallet = await _walletService.FindWalletTransactionForRedirectToTheBankPortal(User.GetUserId(), GatewayType.Zarinpal, parameters.authority, tariff.Value);

                        if (wallet != null)
                        {
                            //Update Market State 
                            var result = await _sellerService.ChargeAccount(id, User.GetUserId());

                            await _walletService.UpdateWalletAndCalculateUserBalanceAfterBankingPayment(wallet);

                            //Pay Tariff
                            await _sellerService.PayHomeVisitTariff(User.GetUserId(), tariff.Value);

                            return RedirectToAction("PaymentResult", "Payment", new { IsSuccess = true, refId = refid });
                        }
                    }
                    else if (errors != "[]")
                    {
                        string errorscode = jo["errors"]["code"].ToString();

                        return BadRequest($"error code {errorscode}");

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return NotFound();
        }

        #endregion
    }
}
