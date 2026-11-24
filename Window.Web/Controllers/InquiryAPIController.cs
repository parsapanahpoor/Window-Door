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

        private readonly IProductService _productService;

        public InquiryAPIController(IStateService stateService, IBrandService brandService, IInquiryService inquiryService, IProductService productService)
        {
            _stateService = stateService;
            _brandService = brandService;
            _inquiryService = inquiryService;
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
        [HttpGet("get-step2/{UserMacAddress}")]
        [HttpGet("get-step3/{sampleId}/{width}/{height}/{KatibeSize}/{userMacAddress}")]
        [HttpGet("get-step4/{userMacAddress}/{brandTitle?}/{orderByPrice?}/{orderByScore?}/{pageId}")]
        [HttpGet("get-Seller-Info/{userId}")]
        [HttpGet("add-seller-score/{score}/{sellerId}/{macAddress}")]
        [AllowAnonymous]
        public IActionResult RetiredInquiry() => StatusCode(StatusCodes.Status410Gone, new
        {
            message = "مسیر قدیمی استعلام بازنشسته شده است. استعلام را در وب‌سایت انجام دهید.",
            url = "/InquiryProduct/InquiryStep1"
        });

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
