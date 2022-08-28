using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.State;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageLocations")]
    public class StateController : AdminBaseController
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        #region State List

        public async Task<IActionResult> FilterState(FilterStateViewModel filter)
        {
            var result = await _stateService.FilterState(filter);
            return View(result);
        }

        #endregion

        #region Create State

        [HttpGet]
        public async Task<IActionResult> CreateState(ulong? parentId)
        {
            ViewBag.parentId = parentId;

            if (parentId != null)
            {
                ViewBag.parentState = await _stateService.GetStateById(parentId.Value);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateState(CreateStateViewModel stateViewModel)
        {

            #region Model State Validations

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی باشد";
                if (stateViewModel.ParentId != null)
                {
                    ViewBag.parentState = await _stateService.GetStateById(stateViewModel.ParentId.Value);
                }
                return View(stateViewModel);
            }

            #endregion

            var result = await _stateService.CreateState(stateViewModel);

            switch (result)
            {
                case CreateStateResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                    if (stateViewModel.ParentId.HasValue)
                    {
                        return RedirectToAction("FilterState", new { ParentId = stateViewModel.ParentId.Value });
                    }
                    return RedirectToAction("FilterState");
                case CreateStateResult.UniqNameIsExist:
                    TempData[ErrorMessage] = "نام یکتا تکراری است";
                    break;
                case CreateStateResult.Fail:
                    TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                    break;
            }

            ViewBag.parentId = stateViewModel.ParentId;

            if (stateViewModel.ParentId != null)
            {
                ViewBag.parentState = await _stateService.GetStateById(stateViewModel.ParentId.Value);
            }

            return View(stateViewModel);
        }

        #endregion

        #region Edit State

        public async Task<IActionResult> EditState(ulong id)
        {
            var result = await _stateService.FillEditStateViewModel(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(EditStateViewModel state)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمیباشد";
                return View(state);
            }

            var result = await _stateService.EditState(state);

            switch (result)
            {
                case EditStateResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                    if (state.ParentId.HasValue)
                    {
                        return RedirectToAction("FilterState", new { ParentId = state.ParentId.Value });
                    }
                    return RedirectToAction("FilterState");
                case EditStateResult.UniqNameIsExist:
                    TempData[ErrorMessage] = "نام یکتا تکراری میباشد";
                    break;
                case EditStateResult.Fail:
                    TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                    break;
            }

            return View(state);
        }

        #endregion

        #region Delete State

        public async Task<IActionResult> DeleteBranch(ulong stateId)
        {
            var result = await _stateService.DeleteState(stateId);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
