using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;

namespace Window.Web.Controllers
{
    public class ConsultantController : SiteBaseController
    {
        #region Ctor

        private readonly IConsulantService _consultantService;

        public ConsultantController(IConsulantService consultantService)
        {
            _consultantService = consultantService;
        }

        #endregion

        #region List Of Consultant 

        public async Task<IActionResult> ListOfConsultant()
        {
            #region Fill Model 

            var model = await _consultantService.ListOfConsoltantForShowSiteSide();
            if (model == null) return NotFound();

            #endregion

            return View(model);
        }

        #endregion
    }
}
