using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.QuestionAnswer;
using Window.Domain.ViewModels.Admin.QuestionAnswer;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;

namespace Window.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        #region Mian Methods

        Task<bool> IsExistAnyQuestionService();

        Task SaveChangesAsync();

        void UpdateQuestion(QuestionAnswer article);

        Task<QuestionAnswer> GetQuestionByIdAsync(ulong ArticleId);

        #endregion

        #region Admin Side

        Task<bool> DeleteQuestionFromAdminPanel(ulong ArticleId);

        Task<FilterQuestionAnswerAdminSideViewModel> FilterArticleAdminSideViewModel(FilterQuestionAnswerAdminSideViewModel filter);

        Task<CreateQuestionAnswerFromAdminPanelResponse> CreateArticleFromAdminPanel(CreateQuestionAnswerAdminViewModel model);

        Task<EditQuestionAnswerAdminSideViewModel> FillEditArticleAdminSideViewModel(QuestionAnswer article);
        Task<EditQuestionAnswerFromAdminPanelResponse> EditArticleFromAdminPanel(EditQuestionAnswerAdminSideViewModel model);

        #endregion

    }
}
