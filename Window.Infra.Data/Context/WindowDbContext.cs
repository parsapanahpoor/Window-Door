using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Contact;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.QuestionAnswer;
using Window.Domain.Entities.Segment;
using Window.Domain.Entities.Wallet;

namespace Window.Data.Context
{
    public class WindowDbContext : DbContext
    {
        #region constructor

        public WindowDbContext(DbContextOptions<WindowDbContext> options) : base(options) { }

        #endregion

        #region db sets

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

        #region Seller 

        public DbSet<MarketLinks> MarketLinks { get; set; }

        public DbSet<MarketWorkSamle> MarketWorkSamle { get; set; }

        public DbSet<MarketPersonalInfo> MarketPersonalInfo { get; set; }

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

            #region Product Type Category

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

            #region config relations

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs) fk.DeleteBehavior = DeleteBehavior.Restrict;

            #endregion
        }

        #endregion
    }
}
