using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditSiteSetting1;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.CreateColorFullSetting;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.CreateFreeConsultant;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.DeleteFreeConsultant;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditColorFullSetting;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditFreeConsultant;
using Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetColorFull;
using Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetFreeConsultant;
using Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfColorFullSiteSettingQuery;
using Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfFreeConsultantSiteSettingQuery;
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

    #region Color Full Site Setting

    #region List Of ColorFull SiteSetting

    [HttpGet]
    public async Task<IActionResult> ListOfColorFullSiteSetting(CancellationToken cancellationToken = default)
    {
        return View(await Mediator.Send(new ListOfColorFullSiteSettingQuery()));
    }

    #endregion

    #region Create Color Full Setting

    [HttpGet]
    public IActionResult CreateColorFullSetting() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateColorFullSetting(CreateColorFullSettingCommand command , 
                                                            CancellationToken cancellation = default)
    {
        #region Create ColorFull Site Setting

        var res = await Mediator.Send(command);
        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ManagePage));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(command);
    }

    #endregion

    #region Edit Color Full Setting

    [HttpGet]
    public async Task<IActionResult> EditColorFullSetting(ulong colorId,
                                                          CancellationToken cancellation = default)
    {
        return View(await Mediator.Send(new GetColorFullSettingQuery()
        {
            ColorFullSettingId = colorId,
        }));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditColorFullSetting(ColorFullSiteSetting command,
                                                          CancellationToken cancellation = default)
    {
        #region Edit Color Full 

        var res = await Mediator.Send(new EditColorFullSiteSettingCommand()
        {
            ColorFullSiteSetting = command,
        });
        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ManagePage));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(command);
    }


    #endregion

    #region Delete ColorFull SiteSetting

    public async Task<IActionResult> DeleteColorFullSiteSetting(ulong colorId,
                                                                CancellationToken cancellation)
    {
        var result = await Mediator.Send(new DeleteColorFullSettingCommand()
        {
            ColorFullId = colorId
        });
        if (result) return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion

    #endregion

    #region Free Consultant

    #region List Of Free Consultant

    [HttpGet]
    public async Task<IActionResult> ListOfFreeConsultant(CancellationToken cancellationToken = default)
    {
        return View(await Mediator.Send(new ListOfFreeConsultantSiteSettingQuery()));
    }

    #endregion

    #region Create Free Consultant

    [HttpGet]
    public IActionResult CreateFreeConsultant() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFreeConsultant(CreateFreeConsultantCommand command,
                                                          CancellationToken cancellation = default)
    {
        #region Create Free Consultant

        var res = await Mediator.Send(command);
        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ManagePage));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(command);
    }

    #endregion

    #region Edit Free Consultant

    [HttpGet]
    public async Task<IActionResult> EditFreeConsultant(ulong consultantId,
                                                        CancellationToken cancellation = default)
    {
        return View(await Mediator.Send(new FreeConsultantSettingQuery()
        {
            FreeConsultantSettingId = consultantId,
        }));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFreeConsultant(FreeConsultant command,
                                                        CancellationToken cancellation = default)
    {
        #region Edit Color Full 

        var res = await Mediator.Send(new EditFreeConsultantCommand()
        {
            FreeConsultant = command,
        });
        if (res)
        {
            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ManagePage));
        }

        #endregion

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(command);
    }


    #endregion

    #region Delete Free Consultant

    public async Task<IActionResult> DeleteFreeConsultant(ulong consultantId,
                                                          CancellationToken cancellation)
    {
        var result = await Mediator.Send(new DeleteFreeConsultantCommand()
        {
            FreeConsultantId = consultantId
        });
        if (result) return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion

    #endregion

    #endregion
}
