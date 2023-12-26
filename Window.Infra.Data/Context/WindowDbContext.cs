#region Usings

using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Window.Domain.Entities;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.BulkSMS;
using Window.Domain.Entities.Comment;
using Window.Domain.Entities.Contact;
using Window.Domain.Entities.Contract;
using Window.Domain.Entities.Counseling;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Inquiry;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.QuestionAnswer;
using Window.Domain.Entities.Sample;
using Window.Domain.Entities.Score;
using Window.Domain.Entities.Segment;
using Window.Domain.Entities.ShopBrands;
using Window.Domain.Entities.ShopColors;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Entities.SiteSetting;
using Window.Domain.Entities.TechnicalIssues;
using Window.Domain.Entities.Wallet;

namespace Window.Data.Context;

#endregion

public class WindowDbContext : DbContext
{
    #region constructor

    public WindowDbContext(DbContextOptions<WindowDbContext> options) : base(options) { }

    #endregion

    #region db sets

    #region Technical Issues

    public DbSet<TechnicalIssues> TechnicalIssues { get; set; }

    #endregion

    #region Counseling

    public DbSet<Counseling> Counselings { get; set; }

    #endregion

    #region Account 

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    #endregion

    #region Wallet

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletData> WalletData { get; set; }

    #endregion

    #region Ticket

    public DbSet<Ticket> Ticket { get; set; }

    public DbSet<TicketMessage> TicketMessages { get; set; }

    #endregion

    #region Bulk SMS

    public DbSet<BulkSMS> BulkSMS { get; set; }

    public DbSet<AllBulkSMS> AllBulkSMSs { get; set; }

    #endregion

    #region Contract 

    public DbSet<RequestForContract> RequestForContracts { get; set; }

    #endregion

    #region Seller 

    public DbSet<MarketLinks> MarketLinks { get; set; }

    public DbSet<MarketWorkSamle> MarketWorkSamle { get; set; }

    public DbSet<MarketPersonalInfo> MarketPersonalInfo { get; set; }

    public DbSet<ScoreForMarket> ScoreForMarkets { get; set; }

    public DbSet<SehateEtelaAt> SehateEtelaAt { get; set; }

    public DbSet<PasokhGoie> PasokhGoie { get; set; }

    public DbSet<TaAhodeZamaneTahvil> TaAhodeZamaneTahvil { get; set; }

    public DbSet<KhadamatePasAzForosh> KhadamatePasAzForosh { get; set; }

    #endregion

    #region Market

    public DbSet<Market> Market { get; set; }

    public DbSet<MarketUsers> MarketUser { get; set; }

    public DbSet<MarketChargeInfo> MarketChargeInfo { get; set; }

    public DbSet<SelersPersonalVideos> SelersPersonalVideos { get; set; }

    #endregion

    #region Article 

    public DbSet<Article> Articles { get; set; }

    #endregion

    #region Question And Answer

    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    #endregion

    #region Glass

    public DbSet<Glass> Glasses { get; set; }

    #endregion

    #region Brand 

    public DbSet<MainBrand> MainBrands { get; set; }

    public DbSet<YaraghBrand> YaraghBrands { get; set; }

    #endregion

    #region Segment

    public DbSet<Segment> Segments { get; set; }

    #endregion

    #region State 

    public DbSet<State> States { get; set; }

    #endregion

    #region Product

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductTypeCategory> ProductTypeCategories { get; set; }

    public DbSet<ProductMainBrandPrice> ProductMainBrandPrices { get; set; }

    public DbSet<ProductYaraghBrandPrice> ProductYaraghBrandPrices { get; set; }

    public DbSet<SegmentPricing> SegmentPricings { get; set; }

    public DbSet<GlassPricing> GlassPricings { get; set; }

    #endregion

    #region Sample

    public DbSet<Sample> Samples { get; set; }

    public DbSet<SampleSelectedSegment> SampleSelectedSegments { get; set; }

    #endregion

    #region Log 

    public DbSet<LogForVisitSellerProfile> LogForVisitSellerProfiles { get; set; }

    public DbSet<LogForBrands> LogForBrands { get; set; }

    public DbSet<LogForInquiry> LogForInquiry { get; set; }

    public DbSet<LogResultOfUserInquiryWithSellersInfo> LogResultOfUserInquiryWithSellersInfos { get; set; }

    #endregion

    #region Site Setting

    public DbSet<EmailSetting> EmailSettings { get; set; }

    public DbSet<SiteSetting> SiteSettings { get; set; }

    #endregion

    #region Inquiry

    public DbSet<LogInquiryForUser> LogInquiryForUsers { get; set; }

    public DbSet<LogInquiryForUserDetail> logInquiryForUserDetails { get; set; }

    #endregion

    #region Comment

    public DbSet<Comment> Comments { get; set; }

    #endregion

    #region Score

    public DbSet<CalculatedSellersScore> CalculatedSellersScores { get; set; }

    #endregion

    #region Shop Product

    public DbSet<ShopProduct> ShopProducts { get; set; }

    #endregion

    #region Shop Categories 

    public DbSet<ShopCategory> ShopCategories { get; set; }

    #endregion

    #region Shop Brands

    public DbSet<ShopBrand> ShopBrands { get; set; }

    #endregion

    #region Shop Colors

    public DbSet<ShopColor> ShopColors { get; set; }

    #endregion

    #endregion

    #region model creating

    public string culture = CultureInfo.CurrentCulture.Name;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Role Seed Data

        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = 1,
            Title = "Admin",
            RoleUniqueName = "Admin",
            CreateDate = DateTime.Now,
            IsDelete = false
        });
   
        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = 2,
            Title = "Support",
            RoleUniqueName = "Support",
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = 3,
            Title = "Seller",
            RoleUniqueName = "Seller",
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = 4,
            Title = "SellerMaster",
            RoleUniqueName = "SellerMaster",
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        #endregion

        #region Product Type Category Seed Date 

        modelBuilder.Entity<ProductTypeCategory>().HasData(new ProductTypeCategory
        {
            Id = 1,
            Name = "کشویی",
            ProductType = Domain.Enums.Types.ProductType.Keshoie,
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        modelBuilder.Entity<ProductTypeCategory>().HasData(new ProductTypeCategory
        {
            Id = 2,
            Name = "کتیبه",
            ProductType = Domain.Enums.Types.ProductType.Keshoie,
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        modelBuilder.Entity<ProductTypeCategory>().HasData(new ProductTypeCategory
        {
            Id = 3,
            Name = "کتیبه",
            ProductType = Domain.Enums.Types.ProductType.Lolaie,
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        modelBuilder.Entity<ProductTypeCategory>().HasData(new ProductTypeCategory
        {
            Id = 4,
            Name = "درب",
            ProductType = Domain.Enums.Types.ProductType.Lolaie,
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        modelBuilder.Entity<ProductTypeCategory>().HasData(new ProductTypeCategory
        {
            Id = 5,
            Name = "لولایی",
            ProductType = Domain.Enums.Types.ProductType.Lolaie,
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        #endregion

        #region Email Setting Seed Data

        var date = new DateTime(2022, 03, 01);

        modelBuilder.Entity<EmailSetting>().HasData(new EmailSetting
        {
            Id = 1,
            Password = "54511441",
            IsDelete = false,
            CreateDate = date,
            IsDefaultEmail = true,
            DisplayName = "برنامه ی درب و پنجره",
            From = "parsapanahpoor77@gmail.com",
            Smtp = "smtp.gmail.com",
            EnableSsL = true,
            Port = 587,
            UserName = "برنامه ی درب و پنجره"
        });

        #endregion

        #region config relations

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs) fk.DeleteBehavior = DeleteBehavior.Restrict;

        #endregion
    }

    #endregion
}
