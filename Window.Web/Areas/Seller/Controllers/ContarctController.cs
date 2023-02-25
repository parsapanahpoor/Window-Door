using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.Seller.Contract;

namespace Window.Web.Areas.Seller.Controllers
{
    public class ContarctController : SellerBaseController
    {
        #region Ctor 

        private readonly IContractService _contractService;
        private readonly ISellerService _sellerService;

        public ContarctController(IContractService contractService, ISellerService sellerService)
        {
            _contractService = contractService;
            _sellerService = sellerService;
        }

        #endregion

        #region List Of Contarcts

        public async Task<IActionResult> ListOfContracts(FilterContractRequestSellerSideViewModel filtre)
        {
            filtre.SellerUserId = User.GetUserId();
            return View(await _contractService.FilterContractRequestSellerSide(filtre));
        }

        #endregion

        #region Accept Request From Seller

        public async Task<IActionResult> AcceptRequest(ulong requestId)
        {
            var res = await _contractService.AcceptRequestFromSeller(requestId , User.GetUserId());
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(ListOfContracts));
            }

            #region Update Seller Activation Tariff

            var request =await _contractService.GetRequestByRequestId(requestId);
            if (request is not null)
            {
                await _sellerService.UpdateSellerActivationTariff(request.UserId, false, true);
            }

            #endregion

            TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
            return RedirectToAction(nameof(ListOfContracts));
        }

        #endregion

        #region Decline Request From Seller

        public async Task<IActionResult> DeclineRequestFromSeller(ulong requestId)
        {
            var res = await _contractService.DeclineRequestFromSeller(requestId , User.GetUserId());
            if (res)
            {
                TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
                return RedirectToAction(nameof(ListOfContracts));
            }

            TempData[ErrorMessage] = "عملیات باشکست مواجه شده است.";
            return RedirectToAction(nameof(ListOfContracts));
        }

        #endregion
    }
}
