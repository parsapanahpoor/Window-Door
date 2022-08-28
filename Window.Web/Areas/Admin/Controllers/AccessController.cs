using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StaticTools;
using Window.Domain.ViewModels.Access;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageAccess")]
    public class AccessController : AdminBaseController
    {
        #region Ctor

        private readonly IPermissionService _permissionService;

        public AccessController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        #endregion

        #region Role

        #region Create Role

        public async Task<IActionResult> CreateRole(ulong? parentId)
        {
            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
            ViewBag.parentId = parentId;

            if (parentId != null)
            {
                ViewBag.parentRole = await _permissionService.GetRoleById(parentId.Value);
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel create)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return View(create);
            }

            if (create.Permissions == null || !create.Permissions.Any())
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "انتخاب یکی از دسترسی ها الزامی است .";
                return View(create);
            }

            var result = await _permissionService.CreateRole(create);

            if (result)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction("FilterRoles", "Access", new { area = "Admin" });
            }

            TempData[WarningMessage] = "عنوان یکتا موجود است.";
            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View(create);
        }

        #endregion

        #region Filter Roles

        public async Task<IActionResult> FilterRoles(FilterRolesViewModel filter)
        {
            var result = await _permissionService.FilterRoles(filter);

            return View(result);
        }

        #endregion

        #region Edit Role

        public async Task<IActionResult> EditRole(ulong id)
        {
            var result = await _permissionService.FillEditRoleViewModel(id);

            if (result == null)
            {
                return NotFound();
            }

            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
                return View(edit);
            }

            if (edit.Permissions == null || !edit.Permissions.Any())
            {
                ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();
                TempData[ErrorMessage] = "انتخاب یکی از دسترسی ها الزامی است .";
                return View(edit);
            }

            var result = await _permissionService.EditRole(edit);

            switch (result)
            {
                case EditRoleResult.Success:
                    TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                    return RedirectToAction("FilterRoles", "Access", new { area = "Admin" });
                case EditRoleResult.RoleNotFound:
                    TempData[ErrorMessage] = "نقش مورد نظر یافت نشده است.";
                    return RedirectToAction("FilterRoles", "Access", new { area = "Admin" });
                case EditRoleResult.UniqueNameExists:
                    TempData[WarningMessage] = "عنوان یکتا موجود است.";
                    break;
            }

            ViewData["Permissions"] = PermissionsList.Permissions.Where(s => !s.IsDelete).ToList();

            return View(edit);
        }

        #endregion

        #region Delete Role

        public async Task<IActionResult> DeleteRole(ulong roleId)
        {
            var result = await _permissionService.DeleteRole(roleId);

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
