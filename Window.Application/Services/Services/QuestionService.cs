using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Data.Context;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.QuestionAnswer;
using Window.Domain.ViewModels.Admin.QuestionAnswer;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;

namespace Window.Application.Services.Services
{
    public class QuestionService : IQuestionService
    {
        #region Constructor

        private readonly WindowDbContext _context;
        private readonly IUserService _userService;

        public QuestionService(WindowDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #endregion

        #region Mian Methods

        public async Task<bool> IsExistAnyQuestionService()
        {
            return await _context.QuestionAnswers.AnyAsync(p => !p.IsDelete);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateQuestion(QuestionAnswer article)
        {
            _context.QuestionAnswers.Update(article);
        }

        public async Task<QuestionAnswer> GetQuestionByIdAsync(ulong ArticleId)
        {
            var article = await _context.QuestionAnswers
                .FirstOrDefaultAsync(a => a.Id == ArticleId && !a.IsDelete);

            return article;
        }

        #endregion

        #region Admin Side

        public async Task<bool> DeleteQuestionFromAdminPanel(ulong ArticleId)
        {
            var article = await GetQuestionByIdAsync(ArticleId);

            if (article == null || article.IsDelete)
            {
                return false;
            }

            article.IsDelete = true;

            UpdateQuestion(article);
            await SaveChangesAsync();

            return true;
        }

        public async Task<FilterQuestionAnswerAdminSideViewModel> FilterArticleAdminSideViewModel(FilterQuestionAnswerAdminSideViewModel filter)
        {
            var query = _context.QuestionAnswers
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State 

            #region Base On State (IsActive , IsDelete , ... )

            switch (filter.FilterQuestionAnswerAdminSideState)
            {
                case FilterQuestionAnswerAdminSideState.All:
                    break;
                case FilterQuestionAnswerAdminSideState.IsActive:
                    query = query.Where(s => s.IsActive == true);
                    break;
                case FilterQuestionAnswerAdminSideState.NotActive:
                    query = query.Where(s => s.IsActive == false);
                    break;
            }

            #endregion

            #region Base On Ordering CreateDate

            switch (filter.FilterQuestionAnswerAdminSideOrder)
            {
                case FilterQuestionAnswerAdminSideOrder.CreateDate_Des:
                    break;
                case FilterQuestionAnswerAdminSideOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
            }

            #endregion

            #endregion

            #region Filter By Properties

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<CreateQuestionAnswerFromAdminPanelResponse> CreateArticleFromAdminPanel(CreateQuestionAnswerAdminViewModel model)
        {
            #region Set Datas From ViewModel

            QuestionAnswer article = new QuestionAnswer()
            {
                Answer = model.Answer,
                Question = model.Question,
                IsActive = model.IsActive,
            };

            #endregion

            #region Add Article

            await _context.QuestionAnswers.AddAsync(article);
            await _context.SaveChangesAsync();

            #endregion

            return CreateQuestionAnswerFromAdminPanelResponse.Success;
        }

        public async Task<EditQuestionAnswerAdminSideViewModel> FillEditArticleAdminSideViewModel(QuestionAnswer article)
        {

            EditQuestionAnswerAdminSideViewModel model = new EditQuestionAnswerAdminSideViewModel
            {
                Question = article.Question,
                Answer = article.Answer,
                IsActive = article.IsActive,
                Id = article.Id,
            };

            return model;
        }

        public async Task<EditQuestionAnswerFromAdminPanelResponse> EditArticleFromAdminPanel(EditQuestionAnswerAdminSideViewModel model)
        {
            #region Check Validation

            var article = await GetQuestionByIdAsync(model.Id);

            if (article == null)
            {
                return EditQuestionAnswerFromAdminPanelResponse.Faild;
            }

            #endregion

            #region Fill Properties

            article.Answer = model.Answer;
            article.Question = model.Question;
            article.IsActive = model.IsActive;

            #endregion

            UpdateQuestion(article);
            await SaveChangesAsync();

            return EditQuestionAnswerFromAdminPanelResponse.Success;
        }

        #endregion
    }
}
