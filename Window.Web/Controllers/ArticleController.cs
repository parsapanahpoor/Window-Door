using Microsoft.AspNetCore.Mvc;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;

namespace Window.Web.Controllers
{
    public class ArticleController : SiteBaseController
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

        #region Get List Of Articles

        public async Task<IActionResult> ListOfArticles(int pageId = 1)
        {
            var model = await _articleService.GetListOfArticles();
            if (model == null) return NotFound();

            #region Paginaition

            ViewBag.pageId = pageId;

            int take = 20;

            int skip = (pageId - 1) * take;

            int pageCount = (model.Count() / take);

            if ((pageCount % 2) == 0 || (pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = model.Skip(skip).Take(take).ToList();

            var viewModel = Tuple.Create(query, pageCount);

            #endregion

            return View(viewModel);
        }

        #endregion

        #region Show Article Detail 

        public async Task<IActionResult> ShowArticleDetailById(ulong id)
        {
            #region Get Article By Id

            var model = await _articleService.GetArticleByIdAsync(id);
            if (model == null) return NotFound();

            #endregion

            return View(model);
        }

        #endregion
    }
}
