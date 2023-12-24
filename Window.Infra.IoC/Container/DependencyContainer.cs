using CRM.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Common.UnitOfWork;
using Window.Application.Interfaces;
using Window.Application.Services;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Data.Repository;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.ShopCategory;
using Window.Infra.Data.Repository;
using Window.Infra.Data.Repository.ShopCategory;

namespace Window.IOC.Container
{
	public static class DependencyContainer
    {
        public static void ConfigureDependencies(IServiceCollection services)
        {
            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ISellerPersonalVideoRepository, SellerPersonalVideoRepository>();
            services.AddScoped<IShopCategoryCommandRepository, ShopCategoryCommandRepository>();
            services.AddScoped<IShopCategoryQueryRepository, ShopCategoryQueryRepository>();
            services.AddScoped<IShopCategoryCommandRepository, ShopCategoryCommandRepository>();
            services.AddScoped<IShopCategoryQueryRepository, ShopCategoryQueryRepository>();

            #endregion

            #region Serviecs

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IMarketService, MarketService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ISegmentService, SegmentService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISampleService, SampleService>();
            services.AddScoped<IInquiryService, InquryService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISMSService, SMSService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<ITechnicalIssues, TechnicalIssuesService>();
            services.AddScoped<IConsulantService, ConsultantService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IBulkSMSService, BulkSMSService>();
            services.AddScoped<ISellerPersonalVideoService, SellerPersonalVideoService>();
            services.AddScoped<IShopCategoryService, ShopCategoryService>();
            services.AddScoped<IShopProductService, ShopProductService>();

            #endregion

            #region Unit Of Work 

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion
        }
    }
}
