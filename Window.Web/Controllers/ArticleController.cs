using CRM.Domain.DTOs.StructuredApiDtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Article;
using Window.Web.HttpManager;
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

        [HttpGet("get-articles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticles()
        {
            var model = await _articleService.GetListOfArticles();

            return JsonResponseStatus.Success(model);
        }

        [HttpGet("get-articles1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticles1()
        {
            var model = await _articleService.GetListOfArticles();

            return new ObjectResult(model);
        }

        [HttpGet("get-articles2")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticles2()
        {
            var model = await _articleService.GetListOfArticles();

            return Ok(model);
        }

        [HttpGet("get-articlesDetail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticles2(ulong id)
        {
            var model = await _articleService.GetArticleByIdWhitoutAuthorAsync(id);

            return Ok(model);
        }

        #endregion
    }
}
