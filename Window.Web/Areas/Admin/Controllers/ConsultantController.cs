using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Counseling;
using Window.Domain.ViewModels.Admin.Cosultant;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers
{
    public class ConsultantController : AdminBaseController
    {
        #region Ctor

        private readonly IConsulantService _consultantService;

        public ConsultantController(IConsulantService consultantService)
        {
            _consultantService = consultantService;
        }

        #endregion

        #region Filter Consultant 

        [HttpGet]
        public async Task<IActionResult> Index(FilterConsultantAdminSideViewModel filter)
        {
            var result = await _consultantService.FilterConsultantAdminSideViewModel(filter);

            return View(result);
        }

        #endregion

        #region Create Consultant

        [HttpGet]
        public async Task<IActionResult> CreateConsultant()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConsultant(Counseling model)
        {
            #region Model State Validations

            if (!ModelState.IsValid) return View(model);

            #endregion

            await _consultantService.CreateConsultant(model);

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Technical Issues

        [HttpGet]
        public async Task<IActionResult> EditConsultant(ulong id)
        {
            #region Fill Model

            var Consultant = await _consultantService.GetConsultantById(id);
            if (Consultant == null) return NotFound();

            #endregion

            return View(Consultant);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConsultant(Counseling texhnical)
        {
            #region Model State Validation 

            if (!ModelState.IsValid)
            {
                return View(texhnical);
            }

            #endregion

            await _consultantService.UpdateConsultant(texhnical);

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Technical Issues

        public async Task<IActionResult> DeleteConsultant(ulong id)
        {
            var result = await _consultantService.DeleteConsultant(id);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
