using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Web.Areas.Admin.Controllers
{
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

        [HttpPost , ValidateAntiForgeryToken]
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
    }
}
