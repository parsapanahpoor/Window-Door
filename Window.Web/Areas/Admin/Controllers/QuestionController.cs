using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Article;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Interfaces;
using Window.Domain.ViewModels.Admin.QuestionAnswer;

namespace Window.Web.Areas.Admin.Controllers
{
    public class QuestionController : AdminBaseController
    {
        #region Constructor

        private readonly IQuestionService _articleService;
        private readonly IUserService _userService;

        public QuestionController(IQuestionService articleService, IUserService userService)
        {
            _articleService = articleService;
            _userService = userService;
        }

        #endregion

        #region List OF Articles

        [HttpGet]
        public async Task<IActionResult> Index(FilterQuestionAnswerAdminSideViewModel filter)
        {
            var result = await _articleService.FilterArticleAdminSideViewModel(filter);

            return View(result);
        }

        #endregion

        #region Create Article
        
        [HttpGet]
        public async Task<IActionResult> CreateArticle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(CreateQuestionAnswerAdminViewModel model)
        {
            var result = await _articleService.CreateArticleFromAdminPanel(model);

            switch (result)
            {
                case CreateQuestionAnswerFromAdminPanelResponse.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("Index", "Question");

                case CreateQuestionAnswerFromAdminPanelResponse.Faild:
                    TempData[ErrorMessage] = "درج با مشکل مواجه شده است ";
                    break;
            }

            return View(model);
        }

        #endregion

        #region Edit Article

        [HttpGet]
        public async Task<IActionResult> EditArticle(ulong Id)
        {
            var article = await _articleService.GetQuestionByIdAsync(Id);

            if (article == null)
            {
                TempData[ErrorMessage] = " یافت نشده است   ";
                return RedirectToAction("Index" , "Question", new { Areas = "Admin"} );
            }

            var model = await _articleService.FillEditArticleAdminSideViewModel(article);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(EditQuestionAnswerAdminSideViewModel model )
        {
            var result = await _articleService.EditArticleFromAdminPanel(model);

            switch (result)
            {
                case EditQuestionAnswerFromAdminPanelResponse.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("Index", "Question");

                case EditQuestionAnswerFromAdminPanelResponse.Faild:
                    TempData[ErrorMessage] = "درج با مشکل مواجه شده است ";
                    break;
            }

            return View(model);
        }

        #endregion

        #region Delete Article

        public async Task<IActionResult> DeleteArticle(ulong Id)
        {
            var result = await _articleService.DeleteQuestionFromAdminPanel(Id);

            if (result)
            {
                return JsonResponseStatus.Success( );
            }

            return JsonResponseStatus.Error();
        }

        #endregion

    }
}
