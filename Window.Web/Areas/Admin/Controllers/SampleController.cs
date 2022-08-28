using Microsoft.AspNetCore.Mvc;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Sample;
using Window.Domain.ViewModels.Admin.Sample;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageSamples")]
    public class SampleController : AdminBaseController
    {
        #region ctor

        private readonly ISampleService _sampleService;

        private readonly ISegmentService _segmentService;

        public SampleController(ISampleService sampleService, ISegmentService segmentService)
        {
            _sampleService = sampleService;
            _segmentService = segmentService;
        }

        #endregion

        #region Filter Sample

        public async Task<IActionResult> FilterSample(FilterSampleAdminSideViewModel filter)
        {
            return View(await _sampleService.FilterSampleAdminSideViewModel(filter));
        }

        #endregion

        #region Create Sample 

        [HttpGet]
        public async Task<IActionResult> CreateSample()
        {
            ViewData["Segments"] = await _segmentService.GetAllSegments();

            return View();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSample(Sample sample , List<ulong> segmentsId , IFormFile ArticleImage)
        {
            #region Model State Validation 

            if (ArticleImage == null)
            {
                TempData[ErrorMessage] = "درج نمونه با شکست روبروشده است .";
                return RedirectToAction(nameof(CreateSample));
            }

            #endregion

            #region Create Sample

            var res = await _sampleService.CreateSampleFromAdmin(sample , segmentsId , ArticleImage);

            if (res)
            {
                TempData[SuccessMessage] = "درج نمونه با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterSample));
            }

            #endregion

            ViewData["Segments"] = await _segmentService.GetAllSegments();

            TempData[ErrorMessage] = "درج نمونه با شکست روبروشده است .";
            return RedirectToAction(nameof(CreateSample));
        }

        #endregion

        #region Edi Sample

        [HttpGet]
        public async Task<IActionResult> EditSample(ulong id)
        {
            #region Fill View Model

            var model = await _sampleService.GetSampleBySampleId(id);
            if (model == null) return NotFound();

            ViewData["Segments"] = await _segmentService.GetAllSegments();
            ViewBag.SelectedSegments = await _sampleService.GetSampleSelectedSegmnetIdBySampleId(id);

            #endregion

            return View(model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSample(Sample sample, List<ulong> segmentsId, IFormFile? ArticleImage)
        {
            #region Update Sample

            var res = await _sampleService.EditSample(sample , segmentsId , ArticleImage);

            if (res)
            {
                TempData[SuccessMessage] = "درج نمونه با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterSample));
            }

            #endregion

            ViewData["Segments"] = await _segmentService.GetAllSegments();
            ViewBag.SelectedSegments = await _sampleService.GetSampleSelectedSegmnetIdBySampleId(sample.Id);

            TempData[ErrorMessage] = "درج نمونه با شکست روبروشده است .";
            return RedirectToAction(nameof(sample));
        }

        #endregion

        #region Delete Sample

        public async Task<IActionResult> DeleteSample(ulong Id)
        {
            var result = await _sampleService.DeleteSample(Id);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
