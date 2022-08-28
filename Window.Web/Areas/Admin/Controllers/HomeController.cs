using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Domain.ViewModels.User;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("Dashboard")]
    public class HomeController : AdminBaseController
    {
        #region constructor

        private readonly IConfiguration _configuration;

        public IUserService _userService;

        private readonly IStateService _stateService;

        public HomeController(IConfiguration configuration, IUserService userService, IStateService stateService)
        {
            _configuration = configuration;
            _userService = userService;
            _stateService = stateService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index()
        {
            return View();
        }

        #endregion

        #region SearchUserModal

        public async Task<IActionResult> SearchUserModal(FilterUserViewModel filter, string baseName)
        {
            filter.TakeEntity = 5;

            var result = await _userService.FilterUsers(filter);

            ViewBag.BaseName = baseName;

            return PartialView("_FilterUsersModalPartial", result);
        }

        #endregion

        #region Load Cities

        public async Task<IActionResult> LoadCities(ulong stateId)
        {
            var result = await _stateService.GetStateChildren(stateId);

            return JsonResponseStatus.Success(result);
        }

        #endregion
    }
}
