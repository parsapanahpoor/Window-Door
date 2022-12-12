using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;

namespace Window.Web.Controllers
{
    public class TechnicalIssuesController : Controller
    {
        #region Ctor

        private readonly ITechnicalIssues _technicalIssues;

        public TechnicalIssuesController(ITechnicalIssues technicalIssues)
        {
            _technicalIssues = technicalIssues;
        }

        #endregion

        #region List Of Technical Issus

        public async Task<IActionResult> ListOfTechnicalIssuse()
        {
            #region Model 

            var model = await _technicalIssues.GetListOfTechnicalIssues();
            if (model == null) return NotFound();

            #endregion

            return View(model);
        }

        #endregion
    }
}
