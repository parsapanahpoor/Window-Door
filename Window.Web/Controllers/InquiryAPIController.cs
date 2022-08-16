﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Window.Application.Services.Interfaces;
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

        public InquiryAPIController(IStateService stateService, IBrandService brandService, IInquiryService inquiryService, ISampleService sampleService)
        {
            _stateService = stateService;
            _brandService = brandService;
            _inquiryService = inquiryService;
            _sampleService = sampleService;
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

        [HttpGet("get-step1/{ProductType}/{ProductKind}/{SellerType}/{MainBrand}/{UserMacAddress}/{City}/{State}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step1(ProductType ProductType , ProductKind ProductKind , SellerType SellerType , string MainBrand , string UserMacAddress , string City , string State)
        {
            #region Get Satte and City

            var state = await _stateService.GetLocationByUniqueName(State);

            var city = await _stateService.GetLocationByUniqueName(City);

            #endregion

            #region Get Brand By Name

            var barand = await _brandService.GetMainBrandByBrandName(MainBrand);

            #endregion

            #region Step 1 Log For User 

            FilterInquiryViewModel log = new FilterInquiryViewModel()
            {
                CountryId = 1,
                StateId = state.Id,
                CityId = city.Id,
                ProductType = ProductType,
                ProductKind = ProductKind,
                MainBrandId = barand.Id,
                UserMacAddress = UserMacAddress,
                SellerType = SellerType
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

        [HttpGet("get-step3/{sampleId}/{width}/{height}/{userMacAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step3(ulong sampleId, int width, int height, string userMacAddress)
        {
            #region Get Samples For Show In Page Model

            var samples = await _sampleService.GetListOfSamplesForShowInAPI(userMacAddress);
            if (samples == null) return NotFound();

            #endregion

            #region Check Is Exist Sample 

            var sample = await _sampleService.GetSampleBySampleId(sampleId);
            if (sample == null) return NotFound();

            #endregion

            #region Add Log For User

            var res = await _inquiryService.LogInquiryForUserPart2(sampleId, width, height, userMacAddress);
            if (!res) return NotFound();

            #endregion

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Inquiry API Step 4

        [HttpGet("get-step4/{userMacAddress}/{pageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Step4(string userMacAddress , int pageId = 1)
        {
            #region Fill Model

            var model = await _inquiryService.ListOfInquiry(userMacAddress);
            if (model == null)
            {
                JsonResponseStatus.Error();
            }

            #endregion

            #region Paginaition

            int take = 20;

            int skip = (pageId - 1) * take;

            int pageCount = (model.Count() / take);

            if ((pageCount % 2) == 0 || (pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = model.Skip(skip).Take(take).ToList();

            var viewModel = Tuple.Create(query, pageCount);

            #endregion

            return JsonResponseStatus.Success(model);
        }

        #endregion
    }
}