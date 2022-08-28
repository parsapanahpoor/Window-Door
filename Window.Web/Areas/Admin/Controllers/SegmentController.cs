using Microsoft.AspNetCore.Mvc;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Segment;
using Window.Domain.ViewModels.Admin.Segment;
using Window.Web.HttpManager;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageSegments")]
    public class SegmentController : AdminBaseController
    {
        #region Ctor

        private readonly ISegmentService _segmentService;

        public SegmentController(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        #endregion

        #region Segment

        #region Filter Segment 

        public async Task<IActionResult> FilterSegment(FilterSegmentViewModel filter)
        {
            return View(await _segmentService.FilterSegmentViewModel(filter));
        }

        #endregion

        #region Create Segment 

        [HttpGet]
        public async Task<IActionResult> CreateSegment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSegment(Segment segment)
        {

            #region Create Brand Method

            var res = await _segmentService.CreateSegment(segment);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterSegment));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";

            return View(segment);
        }

        #endregion

        #region Edit Segment 

        [HttpGet]
        public async Task<IActionResult> EditSegment(ulong segmentId)
        {
            #region Get segment By Id 

            var brand = await _segmentService.GetSegmentById(segmentId);

            if (brand == null) return NotFound();

            #endregion

            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> EditSegment(Segment segment)
        {

            #region Update Method 

            var res = await _segmentService.UpdateSegment(segment);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterSegment));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";

            return View(segment);
        }

        #endregion

        #region Delete Segment 

        public async Task<IActionResult> DeleteSegment(ulong Id)
        {
            var result = await _segmentService.DeleteSegment(Id);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }
        #endregion

        #endregion

        #region Glass

        #region List Of Glasses

        public async Task<IActionResult> FilterGlasses(FilterGlassesViewModel filter)
        {
            return View(await _segmentService.FilterGlasseViewModel(filter));
        }

        #endregion

        #region Create Segment 

        [HttpGet]
        public async Task<IActionResult> CreateGlass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGlass(Glass glass)
        {

            #region Create Glass Method

            var res = await _segmentService.CreateGlass(glass);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterGlasses));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";

            return View(glass);
        }

        #endregion

        #region Edit Segment 

        [HttpGet]
        public async Task<IActionResult> EditGlass(ulong glassId)
        {
            #region Get Glass By Id 

            var glass = await _segmentService.GetGlassById(glassId);

            if (glass == null) return NotFound();

            #endregion

            return View(glass);
        }

        [HttpPost]
        public async Task<IActionResult> EditGlass(Glass glass)
        {

            #region Update Method 

            var res = await _segmentService.UpdateGlass(glass);

            if (res)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شده است .";
                return RedirectToAction(nameof(FilterGlasses));
            }

            #endregion

            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد";

            return View(glass);
        }

        #endregion

        #region Delete Segment 

        public async Task<IActionResult> DeleteGlass(ulong Id)
        {
            var result = await _segmentService.DeleteGlass(Id);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }
        #endregion

        #endregion
    }
}
