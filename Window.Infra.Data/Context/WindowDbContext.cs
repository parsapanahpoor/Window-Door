#region Usings

using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
using Window.Domain.Entities.Segment;
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

        //#region Segments Seed Data 

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 1,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "فریم",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 2,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "زهوار دوجداره",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 3,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "لنگه",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 4,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "یراق ملغی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 5,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "یراق تک حالته",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 6,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "لنگه ی بازشوی پنجره",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 7,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "مولیون لولایی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 8,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی فریم",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 10,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی لنگه",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 11,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی مولیون",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 12,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "لنگه ی درب",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 13,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی دربی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 14,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "یراق درب سویئچی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 15,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "یراق درب سرویسی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 16,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی لولایی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 17,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "پنل",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 18,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی مولیون لولایی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 19,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "لنگه ی کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 20,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "گالوانیزه ی لنگه ی کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 21,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "نوار مویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 22,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "کاور لنگه ی کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 23,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    Door = true,
        //    Keshoie = true,
        //    Lolaie = true,
        //    UPVC = true,
        //    Window = true,
        //    SegmentName = "مولوین کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 24,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "گالوانیزه ی مولوین کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 25,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "کاور مولوین کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 26,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "کاور بارانگیر",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 27,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "ریل کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 28,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "یراق کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 29,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "ریل کشویی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 30,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "لاستیک فشاری",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 31,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "اینترلاک",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 32,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "فریم لولایی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 33,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "وادار لولایی",
        //});

        //modelBuilder.Entity<Segment>().HasData(new Segment
        //{
        //    Id = 34,
        //    CreateDate = DateTime.Now,
        //    IsDelete = false,
        //    Aluminum = true,
        //    UPVC = true,
        //    Yaragh = false,
        //    SegmentName = "قفل چهارلنگه",
        //});

        //#endregion

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
