using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Window.Domain.Entities.Article;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;

namespace Window.Application.Services.Interfaces
{
    public interface IArticleService
    {
        #region Main Methodes

        Task SaveChangesAsync();
        void UpdateArticle(Article article);
        Task<Article> GetArticleByIdAsync(ulong ArticleId);
        Task<bool> IsExistAnyArticle();

        #endregion

        #region Admin Side

        Task<FilterArticleAdminSideViewModel> FilterArticleAdminSideViewModel(FilterArticleAdminSideViewModel filter);
        Task<CreateArticleFromAdminPanelResponse> CreateArticleFromAdminPanel(CreateArticleAdminViewModel model, IFormFile ArticleImage);
        Task<EditArticleAdminSideViewModel> FillEditArticleAdminSideViewModel(Article article);
        Task<EditArticleFromAdminPanelResponse> EditArticleFromAdminPanel(EditArticleAdminSideViewModel model, IFormFile ArticleImage);
        Task<bool> DeleteArticleFromAdminPanel(ulong ArticleId);

        #endregion

    }
}
