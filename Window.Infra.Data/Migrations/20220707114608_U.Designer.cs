﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Window.Data.Context;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    [DbContext(typeof(WindowDbContext))]
    [Migration("20220707114608_U")]
    partial class U
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Window.Domain.Entities.Account.Role", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("RoleUniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1m,
                            CreateDate = new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9520),
                            IsDelete = false,
                            RoleUniqueName = "Admin",
                            Title = "Admin"
                        },
                        new
                        {
                            Id = 2m,
                            CreateDate = new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9548),
                            IsDelete = false,
                            RoleUniqueName = "Support",
                            Title = "Support"
                        },
                        new
                        {
                            Id = 3m,
                            CreateDate = new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9559),
                            IsDelete = false,
                            RoleUniqueName = "Seller",
                            Title = "Seller"
                        },
                        new
                        {
                            Id = 4m,
                            CreateDate = new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9570),
                            IsDelete = false,
                            RoleUniqueName = "SellerMaster",
                            Title = "SellerMaster"
                        });
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.RolePermission", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("PermissionId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.User", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("Avatar")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("BanForTicket")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBan")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("WalletBalance")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.UserRole", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Window.Domain.Entities.Article.Article", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<decimal>("AuthorId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ShortKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Window.Domain.Entities.Contact.Ticket", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReadByAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReadByOwner")
                        .HasColumnType("bit");

                    b.Property<bool>("OnWorking")
                        .HasColumnType("bit");

                    b.Property<decimal>("OwnerId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("Window.Domain.Entities.Contact.TicketMessage", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SenderId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("TicketId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketMessages");
                });

            modelBuilder.Entity("Window.Domain.Entities.Location.State", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal?>("ParentId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("States");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.Market", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<int>("ActivationTariff")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("MarketName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MarketPersonalsInfoState")
                        .HasColumnType("int");

                    b.Property<decimal?>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Market");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketChargeInfo", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("CurrentAccountCharge")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("MarketId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.HasIndex("UserId");

                    b.ToTable("MarketChargeInfo");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketLinks", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("LinkTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MarketId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("MarketId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("MarketLinks");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketPersonalInfo", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<decimal?>("CityId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("CompanyLogo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("CountryId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("MarketId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("MarketPersonalsInfoState")
                        .HasColumnType("int");

                    b.Property<int>("NationalCode")
                        .HasColumnType("int");

                    b.Property<string>("PeriodTimesOfWork")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoOfBusinessLicense")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoOfNationalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resume")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SellerType")
                        .HasColumnType("int");

                    b.Property<decimal?>("StateId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("WorkAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.HasIndex("MarketId")
                        .IsUnique();

                    b.HasIndex("StateId");

                    b.HasIndex("UserId");

                    b.ToTable("MarketPersonalInfo");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketUsers", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("MarketId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("MarketUser");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketWorkSamle", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("MarketId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("WorkSampleImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkSampleTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MarketId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("MarketWorkSamle");
                });

            modelBuilder.Entity("Window.Domain.Entities.QuestionAnswer.QuestionAnswer", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("QuestionAnswers");
                });

            modelBuilder.Entity("Window.Domain.Entities.Wallet.Wallet", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("GatewayType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFinally")
                        .HasColumnType("bit");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<int>("TransactionWay")
                        .HasColumnType("int");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Window.Domain.Entities.Wallet.WalletData", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("GatewayType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("ReferenceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrackingCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("WalletId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("WalletId")
                        .IsUnique();

                    b.ToTable("WalletData");
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.RolePermission", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.UserRole", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Article.Article", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Window.Domain.Entities.Contact.Ticket", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.User", "Owner")
                        .WithMany("Tickets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Window.Domain.Entities.Contact.TicketMessage", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.User", "Sender")
                        .WithMany("TicketMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Contact.Ticket", "Ticket")
                        .WithMany("TicketMessages")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Sender");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Window.Domain.Entities.Location.State", b =>
                {
                    b.HasOne("Window.Domain.Entities.Location.State", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.Market", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.Role", null)
                        .WithMany("Markets")
                        .HasForeignKey("RoleId");

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithOne("Market")
                        .HasForeignKey("Window.Domain.Entities.Market.Market", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketChargeInfo", b =>
                {
                    b.HasOne("Window.Domain.Entities.Market.Market", "MArket")
                        .WithMany("MarketChargeInfos")
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany("MarketChargeInfos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MArket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketLinks", b =>
                {
                    b.HasOne("Window.Domain.Entities.Market.Market", "Market")
                        .WithOne("MarketLinks")
                        .HasForeignKey("Window.Domain.Entities.Market.MarketLinks", "MarketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany("SellerLinks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Market");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketPersonalInfo", b =>
                {
                    b.HasOne("Window.Domain.Entities.Location.State", "City")
                        .WithMany("UserCities")
                        .HasForeignKey("CityId");

                    b.HasOne("Window.Domain.Entities.Location.State", "Country")
                        .WithMany("UserCountries")
                        .HasForeignKey("CountryId");

                    b.HasOne("Window.Domain.Entities.Market.Market", "Market")
                        .WithOne("MarketPersonalInfo")
                        .HasForeignKey("Window.Domain.Entities.Market.MarketPersonalInfo", "MarketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Location.State", "State")
                        .WithMany("UserStates")
                        .HasForeignKey("StateId");

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany("SellersPersonalInfos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Country");

                    b.Navigation("Market");

                    b.Navigation("State");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketUsers", b =>
                {
                    b.HasOne("Window.Domain.Entities.Market.Market", "Market")
                        .WithMany("MarketUsers")
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Account.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Market");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.MarketWorkSamle", b =>
                {
                    b.HasOne("Window.Domain.Entities.Market.Market", "Market")
                        .WithOne("MarketWorkSamle")
                        .HasForeignKey("Window.Domain.Entities.Market.MarketWorkSamle", "MarketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany("SellerWorkSamles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Market");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Wallet.Wallet", b =>
                {
                    b.HasOne("Window.Domain.Entities.Account.User", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Window.Domain.Entities.Wallet.WalletData", b =>
                {
                    b.HasOne("Window.Domain.Entities.Wallet.Wallet", "Wallet")
                        .WithOne("WalletData")
                        .HasForeignKey("Window.Domain.Entities.Wallet.WalletData", "WalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.Role", b =>
                {
                    b.Navigation("Markets");

                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Window.Domain.Entities.Account.User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Market")
                        .IsRequired();

                    b.Navigation("MarketChargeInfos");

                    b.Navigation("SellerLinks");

                    b.Navigation("SellerWorkSamles");

                    b.Navigation("SellersPersonalInfos");

                    b.Navigation("TicketMessages");

                    b.Navigation("Tickets");

                    b.Navigation("UserRoles");

                    b.Navigation("Wallets");
                });

            modelBuilder.Entity("Window.Domain.Entities.Contact.Ticket", b =>
                {
                    b.Navigation("TicketMessages");
                });

            modelBuilder.Entity("Window.Domain.Entities.Location.State", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("UserCities");

                    b.Navigation("UserCountries");

                    b.Navigation("UserStates");
                });

            modelBuilder.Entity("Window.Domain.Entities.Market.Market", b =>
                {
                    b.Navigation("MarketChargeInfos");

                    b.Navigation("MarketLinks")
                        .IsRequired();

                    b.Navigation("MarketPersonalInfo")
                        .IsRequired();

                    b.Navigation("MarketUsers");

                    b.Navigation("MarketWorkSamle")
                        .IsRequired();
                });

            modelBuilder.Entity("Window.Domain.Entities.Wallet.Wallet", b =>
                {
                    b.Navigation("WalletData")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
