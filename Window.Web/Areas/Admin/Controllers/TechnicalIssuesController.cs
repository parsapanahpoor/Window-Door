using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.Entities.TechnicalIssues;
using Window.Domain.ViewModels.Admin.TechnicalIssues;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers
{
    public class TechnicalIssuesController : AdminBaseController
    {
        #region Ctor

        private readonly ITechnicalIssues _technicalIssues;

        public TechnicalIssuesController(ITechnicalIssues technicalIssues)
        {
            _technicalIssues = technicalIssues;
        }

        #endregion

        #region Filter Technical Issues 

        [HttpGet]
        public async Task<IActionResult> Index(FilterTechnicalIssuesAdminSideViewModel filter)
        {
            var result = await _technicalIssues.FilterTechnicalIssuesAdminSideViewModel(filter);

            return View(result);
        }

        #endregion

        #region Create TechnicalIssues

        [HttpGet]
        public async Task<IActionResult> CreateTechnicalIssues()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTechnicalIssues(TechnicalIssues model)
        {
            #region Model State Validations

            if (!ModelState.IsValid) return View(model);

            #endregion

            await _technicalIssues.CreateTechnicalIssues(model);

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Technical Issues

        [HttpGet]
        public async Task<IActionResult> EditTechnicalIssues(ulong id)
        {
            #region Fill Model

            var technicalIssues = await _technicalIssues.GetTechnicalIssuesById(id);
            if (technicalIssues == null) return NotFound();

            #endregion

            return View(technicalIssues);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTechnicalIssues(TechnicalIssues texhnical)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                return View(texhnical);
            }

            #endregion

            await _technicalIssues.UpdateTechnicalIssues(texhnical);

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Technical Issues

        public async Task<IActionResult> DeleteTechnicalIssues(ulong id)
        {
            var result = await _technicalIssues.DeleteTechnicalIssues(id);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
