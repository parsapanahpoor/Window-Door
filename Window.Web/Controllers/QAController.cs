using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Web.HttpServices;

namespace Window.Web.Controllers
{
    public class QAController : SiteBaseController
    {
        #region Ctor

        private readonly IQuestionService _questionService;

        public QAController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region List Of Question And Answers

        public async Task<IActionResult> ListOfQuestionAndAnswers()
        {
            #region Fill Model 

            var model = await _questionService.GetListOfQuestions();

            if (model == null)
            {
                TempData[ErrorMessage] = "اطلاعاتی یافت نشده است.";
                return RedirectToAction("Index" , "Home" );
            }

            #endregion

            return View(model);
        }

        #endregion

    }
}
