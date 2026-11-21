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

    #region Inquiry ownership

    private const string InquiryTokenSessionKey = "InquiryToken";

    private bool IsSignedIn => User.Identity?.IsAuthenticated == true;

    /// <summary>
    /// Resolves the inquiry token for the current request. Guests may only use the
    /// token stored in their own session; signed-in users must own the inquiry.
    /// A guest inquiry is claimed by the user on the first signed-in request.
    /// </summary>
    private async Task<string?> ResolveInquiryTokenAsync(string? requestToken)
    {
        var sessionToken = HttpContext.Session.GetString(InquiryTokenSessionKey);

        if (!IsSignedIn)
        {
            if (!string.IsNullOrWhiteSpace(requestToken))
            {
                return requestToken == sessionToken ? requestToken : null;
            }

            return sessionToken;
        }

        var token = string.IsNullOrWhiteSpace(requestToken) ? sessionToken : requestToken;

        if (string.IsNullOrWhiteSpace(token))
        {
            token = await _inquiryService.GetLatestInquiryTokenForUser(User.GetUserId());
            if (!string.IsNullOrWhiteSpace(token))
            {
                HttpContext.Session.SetString(InquiryTokenSessionKey, token);
            }

            return token;
        }

        if (await _inquiryService.ClaimInquiry(token, User.GetUserId()))
        {
            HttpContext.Session.SetString(InquiryTokenSessionKey, token);
            return token;
        }

        if (await _inquiryService.InquiryBelongsToUser(token, User.GetUserId()))
        {
            HttpContext.Session.SetString(InquiryTokenSessionKey, token);
            return token;
        }

        return null;
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

        // The previous implementation keyed ownership by the server's IP address, so every
        // visitor shared one basket. Each inquiry now gets a server-generated token; the
        // token is bound to the visitor's session (and to the user id after sign-in).
        var inquiryToken = Guid.NewGuid().ToString("N");

        FilterInquiryViewModel log = new FilterInquiryViewModel()
        {
            CountryId = 1,
            StateId = StateId,
            CityId = CityId,
            UserMacAddress = inquiryToken,
            SellerType = SellerType,
            GlassId = GlassId
        };

        #endregion

        #region Proccess Step 1 Log

        await _inquiryService.LogInquiryForUserPart1(log, IsSignedIn ? User.GetUserId() : null);
        HttpContext.Session.SetString(InquiryTokenSessionKey, inquiryToken);

        #endregion

        ViewBag.UserMacAddress = inquiryToken;

        return View(await _brandService.ShowListOFBrandsByBrandType(SellerType));
    }

    #endregion

    #region Inquiry Step 3

    [HttpGet]
    public async Task<IActionResult> InquiryStep3(ulong? brandId, string? userMacAddress)
    {
        var inquiryToken = await ResolveInquiryTokenAsync(userMacAddress);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

        #region Get Samples For Show In Page Model

        var samples = await _sampleService.GetListOfSamplesForShowInAPI(inquiryToken);
        if (samples == null || !samples.Any())
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشند .";
            return NotFound();
        }

        #endregion

        #region Update User Log

        if (brandId.HasValue)
        {
            await _inquiryService.UpdateLogUserInquiryRequest(inquiryToken, brandId.Value);
        }

        #endregion

        ViewBag.UserMacAddress = inquiryToken;

        var CountOfVerifySample = await _inquiryService.CheckLogResultUserInquiry(inquiryToken);
        ViewBag.CountOfVerifySample = 5 - CountOfVerifySample;

        return View(samples);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> InquiryStep3(ulong sampleId, int width, int height, int SampleCount, int? katibeSize, string userMacAddress)
    {
        var inquiryToken = await ResolveInquiryTokenAsync(userMacAddress);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

        #region Check Is Exist Sample

        var sample = await _sampleService.GetSampleBySampleIdWithAsNoTracking(sampleId);
        if (sample == null)
        {
            TempData[ErrorMessage] = "نمونه انتخاب شده معتبر نیست.";
            return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = inquiryToken });
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

        var CountOfVerifySample = await _inquiryService.CheckLogResultUserInquiry(inquiryToken);
        ViewBag.CountOfVerifySample = 5 - CountOfVerifySample;

        //Check User Inquiry Log Count
        if (CountOfVerifySample >= 5)
        {
            TempData[ErrorMessage] = "بیش از تعداد 5 محصول را نمی توان به طور همزمان مقدار دهی کرد.";
            return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = inquiryToken });
        }

        #region Add Log For User

        var res = await _inquiryService.InitialResultOfUserInquiry(sampleId, width, height, SampleCount, katibeSize, inquiryToken, inquiryToken);
        if (!res) return NotFound();

        #endregion

        ViewBag.UserMacAddress = inquiryToken;

        TempData[SuccessMessage] = "محصول مورد نظر به سبد شما اضافه شده است.";
        TempData[InfoMessage] = "شما میتوانید مجدد اندازه ی جدیدی را وارد کنید.";
        return RedirectToAction(nameof(InquiryStep3), new { userMacAddress = inquiryToken });
    }

    #endregion

    #region Inquiry Step 4 (proccess inquiry)

    public async Task<IActionResult> InquiryStep4(string? userMacAddress, int? orderByPrice)
    {
        var inquiryToken = await ResolveInquiryTokenAsync(userMacAddress);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #region Fill Model

        var model = await _inquiryService.ListOfInquiry(inquiryToken, inquiryToken);
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
        var inquiryToken = await ResolveInquiryTokenAsync(null);

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
        ViewBag.LastInquiry = string.IsNullOrWhiteSpace(inquiryToken)
            ? null
            : await _inquiryService.ListOfInquiry(inquiryToken, inquiryToken);


        #endregion

        return View(model);
    }

    #endregion

    #region Last Inquiry Base On User Log 

    public async Task<IActionResult> LastUserInquryBaseOnUserLog()
    {
        var inquiryToken = await ResolveInquiryTokenAsync(null);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

        return RedirectToAction(nameof(InquiryStep4), new { userMacAddress = inquiryToken });
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

        var inquiryToken = await ResolveInquiryTokenAsync(null);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

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

        var res = await _inquiryService.AddScoreForSeller(model, inquiryToken);

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

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserLasteInquiryDetail(ulong Id, string? userMacAddress)
    {
        var inquiryToken = await ResolveInquiryTokenAsync(userMacAddress);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

        #region Remove Method

        var res = await _inquiryService.DeleteUserLastestInquiryDetail(Id, inquiryToken);
        if (res == false)
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        return RedirectToAction(nameof(GetUserLastestInquiry), new { userMacAddress = inquiryToken });
    }

    #endregion

    #region Get Lastest User Iquiry 

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserLastestInquiry(string? userMacAddress)
    {
        var inquiryToken = await ResolveInquiryTokenAsync(userMacAddress);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

        #region Get List Of Lastest Inquiry

        var res = await _inquiryService.GetUserLastestInquiryDetailForChange(inquiryToken);
        if (res == null)
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        ViewBag.UserMacAddress = inquiryToken;

        return View(res);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> GetUserLastestInquiry(ulong inquiryDetailId, ulong sampleId, int width, int height, int? katibeSize, string userMacAddress, int? SampleCount)
    {
        var inquiryToken = await ResolveInquiryTokenAsync(userMacAddress);
        if (string.IsNullOrWhiteSpace(inquiryToken))
        {
            TempData[ErrorMessage] = "استعلام یافت نشد. لطفا استعلام را از ابتدا شروع کنید.";
            return RedirectToAction(nameof(InquiryStep1));
        }

        #region Get Samples For Show In Page Model

        var samples = await _sampleService.GetListOfSamplesForShowInAPI(inquiryToken);
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

        var res = await _inquiryService.UpdateUserInquiryItrm(inquiryDetailId, sampleId, width, height, katibeSize, inquiryToken, SampleCount);
        if (!res)
        {
            TempData[ErrorMessage] = "نتیجه ای برای استعلام شما یافت نشده است .";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        ViewBag.UserMacAddress = inquiryToken;

        return RedirectToAction(nameof(GetUserLastestInquiry), new { userMacAddress = inquiryToken });
    }

    #endregion

    #region Send Contract Request For Seller 

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
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

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
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
