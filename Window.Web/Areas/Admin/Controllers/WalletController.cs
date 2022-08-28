﻿using Window.Application.Interfaces;
using Window.Domain.ViewModels.Admin.Wallet;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Window.Application.Security;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ListOfTransactions")]
    public class WalletController : AdminBaseController
    {
        #region Ctor

        private readonly IWalletService _walletService;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<WalletController> _localizer;
        private readonly IStringLocalizer<SharedLocalizer.SharedLocalizer> _sharedLocalizer;

        public WalletController(IWalletService walletService, IUserService userService, IStringLocalizer<WalletController> localizer, IStringLocalizer<SharedLocalizer.SharedLocalizer> sharedLocalizer)
        {
            _walletService = walletService;
            _userService = userService;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        #endregion

        #region List Of Wallets

        public async Task<IActionResult> Index(FilterWalletViewModel model)
        {
            var filter = await _walletService.FilterWalletsAsync(model);
            return View(filter);
        }

        #endregion

        #region Create Wallet

        public async Task<IActionResult> CreateWallet(ulong id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                TempData[ErrorMessage] = _sharedLocalizer["User not found"].Value;
                return RedirectToAction("Index", "User");
            }

            var model = new AdminCreateWalletViewModel
            {
                UserId = id,
                User = user
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(ulong id, AdminCreateWalletViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.User = await _userService.GetUserById(id);
                return View(model);
            }

            var response = await _walletService.CreateWalletAsync(model);

            switch (response)
            {
                case AdminCreateWalletResponse.Success:
                    TempData[SuccessMessage] = _sharedLocalizer["Operation Successfully"].Value;
                    return RedirectToAction("Profile", "User",new { id = id });
                    break;

                case AdminCreateWalletResponse.UserNotFound:
                    TempData[ErrorMessage] = _sharedLocalizer["User not found"].Value;
                    return RedirectToAction("Index", "User");

                default:
                    TempData[ErrorMessage] = _localizer["An error occurred in the create wallet process"].Value;
                    break;
            }

            model.User = await _userService.GetUserById(id);
            return View(model);
        }

        #endregion

        #region Edit Wallet

        public async Task<IActionResult> EditWallet(ulong id)
        {
            var editableWallet = await _walletService.GetWalletForEditAsync(id);

            if (editableWallet == null)
            {
                TempData[ErrorMessage] = _localizer["Wallet not found"].Value;
                return RedirectToAction("Index");
            }


            return View(editableWallet);
        }

        [HttpPost]
        public async Task<IActionResult> EditWallet(ulong id, AdminEditWalletViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.User = await _userService.GetUserById(id);
                return View(model);
            }

            var response = await _walletService.EditWalletAsync(model);

            switch (response)
            {
                case AdminEditWalletResponse.Success:
                    TempData[SuccessMessage] = _sharedLocalizer["Operation Successfully"].Value;
                    return RedirectToAction("Index");

                case AdminEditWalletResponse.WalletNotFound:
                    TempData[ErrorMessage] = _localizer["Wallet Not Found"].Value;
                    return RedirectToAction("Index");
                    break;

                default:
                    TempData[ErrorMessage] = _localizer["An error occurred in the edit wallet process"].Value;
                    break;
            }

            model.User = await _userService.GetUserById(id);
            return View(model);
        }

        #endregion

        #region Remove Wallet

        public async Task<IActionResult> RemoveWallet(ulong id)
        {
            var result = await _walletService.RemoveWalletAsync(id);

            if (result)
            {
                return ApiResponse.SetResponse(ApiResponseStatus.Success, null,
                    _sharedLocalizer["Operation Successfully"].Value);
            }

            return ApiResponse.SetResponse(ApiResponseStatus.Danger, null,
                _sharedLocalizer["Operation Failed"].Value);
        }

        #endregion
    }
}
