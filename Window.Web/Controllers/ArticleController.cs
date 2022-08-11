using CRM.Domain.DTOs.StructuredApiDtos.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Article;
using Window.Web.HttpServices;

namespace Window.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
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

        [HttpPost("get-articles")]
        public async Task<IActionResult> GetArticles(string? pageId)
        {
            var model = await _articleService.GetListOfArticles();

            #region Get Page Id

            int PageId = 1;

            if (pageId != null)
            {
                PageId = Int32.Parse(pageId);
            }

            #endregion

            #region Paginaition

            int take = 20;

            int skip = (PageId - 1) * take;

            int pageCount = (model.Count() / take);

            if ((pageCount % 2) == 0 || (pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = model.Skip(skip).Take(take).ToList();

            var viewModel = Tuple.Create(query, pageCount);

            #endregion

            return ApiResult.SetResponse(ApiResultEnum.Success, null , model);
            
        }

        #endregion
    }
}
