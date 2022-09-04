using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Web.HttpServices;

namespace Window.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QAController : ControllerBase
    {
        #region Ctor

        private readonly IQuestionService _questionService;

        public QAController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region Get List Of Questions 

        [HttpGet("get-QA")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQA(string? pageId)
        {
            var model = await _questionService.GetListOfQuestions();

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

            return Ok(model);

        }

        #endregion

        [HttpGet("get-qaDetail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQADetail(ulong id)
        {
            var model = await _questionService.GetQuestionByIdAsync(id);

            return Ok(model);
        }

    }
}
