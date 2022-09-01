﻿using Window.Application.Interfaces;
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

        private readonly IAdminDashboardService _adminDashboardService;

        public HomeController(IConfiguration configuration, IUserService userService, IStateService stateService, IAdminDashboardService adminDashboardService)
        {
            _configuration = configuration;
            _userService = userService;
            _stateService = stateService;
            _adminDashboardService = adminDashboardService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index()
        {
            //Count Of Active User 
            ViewBag.CountOfActiveUser = await _adminDashboardService.CountOfActiveUsers();

            //Count Of Active Markets
            ViewBag.CountOfActiveMarkets = await _adminDashboardService.CountOfActiveMarkets();

            //Count Of Dis Active Markets 
            ViewBag.DisActiveMarkets = await _adminDashboardService.CountOfDisActiveMarkets();

            //Count Of Today Register Users
            ViewBag.CountOfTodayRegisterUsers = await _adminDashboardService.CountOFTodayRegisterUsers();

            //Get List Of MArket That DisActive Today 
            ViewBag.ListOfMArketsThatDisActiveToday = await _adminDashboardService.GetListOfMarketsThatDisActiveToday();

            ViewBag.ListOfMArketsThatDisActiveIn3Day = await _adminDashboardService.GetListOfMarketsThatDisActiveIn3Day();

            ViewBag.ListOfMArketsThatDisActiveIn15Day = await _adminDashboardService.GetListOfMarketsThatDisActiveIn15Day();

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
