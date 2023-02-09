using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.Contract;
using Window.Domain.ViewModels.Seller.Contract;

namespace Window.Web.Areas.Admin.Controllers
{
    public class ContractController : AdminBaseController
    {
        #region Ctor 

        private readonly IContractService _contractService;
        private readonly IStateService _stateService;

        public ContractController(IContractService contractService, IStateService stateService)
        {
            _contractService = contractService;
            _stateService = stateService;
        }

        #endregion

        #region Filter Contracts

        [HttpGet]
        public async Task<IActionResult> FilterContract(FiltreContractAdminSideViewModel filtre)
        {
            #region Location ViewBags 

            ViewData["Countries"] = await _stateService.GetAllCountries();

            if (filtre.CountryId != null)
            {
                ViewData["States"] = await _stateService.GetStateChildren(filtre.CountryId.Value);
                if (filtre.StateId != null)
                {
                    ViewData["Cities"] = await _stateService.GetStateChildren(filtre.StateId.Value);
                }
            }

            #endregion

            return View(await _contractService.FiltreContractAdminSide(filtre));
        }

        #endregion
    }
}
