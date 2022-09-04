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
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Article.Admin;

namespace Window.Application.Services.Services
{
    public class ArticleService : IArticleService
    {
        #region Constructor

        private readonly WindowDbContext _context;
        private readonly IUserService _userService;

        public ArticleService(WindowDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #endregion

        #region Mian Methods

        public async Task<List<Article>?> GetListOfArticles()
        {
            return await _context.Articles.Where(p=> !p.IsDelete && p.IsActive).ToListAsync();
        }

        public async Task<bool> IsExistAnyArticle()
        {
            return await _context.Articles.AnyAsync(p => !p.IsDelete);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
        }

        public async Task<Article> GetArticleByIdAsync(ulong ArticleId)
        {
            var article = await _context.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == ArticleId && !a.IsDelete);

            return article;
        }

        public async Task<Article> GetArticleByIdWhitoutAuthorAsync(ulong ArticleId)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == ArticleId && !a.IsDelete);
               
            return article;
        }

        #endregion

        #region Admin Side

        public async Task<bool> DeleteArticleFromAdminPanel(ulong ArticleId)
        {
            var article = await GetArticleByIdAsync(ArticleId);

            if (article == null || article.IsDelete)
            {
                return false;
            }

            article.IsDelete = true;

            UpdateArticle(article);
            await SaveChangesAsync();

            return true;
        }

        public async Task<FilterArticleAdminSideViewModel> FilterArticleAdminSideViewModel(FilterArticleAdminSideViewModel filter)
        {
            var query = _context.Articles
                .Include(p => p.Author)
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State 

            #region Base On State (IsActive , IsDelete , ... )

            switch (filter.filterArticleAdminSideState)
            {
                case FilterArticleAdminSideState.All:
                    break;
                case FilterArticleAdminSideState.IsActive:
                    query = query.Where(s => s.IsActive == true);
                    break;
                case FilterArticleAdminSideState.NotActive:
                    query = query.Where(s => s.IsActive == false);
                    break;
            }

            #endregion

            #region Base On Ordering CreateDate

            switch (filter.filterArticleAdminSideOrder)
            {
                case FilterArticleAdminSideOrder.CreateDate_Des:
                    break;
                case FilterArticleAdminSideOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
            }

            #endregion

            #endregion

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.AuthorName))
                query = query.Where(s => EF.Functions.Like(s.Author.Username, $"%{filter.AuthorName}%"));

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<CreateArticleFromAdminPanelResponse> CreateArticleFromAdminPanel(CreateArticleAdminViewModel model, IFormFile ArticleImage)
        {
            #region Check Validation

            if (model.AuthorId == null || model.AuthorId == 0)
            {
                return CreateArticleFromAdminPanelResponse.AuthorNotFound;
            }

            if (ArticleImage == null)
            {
                return CreateArticleFromAdminPanelResponse.ImageNotUploaded;
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return CreateArticleFromAdminPanelResponse.DescriptionNotFound;
            }

            if (!ArticleImage.IsImage())
            {
                return CreateArticleFromAdminPanelResponse.ImageNotFound;
            }

            #endregion

            #region Set Datas From ViewModel

            Article article = new Article()
            {
                Title = model.Title.SanitizeText(),
                AuthorId = model.AuthorId.Value,
                Description = model.Description.SanitizeText(),
                ShortDescription = model.ShortDescription.SanitizeText(),
                IsActive = model.IsActive,
            };

            #endregion

            #region Article Image

            var imageName = Guid.NewGuid() + Path.GetExtension(ArticleImage.FileName);
            ArticleImage.AddImageToServer(imageName, FilePaths.ArticleImageOriginPathServer, 400, 300, FilePaths.ArticleImageThumbPathServer);
            article.Image = imageName;

            #endregion

            #region Add Article

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            #endregion

            #region Short Key

            article.ShortKey = article.Id.ToString();
            UpdateArticle(article);
            await SaveChangesAsync();

            #endregion

            return CreateArticleFromAdminPanelResponse.Success;
        }

        public async Task<EditArticleAdminSideViewModel> FillEditArticleAdminSideViewModel(Article article)
        {
            var user = await _userService.GetUserById(article.AuthorId);

            EditArticleAdminSideViewModel model = new EditArticleAdminSideViewModel
            {
                AuthorId = article.AuthorId,
                AuthorName = user.Username,
                Title = article.Title,
                ShortDescription = article.ShortDescription,
                Description = article.Description,
                imagename = article.Image,
                IsActive = article.IsActive,
                Id = article.Id,
            };

            return model;
        }

        public async Task<EditArticleFromAdminPanelResponse> EditArticleFromAdminPanel(EditArticleAdminSideViewModel model, IFormFile ArticleImage)
        {
            #region Check Validation

            if (model.AuthorId == null || model.AuthorId == 0)
            {
                return EditArticleFromAdminPanelResponse.AuthorNotFound;
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return EditArticleFromAdminPanelResponse.DescriptionNotFound;
            }

            if (ArticleImage != null && !ArticleImage.IsImage())
            {
                return EditArticleFromAdminPanelResponse.ImageNotFound;
            }

            var article = await GetArticleByIdAsync(model.Id);

            if (article == null)
            {
                return EditArticleFromAdminPanelResponse.ArticleNotFound;
            }

            #endregion

            #region Fill Properties

            article.Title = model.Title.SanitizeText();
            article.AuthorId = model.AuthorId.Value;
            article.ShortDescription = model.ShortDescription.SanitizeText();
            article.Description = model.Description.SanitizeText();
            article.IsActive = model.IsActive;

            #endregion

            #region Article Image

            if (ArticleImage != null)
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(ArticleImage.FileName);
                ArticleImage.AddImageToServer(imageName, FilePaths.ArticleImageOriginPathServer, 400, 300, FilePaths.ArticleImageThumbPathServer);

                if (!string.IsNullOrEmpty(article.Image))
                {
                    article.Image.DeleteImage(FilePaths.ArticleImageOriginPathServer, FilePaths.ArticleImageThumbPathServer);
                }

                article.Image = imageName;
            }

            #endregion

            UpdateArticle(article);
            await SaveChangesAsync();

            return EditArticleFromAdminPanelResponse.Success;
        }

        #endregion
    }
}
