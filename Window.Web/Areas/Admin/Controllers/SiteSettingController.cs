using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditSiteSetting1;
using Window.Application.CQRS.AdminPanel.SiteSetting.Query.SiteSetting1;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;
using Window.Web.HttpManager;
namespace Window.Web.Areas.Admin.Controllers;

public class SiteSettingController : AdminBaseController
{
    #region Ctor

    private readonly ISiteSettingService _siteSettingService;

    public SiteSettingController(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    #endregion

    #region Add Or Edit Site Setting

    [HttpGet]
    public async Task<IActionResult> AddOrEditSiteSetting()
    {
        return View(await _siteSettingService.FillSiteSettingModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEditSiteSetting(SiteSetting siteSetting)
    {
        #region Model State Validations

        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        #endregion

        #region Add Or Update Site Setting 

        var res = await _siteSettingService.AddOrUpdateSiteSetting(siteSetting);

        if (res)
        {
            return RedirectToAction(nameof(AddOrEditSiteSetting));
        }

        #endregion

        return View();
    }

    #endregion

    #region Admin Mobiles

    #region List Of Mobiles

    [HttpGet]
    public async Task<IActionResult> ListOfMobiles()
    {
        return View(await _siteSettingService.ListOfAdminMobiles());
    }

    #endregion

    #region Create Mobiles 

    [HttpGet]
    public async Task<IActionResult> CreateAdminMobile()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdminMobile(string mobile)
    {
        if (ModelState.IsValid)
        {
            await _siteSettingService.Add_AdminMobiles(new AdminMobiles()
            {
                AdminMobile = mobile.Trim().ToLower().SanitizeText()
            });

            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ListOfMobiles));
        }

        TempData[ErrorMessage] = "";
        return View();
    }

    #endregion

    #region Delete Admin Mobile

    public async Task<IActionResult> DeleteAdminMobile(ulong mobileId,
                                                       CancellationToken cancellation)
    {
        var result = await _siteSettingService.Delete_AdminMobile(mobileId, cancellation);
        if (result) return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion

    #endregion

    #region Shop Scales

    #region List Of Scales

    [HttpGet]
    public async Task<IActionResult> ListOfScales()
    {
        return View(await _siteSettingService.ListOfSalesScales());
    }

    #endregion

    #region Create Scales 

    [HttpGet]
    public async Task<IActionResult> CreateScales()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateScales(string scaleName)
    {
        if (ModelState.IsValid)
        {
            await _siteSettingService.Add_SaleScale(new SalesScale()
            {
                ScaleTitle = scaleName.Trim().ToLower().SanitizeText()
            });

            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ListOfScales));
        }

        TempData[ErrorMessage] = "";
        return View();
    }

    #endregion

    #region Delete Scales

    public async Task<IActionResult> DeleteScales(ulong scaleId,
                                                       CancellationToken cancellation)
    {
        var result = await _siteSettingService.Delete_SaleScaleId(scaleId, cancellation);
        if (result) return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion

    #endregion

    #region Landing page Settings 

    #region Manage Page 

    [HttpGet]
    public async Task<IActionResult> ManagePage() => View();

    #endregion

    #region SiteSetting 1

    [HttpGet]
    public async Task<IActionResult> SiteSetting1(CancellationToken cancellationToken = default)
    => View(await Mediator.Send(new SiteSetting1Query()));

    [HttpPost]
    public async Task<IActionResult> SiteSetting1(AddOrEditSiteSetting1Query request , 
                                                  CancellationToken cancellation = default)
    {
        if (ModelState.IsValid)
        {
            var res = await Mediator.Send(request, cancellation);
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(ManagePage));
            }
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(request);
    }

    #endregion

    #endregion
}
