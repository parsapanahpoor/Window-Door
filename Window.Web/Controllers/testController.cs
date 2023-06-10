using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Web.Controllers
{
    [Authorize]
    public class testController : SiteBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        private readonly IStateService _stateService;

        private readonly IBrandService _brandService;

        private readonly ISampleService _sampleService;

        private readonly IInquiryService _inquiryService;

        private readonly ISellerService _sellerService;

        public testController(IProductService prodcutService, IStateService stateService, IBrandService brandService
            , ISampleService sampleService, IInquiryService inquiryService, ISellerService sellerService)
        {
            _productService = prodcutService;
            _stateService = stateService;
            _brandService = brandService;
            _sampleService = sampleService;
            _inquiryService = inquiryService;
            _sellerService = sellerService;
        }

        #endregion

        #region Test For Step 1

        public async Task<IActionResult> InquiryStep1(FilterInquiryViewModel filter)
        {
            #region Location ViewBags 

            ViewData["Countries"] = await _stateService.GetAllCountries();

            if (filter.CountryId != null)
            {
                ViewData["States"] = await _stateService.GetStateChildren(filter.CountryId.Value);
                if (filter.StateId != null)
                {
                    ViewData["Cities"] = await _stateService.GetStateChildren(filter.StateId.Value);
                }
            }

            #endregion

            #region Brand ViewBag

            ViewBag.Brand = await _brandService.GetAllBrands();

            #endregion

            #region Glasses ViewBag

            ViewBag.Glasses = await _productService.GetAllGlasses();

            #endregion

            ViewBag.UserId = User.GetUserId();

            return View();
        }

        #endregion

        #region Test For Step 2

        public async Task<IActionResult> InquiryStep2(ulong CountryId, ulong StateId, ulong CityId, ProductType? ProductType, ProductKind? ProductKind, SellerType? SellerType, ulong? MainBrandId, ulong? GlassId, string UserMacAddress)
        {
            #region Model State Valdiation

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا اطلاعات را کامل وارد کنید";
                return RedirectToAction(nameof(InquiryStep1));
            }

            #endregion

            #region Step 1 Log For User 

            FilterInquiryViewModel log = new FilterInquiryViewModel()
            {
                CountryId = CountryId,
                StateId = StateId,
                CityId = CityId,
                MainBrandId = MainBrandId,
                UserMacAddress = UserMacAddress,
                SellerType = SellerType,
                GlassId = GlassId
            };

            #endregion

            #region Proccess Step 1 Log 

            await _inquiryService.LogInquiryForUserPart1(log);

            #endregion

            return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = UserMacAddress });
        }

        #endregion

        #region Inquiry Step 3

        [HttpGet]
        public async Task<IActionResult> InquiryStep3(string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            #endregion

            ViewBag.UserMacAddress = userMacAddress;

            return View(samples);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> InquiryStep3(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            #endregion

            #region Check Is Exist Sample 

            var sample = await _sampleService.GetSampleBySampleId(sampleId);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            if (sample.MaxHeight < height)
            {
                TempData[ErrorMessage] = "ارتفاع وارد شده بیشتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MinHeight > height)
            {
                TempData[ErrorMessage] = "ارتفاع وارد شده کمتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MaxWidth < width)
            {
                TempData[ErrorMessage] = "عرض وارد شده بیشتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MinWidth > width)
            {
                TempData[ErrorMessage] = "عرض وارد شده کمتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }

            #endregion

            #region Add Log For User

            var res = await _inquiryService.LogInquiryForUserPart2(sampleId, width, height, katibeSize, userMacAddress, SampleCount);
            if (!res) return NotFound();

            #endregion

            ViewBag.UserMacAddress = userMacAddress;

            return View(samples);
        }

        #endregion

        #region Inquiry Step 4 (proccess inquiry)

        public async Task<IActionResult> InquiryStep4(string userMacAddress, string? brandTitle, int? orderByPrice, int? orderByScore, int pageId = 1)
        {
            return View();
        }

        #endregion

        #region Show Seller Persoanl Inormation 

        public async Task<IActionResult> ShowSellerPersoanlInfo(ulong userId)
        {
            #region Fill Model 

            var model = await _sellerService.FillListOfPersonalInfoViewModel(userId);

            if (model == null) return NotFound();

            ViewData["Countries"] = await _stateService.GetAllCountries();
            ViewData["States"] = await _stateService.GetStateChildren(model.CountryId);
            ViewData["Cities"] = await _stateService.GetStateChildren(model.StateId);

            #endregion

            #region Update Seller Activation Tariff

            await _sellerService.UpdateSellerActivationTariff(userId, false, true);

            #endregion

            #region Send SMS

            //var res = await _sellerService.SendSMSForSellerForSeenProfile(userId);

            //if (res == false)
            //{
            //    return NotFound();
            //}

            #endregion

            #region Log For Visit Seller Profile

            await _sellerService.LogForSellerVisitProfile(userId);

            #endregion

            return View(model);
        }

        #endregion

        #region Last Inquiry Base On User Log 

        public async Task<IActionResult> LastUserInquryBaseOnUserLog()
        {
            //Get User ID 
            var userId = User.GetUserId();

            return RedirectToAction(nameof(InquiryStep4), new { userMacAddress = userId });
        }

        #endregion

        #region Add Score For Seller

        [HttpGet]
        public async Task<IActionResult> AddScoreToSeller(ulong sellerId)
        {
            #region Check Is User Was Scored To Seller

            //if (await _inquiryService.checkIsUserScoredToSeller(User.GetUserId().ToString() , sellerId))
            //{
            //    TempData[ErrorMessage] = "شما درگذشته امتیاز خود را برای این فروشنده ثبت کرده اید.";
            //    return RedirectToAction("Index", "Home");
            //}

            #endregion

            ViewBag.sellerId = sellerId;

            return View(new AddScoreToTheSellerViewModel()
            {
                SellerId = sellerId
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddScoreToSeller(AddScoreToTheSellerViewModel model)
        {
            #region Check Is User Was Scored To Seller

            //if (await _inquiryService.checkIsUserScoredToSeller(User.GetUserId().ToString(), sellerId))
            //{
            //    TempData[ErrorMessage] = "شما درگذشته امتیاز خود را برای این فروشنده ثبت کرده اید.";
            //    return RedirectToAction("LastUserInquryBaseOnUserLog", "test");
            //}

            #endregion

            #region Add Score For Seller

            if (model.SehateEtelaAt > 5 || model.SehateEtelaAt < 0
                || model.PasokhGoie > 5 || model.PasokhGoie < 0
                || model.TaAhodZamaneTahvil > 5 || model.TaAhodZamaneTahvil < 0
                || model.KeyfiateKar > 5 || model.KeyfiateKar < 0
                || model.KhadamatePasAzForosh > 5 || model.KhadamatePasAzForosh < 0)
            {
                TempData[ErrorMessage] = "امتیاز وارد شده صحیح نمی باشد";
                return RedirectToAction("Index", "Home");
            }

            var res = await _inquiryService.AddScoreForSeller(model, User.GetUserId().ToString());

            if (res)
            {
                TempData[SuccessMessage] = "امتیاز شما برای فروشنده باموفقیت انجام شده است";
                return RedirectToAction("Index", "Home");
            }

            #endregion

            return View(model);
        }

        #endregion

        #region Delete User Lastest Inquiry Detail

        public async Task<IActionResult> DeleteUserLasteInquiryDetail(ulong Id)
        {
            #region Remove Method

            var res = await _inquiryService.DeleteUserLastestInquiryDetail(Id, User.GetUserId().ToString());
            if (res == false)
            {
                TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
                return RedirectToAction("Index", "Home");
            }

            #endregion

            return RedirectToAction(nameof(GetUserLastestInquiry) , new { userMacAddress = User.GetUserId().ToString() });
        }

        #endregion

        #region Get Lastest User Iquiry 

        [HttpGet]
        public async Task<IActionResult> GetUserLastestInquiry(string userMacAddress)
        {
            #region Get List Of Lastest Inquiry

            var res = await _inquiryService.GetUserLastestInquiryDetailForChange(User.GetUserId().ToString());
            if (res == null)
            {
                TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
                return RedirectToAction("Index", "Home");
            }

            #endregion

            ViewBag.UserMacAddress = User.GetUserId().ToString();

            return View(res);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserLastestInquiry(ulong inquiryDetailId, ulong sampleId, int width, int height, int? katibeSize, string userMacAddress , int? SampleCount)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            #endregion

            #region Check Is Exist Sample 

            var sample = await _sampleService.GetSampleBySampleId(sampleId);
            if (samples == null || !samples.Any())
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }
            if (sample == null)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
                return NotFound();
            }

            if (sample.MaxHeight < height)
            {
                TempData[ErrorMessage] = "ارتفاع وارد شده بیشتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MinHeight > height)
            {
                TempData[ErrorMessage] = "ارتفاع وارد شده کمتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MaxWidth < width)
            {
                TempData[ErrorMessage] = "عرض وارد شده بیشتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }
            if (sample.MinWidth > width)
            {
                TempData[ErrorMessage] = "عرض وارد شده کمتر از حد مجاز است .";
                return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
            }

            #endregion

            #region Add Log For User

            var res = await _inquiryService.UpdateUserInquiryItrm(inquiryDetailId , sampleId, width, height, katibeSize, userMacAddress , SampleCount);
            if (!res)
            {
                TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
                return RedirectToAction("Index", "Home");
            }

            #endregion

            ViewBag.UserMacAddress = User.GetUserId().ToString();

            return RedirectToAction(nameof(GetUserLastestInquiry));
        }

        #endregion
    }
}
