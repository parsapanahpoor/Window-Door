using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Article;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;
using Window.Web.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Interfaces;

namespace Window.Web.Areas.Admin.Controllers
{
    [PermissionChecker("ManageArticles")]
    public class ArticleController : AdminBaseController
    {
        #region Constructor

        private readonly IArticleService _articleService;
        private readonly IUserService _userService;

        public ArticleController(IArticleService articleService, IUserService userService)
        {
            _articleService = articleService;
            _userService = userService;
        }

        #endregion

        #region List OF Articles

        [HttpGet]
        public async Task<IActionResult> Index(FilterArticleAdminSideViewModel filter)
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
        public async Task<IActionResult> CreateArticle(CreateArticleAdminViewModel model, IFormFile ArticleImage)
        {
            var result = await _articleService.CreateArticleFromAdminPanel(model, ArticleImage);

            switch (result)
            {
                case CreateArticleFromAdminPanelResponse.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("Index", "Article");

                case CreateArticleFromAdminPanelResponse.Faild:
                    TempData[ErrorMessage] = "درج مقاله با مشکل مواجه شده است ";
                    break;

                case CreateArticleFromAdminPanelResponse.ImageNotUploaded:
                    TempData[ErrorMessage] = "مقاله باید دارای تصویر باشد  ";
                    break;

                case CreateArticleFromAdminPanelResponse.AuthorNotFound:
                    TempData[ErrorMessage] = "نویسنده ی مقاله یافت نشده است  ";
                    break;

                case CreateArticleFromAdminPanelResponse.DescriptionNotFound:
                    TempData[ErrorMessage] = "لطفا توضیحات کامل را وراد کنید  ";
                    break;

                case CreateArticleFromAdminPanelResponse.ImageNotFound:
                    TempData[ErrorMessage] = "لطفا تصویر مقاله ی خود را انتخاب کنید  ";
                    break;
            }

            return View(model);
        }

        #endregion

        #region Edit Article

        [HttpGet]
        public async Task<IActionResult> EditArticle(ulong Id)
        {
            var article = await _articleService.GetArticleByIdAsync(Id);

            if (article == null)
            {
                TempData[ErrorMessage] = "مقاله ای یافت نشده است   ";
                return RedirectToAction("Index" ,"Article" , new { Areas = "Admin"} );
            }

            var model = await _articleService.FillEditArticleAdminSideViewModel(article);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(EditArticleAdminSideViewModel model , IFormFile? ArticleImage)
        {
            var result = await _articleService.EditArticleFromAdminPanel(model, ArticleImage);

            switch (result)
            {
                case EditArticleFromAdminPanelResponse.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("Index", "Article");

                case EditArticleFromAdminPanelResponse.Faild:
                    TempData[ErrorMessage] = "درج مقاله با مشکل مواجه شده است ";
                    break;

                case EditArticleFromAdminPanelResponse.ImageNotUploaded:
                    TempData[ErrorMessage] = "مقاله باید دارای تصویر باشد  ";
                    break;

                case EditArticleFromAdminPanelResponse.AuthorNotFound:
                    TempData[ErrorMessage] = "نویسنده ی مقاله یافت نشده است  ";
                    break;

                case EditArticleFromAdminPanelResponse.DescriptionNotFound:
                    TempData[ErrorMessage] = "لطفا توضیحات کامل را وراد کنید  ";
                    break;

                case EditArticleFromAdminPanelResponse.ImageNotFound:
                    TempData[ErrorMessage] = "لطفا تصویر مقاله ی خود را انتخاب کنید  ";
                    break;

                case EditArticleFromAdminPanelResponse.ArticleNotFound:
                    TempData[ErrorMessage] = "مقاله ای یافت نشده است   ";
                    break;
            }

            return View(model);
        }

        #endregion

        #region Delete Article

        public async Task<IActionResult> DeleteArticle(ulong Id)
        {
            var result = await _articleService.DeleteArticleFromAdminPanel(Id);

            if (result)
            {
                return JsonResponseStatus.Success( );
            }

            return JsonResponseStatus.Error();
        }

        #endregion

    }
}
