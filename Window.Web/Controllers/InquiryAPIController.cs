using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Window.Application.Convertors;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.Entities.Glass;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Site.Inquiry;
using Window.Web.HttpManager;

namespace Window.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InquiryAPIController : ControllerBase
    {
        #region Ctor

        private readonly IStateService _stateService;

        private readonly IBrandService _brandService;

        private readonly IInquiryService _inquiryService;

        private readonly ISampleService _sampleService;

        private readonly ISellerService _sellerService;

        private readonly IProductService _productService;

        public InquiryAPIController(IStateService stateService, IBrandService brandService, IInquiryService inquiryService, ISampleService sampleService, ISellerService sellerService, IProductService productService)
        {
            _stateService = stateService;
            _brandService = brandService;
            _inquiryService = inquiryService;
            _sampleService = sampleService;
            _sellerService = sellerService;
            _productService = productService;
        }

        #endregion

        #region List Of States

        [HttpGet("get-states")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListOfStates()
        {
            var states = await _stateService.GetListOfState();
            return JsonResponseStatus.Success(states);
        }

        #endregion

        #region List Of City By State Name

        [HttpGet("get-City/{StateName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListOfCity(string StateName)
        {
            var city = await _stateService.GetListOfCityByCityName(StateName);

            return JsonResponseStatus.Success(city);
        }

        #endregion

        #region List Of Brands

        [HttpGet("get-brands")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListOfBrands()
        {
            var brands = await _brandService.GetListOfMainBrand();

            return JsonResponseStatus.Success(brands);
        }

        #endregion

        #region Inquiry API Step 1

        [HttpGet("get-step1/{SellerType}/{MainBrand}/{GlassName}/{UserMacAddress}/{City}/{State}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step1(SellerType SellerType , string MainBrand , string GlassName, string UserMacAddress , string City , string State)
        {
            #region Get Satte and City

            var state = await _stateService.GetLocationByUniqueName(State);

            var city = await _stateService.GetLocationByUniqueName(City);

            #endregion

            #region Get Brand By Name

            var barand = await _brandService.GetMainBrandByBrandName(MainBrand);

            #endregion

            #region Get Glass By Name

            var glass = await _productService.GetGlassWithName(GlassName);

            #endregion

            #region Step 1 Log For User 

            FilterInquiryViewModel log = new FilterInquiryViewModel()
            {
                CountryId = 1,
                StateId = state.Id,
                CityId = city.Id,
                MainBrandId = barand.Id,
                UserMacAddress = UserMacAddress,
                SellerType = SellerType,
                GlassId = glass.Id
            };

            #endregion

            #region Proccess Step 1 Log 

            await _inquiryService.LogInquiryForUserPart1(log);

            #endregion

            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(UserMacAddress);
            if (samples == null) return NotFound();

            #endregion

            return JsonResponseStatus.Success(samples);
        }

        #endregion

        #region Inquiry API Step 2

        [HttpGet("get-step2/{UserMacAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step2(string UserMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(UserMacAddress);
            if (samples == null) return NotFound();

            #endregion

            return JsonResponseStatus.Success(samples);
        }

        #endregion

        #region Inquiry API Step 3

        [HttpGet("get-step3/{sampleId}/{width}/{height}/{KatibeSize}/{userMacAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step3(ulong sampleId, int width, int height , int SampleCount, int? KatibeSize , string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null) return NotFound();

            #endregion

            #region Check Is Exist Sample 

            var sample = await _sampleService.GetSampleBySampleId(sampleId);
            if (sample == null) return NotFound();


            if (sample.MaxHeight < height)
            {
                return JsonResponseStatus.Error("ارتفاع وارد شده بیشتر از حد مجاز است.");
            }
            if (sample.MinHeight > height)
            {
                return JsonResponseStatus.Error("ارتفاع وارد شده کمتر از حد مجاز است.");
            }
            if (sample.MaxWidth < width)
            {
                return JsonResponseStatus.Error("عرض وارد شده بیشتر از حد مجاز است.");
            }
            if (sample.MinWidth > width)
            {
                return JsonResponseStatus.Error("عرض وارد شده کمتر از حد مجاز است.");
            }

            #endregion

            #region Add Log For User

            var res = await _inquiryService.LogInquiryForUserPart2(sampleId, width, height , KatibeSize, userMacAddress , SampleCount);
            if (!res) return NotFound();

            #endregion

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Inquiry API Step 4

        [HttpGet("get-step4/{userMacAddress}/{brandTitle?}/{orderByPrice?}/{orderByScore?}/{pageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step4(string userMacAddress, string? brandTitle, int? orderByPrice, int? orderByScore, int pageId = 1)
        {
            #region Update Inqury By Brand 

            if (!string.IsNullOrEmpty(brandTitle))
            {
                var res = await _inquiryService.UpdateUserInquryInLastStep(userMacAddress, brandTitle);
                if (!res) return NotFound();
            }

            #endregion

            #region Fill Model

            var model = await _inquiryService.ListOfInquiry(userMacAddress);
            if (model == null)
            {
                JsonResponseStatus.Error();
            }

            #endregion

            #region Oredr By Price Value

            if (orderByPrice == 1)
            {
                model = model.OrderByDescending(p => p.Price).ToList();
            }
            else if (orderByPrice == 2)
            {
                model = model.OrderBy(p => p.Price).ToList();
            }

            #endregion

            #region Order By Score

            if (orderByScore == 1)
            {
                model = model.OrderByDescending(p => p.Score).ToList();
            }
            else if (orderByScore == 2)
            {
                model = model.OrderBy(p => p.Score).ToList();
            }

            #endregion

            //#region Paginaition

            //int take = 20;

            //int skip = (pageId - 1) * take;

            //int pageCount = (model.Count() / take);

            //if ((pageCount % 2) == 0 || (pageCount % 2) != 0)
            //{
            //    pageCount += 1;
            //}

            ////var query = model.Skip(skip).Take(take).ToList();

            //var viewModel = Tuple.Create(query, pageCount);

            //#endregion

            return JsonResponseStatus.Success(model);
        }

        #endregion

        #region Show Seller Personal Information

        [HttpGet("get-Seller-Info/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowSellerPersoanlInfo(int userId)
        {
            #region Update Seller Activation Tariff

            await _sellerService.UpdateSellerActivationTariff(Convert.ToUInt64(userId) , false , true);

            #endregion

            #region Log For Visit Seller Profile

            await _sellerService.LogForSellerVisitProfile(Convert.ToUInt64(userId));

            #endregion

            #region Fill Model 

            var model = await _sellerService.FillListOfPersonalInfoForInquiryViewModel(Convert.ToUInt64(userId));
            if (model == null)
            {
                JsonResponseStatus.Error();
            }

            #region Send SMS

            //var res = await _sellerService.SendSMSForSellerForSeenProfile(Convert.ToUInt64(userId));
            //if (res == false)
            //{
            //    JsonResponseStatus.Error();
            //}

            #endregion

            #endregion

            return JsonResponseStatus.Success(model);
        }

        #endregion

        #region Get Last User Last Inquiry

        [HttpGet("add-seller-score/{score}/{sellerId}/{macAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> AddScoreToSeller(int score, int sellerId , string macAddress)
        {
            #region Check Is User Was Scored To Seller

            if (await _inquiryService.checkIsUserScoredToSeller(macAddress, Convert.ToUInt64(sellerId)))
            {
                JsonResponseStatus.Error();
            }

            #endregion

            #region Add Score For Seller

            if (score > 5 || score < 0)
            {
                JsonResponseStatus.Error();
            }

            var res = await _inquiryService.AddScoreForSeller(score, Convert.ToUInt64(sellerId), macAddress);

            #endregion

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Today Date Time API

        [HttpGet("get-dateTime-api")]
        [AllowAnonymous]
        public async Task<IActionResult> DateTimeNowAPI()
        {
            return JsonResponseStatus.Success(DateTime.Now.ToShamsiDate());
        }

        #endregion

        #region List Of Glasses

        [HttpGet("get-galsses")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListOfGlasses()
        {
            var glasses = await _productService.GetListOfGlasses();

            return JsonResponseStatus.Success(glasses);
        }

        #endregion

        #region Count Of Inquiry In Cities

        [HttpGet("get-countOfInquiryInCities/{cityName}")]
        [AllowAnonymous]
        public async Task<IActionResult> CountOfInquiryInCities(string? cityName)
        {
            #region Get Model 

            var model = await _inquiryService.GetCountOfInquiryInCities(cityName);

            #endregion

            return JsonResponseStatus.Success(model);
        }

        #endregion 
        
        #region Count Of Inquiry In Cities

        [HttpGet("get-countOfInquiryInState/{stateName}")]
        [AllowAnonymous]
        public async Task<IActionResult> CountOfInquiryInState(string stateName)
        {
            #region Get Model 

            var model = await _inquiryService.CountOfInquiryInState(stateName);

            #endregion

            return JsonResponseStatus.Success(model);
        }

        #endregion
    }
}
