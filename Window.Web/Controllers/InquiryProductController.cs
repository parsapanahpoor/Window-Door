#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Web.Controllers;

#endregion

public class InquiryProductController : SiteBaseController
{
    #region Ctor

    private readonly IProductService _productService;
    private readonly IStateService _stateService;
    private readonly IBrandService _brandService;
    private readonly ISampleService _sampleService;
    private readonly IInquiryService _inquiryService;
    private readonly ISellerService _sellerService;
    private readonly IMarketService _marketService;
    private readonly IContractService _contractService;
    private readonly IUserService _userService;

    public InquiryProductController(IProductService prodcutService, IStateService stateService, IBrandService brandService
        , ISampleService sampleService, IInquiryService inquiryService, ISellerService sellerService, IMarketService marketService
                , IContractService contractService, IUserService userService)
    {
        _productService = prodcutService;
        _stateService = stateService;
        _brandService = brandService;
        _sampleService = sampleService;
        _inquiryService = inquiryService;
        _sellerService = sellerService;
        _marketService = marketService;
        _contractService = contractService;
        _userService = userService;
    }

    #endregion

    #region Inquiry Step 1

    public async Task<IActionResult> InquiryStep1(FilterInquiryViewModel filter)
    {
        #region Location ViewBags 

        ViewData["States"] = await _stateService.GetStateChildren(1);

        if (filter.StateId != null)
        {
            ViewData["Cities"] = await _stateService.GetStateChildren(filter.StateId.Value);
        }

        #endregion

        #region Glasses ViewBag

        ViewBag.Glasses = await _productService.GetAllGlasses();

        #endregion

        return View();
    }

    #endregion

    #region Test For Step 2

    [HttpGet]
    public async Task<IActionResult> InquiryStep2(ulong StateId, ulong CityId, ProductType? ProductType, ProductKind? ProductKind, SellerType SellerType, ulong? GlassId)
    {
        #region Check That If User Is Seller

        //if (await _sellerService.IsExistAnySellerInfo(User.GetUserId()))
        //{
        //    if (!await _userService.CheckThatIsUserAdminOrNot(User.GetUserId()))
        //    {
        //        TempData[WarningMessage] = "فروشندگان دسترسی به استعلام گیری تدارند.";
        //        return RedirectToAction(nameof(InquiryStep1));
        //    }
        //}

        #endregion

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
            CountryId = 1,
            StateId = StateId,
            CityId = CityId,
            UserMacAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString(),
            SellerType = SellerType,
            GlassId = GlassId
        };

        #endregion

        #region Proccess Step 1 Log 

        await _inquiryService.LogInquiryForUserPart1(log);

        #endregion

        ViewBag.UserMacAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        return View(await _brandService.ShowListOFBrandsByBrandType(SellerType));
    }

    #endregion

    #region Inquiry Step 3

    [HttpGet]
    public async Task<IActionResult> InquiryStep3(ulong? brandId)
    {
        #region Get Samples For Show In Page Model

        var samples = await _sampleService.GetListOfSamplesForShowInAPI(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
        if (samples == null || !samples.Any())
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
            return NotFound();
        }

        #endregion

        #region Update User Log 

        if (brandId.HasValue)
        {
            await _inquiryService.UpdateLogUserInquiryRequest(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString(), brandId.Value);
        }

        #endregion

        ViewBag.UserMacAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        var CountOfVerifySample = await _inquiryService.CheckLogResultUserInquiry(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
        ViewBag.CountOfVerifySample = 5 - CountOfVerifySample;

        return View(samples);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> InquiryStep3(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, string userMacAddress)
    {
        #region Check Is Exist Sample 

        var sample = await _sampleService.GetSampleBySampleIdWithAsNoTracking(sampleId);

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
        if (SampleCount <= 0)
        {
            TempData[ErrorMessage] = "تعداد وارد شده صحیح نمی باشد.";
            return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
        }

        #endregion

        var CountOfVerifySample = await _inquiryService.CheckLogResultUserInquiry(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
        ViewBag.CountOfVerifySample = 5 - CountOfVerifySample;

        //Check User Inquiry Log Count 
        if (CountOfVerifySample == 5)
        {
            TempData[ErrorMessage] = "بیش از تعداد 5 محصول را نمی توان به طور همزمان مقدار دهی کرد.";
            return RedirectToAction(nameof(InquiryStep3));
        }

        #region Add Log For User

        var res = await _inquiryService.InitialResultOfUserInquiry(sampleId, width, height, SampleCount, katibeSize, Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString(), userMacAddress);
        if (!res) return NotFound();

        #endregion

        ViewBag.UserMacAddress = userMacAddress;

        TempData[SuccessMessage] = "محصول مورد نظر به سبد شما اضافه شده است.";
        TempData[InfoMessage] = "شما میتوانید مجدد اندازه ی جدیدی را وارد کنید.";
        return RedirectToAction(nameof(InquiryStep3));
    }

    #endregion

    #region Inquiry Step 4 (proccess inquiry)

    public async Task<IActionResult> InquiryStep4(string userMacAddress, int? orderByPrice)
    {
        userMacAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        #region Fill Model

        var model = await _inquiryService.ListOfInquiry(userMacAddress, Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
        if (model == null || !model.Any())
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        ViewBag.userMacAddress = userMacAddress;
        ViewBag.OredrByPrice = orderByPrice;

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

        TempData[SuccessMessage] = "استعلام گیری با موفقیت انجام شده است .";
        return View(model);
    }

    #endregion

    #region Show Seller Persoanl Inormation 

    [Authorize]
    public async Task<IActionResult> ShowSellerPersoanlInfo(ulong userId)
    {
        string userMacAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        #region Fill Model 

        var model = await _sellerService.FillListOfPersonalInfoViewModel(userId);

        if (model == null) return NotFound();

        ViewData["Countries"] = await _stateService.GetAllCountries();
        ViewData["States"] = await _stateService.GetStateChildren(model.CountryId);
        ViewData["Cities"] = await _stateService.GetStateChildren(model.StateId);

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

        #region Contract Checker

        ViewBag.CanInsertCommentAndStart = await _contractService.CanUserInsertCommentForSeller(User.GetUserId(), userId);
        ViewBag.sellerId = userId;
        ViewBag.ListOfSellerCommentsForShow = await _contractService.ListOfSellerCommentsForShow(userId);
        ViewBag.CurrentUser = await _userService.GetUserById(User.GetUserId());
        ViewBag.LastInquiry = await _inquiryService.ListOfInquiry(userMacAddress, Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());


        #endregion

        return View(model);
    }

    #endregion

    #region Last Inquiry Base On User Log 

    public async Task<IActionResult> LastUserInquryBaseOnUserLog()
    {
        #region Get User Ip Address

        string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        #endregion

        //Get User ID 
        var userId = Ip;

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

        #region Get User Ip Address

        string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

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

        var res = await _inquiryService.AddScoreForSeller(model, Ip);

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
        #region Get User Ip Address

        string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        #endregion

        #region Remove Method

        var res = await _inquiryService.DeleteUserLastestInquiryDetail(Id, Ip);
        if (res == false)
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        return RedirectToAction(nameof(GetUserLastestInquiry), new { userMacAddress = Ip });
    }

    #endregion

    #region Get Lastest User Iquiry 

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserLastestInquiry(string userMacAddress)
    {
        #region Get User Ip Address

        string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        #endregion

        #region Get List Of Lastest Inquiry

        var res = await _inquiryService.GetUserLastestInquiryDetailForChange(Ip);
        if (res == null)
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        ViewBag.UserMacAddress = Ip;

        return View(res);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> GetUserLastestInquiry(ulong inquiryDetailId, ulong sampleId, int width, int height, int? katibeSize, string userMacAddress, int? SampleCount)
    {
        #region Get User Ip Address

        string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

        #endregion

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
        if (SampleCount <= 0)
        {
            TempData[ErrorMessage] = "تعداد وارد شده صحیح نمی باشد.";
            return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = userMacAddress });
        }

        #endregion

        #region Add Log For User

        var res = await _inquiryService.UpdateUserInquiryItrm(inquiryDetailId, sampleId, width, height, katibeSize, userMacAddress, SampleCount);
        if (!res)
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        ViewBag.UserMacAddress = Ip;

        return RedirectToAction(nameof(GetUserLastestInquiry));
    }

    #endregion

    #region Send Contract Request For Seller 

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> RequestForContract(ulong sellerId)
    {
        #region Create Contract Request 

        var res = await _contractService.CreateContractRequestFromUser(User.GetUserId(), sellerId);

        if (res)
        {
            TempData[SuccessMessage] = "درخواست قرارداد باموفقیت ارسال شده است.";
            return RedirectToAction(nameof(ShowSellerPersoanlInfo), new { userId = sellerId });
        }

        #endregion

        TempData[ErrorMessage] = "عملیات باخطا مواجه شده است.";
        return RedirectToAction(nameof(ShowSellerPersoanlInfo), new { userId = sellerId });
    }

    #endregion

    #region Add Comment For User 

    public async Task<IActionResult> AddCommentFromUser(AddCommentSiteSideViewModel comment)
    {
        #region Model State Validation 

        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return RedirectToAction(nameof(ShowSellerPersoanlInfo), new { userId = comment.SellerId });
        }

        #endregion

        #region Add Comment Method 

        var res = await _contractService.AddCommentFromUser(comment, User.GetUserId());

        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ShowSellerPersoanlInfo), new { userId = comment.SellerId });
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction(nameof(ShowSellerPersoanlInfo), new { userId = comment.SellerId });
    }

    #endregion
}
