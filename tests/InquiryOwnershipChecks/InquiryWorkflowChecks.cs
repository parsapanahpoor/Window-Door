using Microsoft.EntityFrameworkCore;
using Window.Application.Services.Services;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Inquiry;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.Market;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.Sample;
using Window.Domain.Entities.Segment;
using Window.Domain.Entities.SiteSetting;
using Window.Domain.Enums.SellerType;
using Window.Domain.ViewModels.Admin.Log;

internal static class InquiryWorkflowChecks
{
    public static async Task RunAsync(WindowDbContext seed, DbContextOptions<WindowDbContext> options)
    {
        const string token = "web_fixture_pricing";
        var country = new State { Title = "Test country", UniqueName = "test-country" };
        var state = new State { Title = "Test state", UniqueName = "test-state", Parent = country };
        var city = new State { Title = "Test city", UniqueName = "test-city", Parent = state };
        var seller = new User
        {
            Username = "Fixture seller", Mobile = "09100000991", Password = Window.Application.Security.PasswordHelper.EncodePasswordMd5("fixture-password"), IsMobileConfirm = true,
            EmailActivationCode = "fixture", MobileActivationCode = "fixture"
        };
        var market = new Market { User = seller, MarketName = "Fixture market", MarketPersonalsInfoState = MarketPersonalsInfoState.ActiveMarketAccount };
        seed.MarketPersonalInfo.Add(new MarketPersonalInfo
        {
            Market = market, User = seller, Country = country, State = state, City = city,
            PhotoOfBusinessLicense = "fixture", PhotoOfNationalCode = "fixture", CompanyName = "fixture",
            CompanyLogo = "fixture", Resume = "fixture", PeriodTimesOfWork = "fixture", WorkAddress = "fixture"
        });
        var brand = new MainBrand { BrandName = "Fixture aluminum", Alominum = true };
        var glass = new Glass { GlassName = "Fixture glass" };
        seed.MainBrands.Add(brand);
        seed.Glasses.Add(glass);
        seed.SiteSettings.Add(new SiteSetting
        {
            ChargeTariffAboutListOfInquiry = 7, AlertForSellerForSeenProfile = "fixture",
            SMSForDisActiveFromToday = "fixture", SMSForDisActiveFrom3Day = "fixture", SMSForDisActiveFrom15Day = "fixture"
        });
        for (var i = 0; i < 52; i++)
        {
            seed.Segments.Add(new Segment { SegmentName = $"Fixture part {i}", Aluminum = true });
            await seed.SaveChangesAsync();
        }
        for (var i = 0; i < 10; i++)
        {
            seed.Samples.Add(new Sample { SegmentName = $"Fixture sample {i}", Image = "fixture.png", Aluminum = true, Window = true, Lolaie = true, MaxHeight = 500, MaxWidth = 500 });
            await seed.SaveChangesAsync();
        }
        var product = new Product
        {
            User = seller, MainBrand = brand, CountryId = country.Id, StateId = state.Id, CityId = city.Id, SellerType = SellerType.Aluminium
        };
        seed.SegmentPricings.AddRange(
            new SegmentPricing { Product = product, SegmentId = 52, Price = 100 },
            new SegmentPricing { Product = product, SegmentId = 47, Price = 50 });
        seed.GlassPricings.Add(new GlassPricing { User = seller, Glass = glass, Price = 200 });
        seed.SampleSelectedSegments.AddRange(
            new SampleSelectedSegment { SampleId = 10, SegmentId = 52 },
            new SampleSelectedSegment { SampleId = 10, SegmentId = 47 },
            new SampleSelectedSegment { SampleId = 9, SegmentId = 1 });
        var inquiry = new LogInquiryForUser
        {
            UserMAcAddress = token, SellerType = SellerType.Aluminium,
            CountryId = country.Id, StateId = state.Id, CityId = city.Id,
            BrandId = brand.Id, GlassId = glass.Id
        };
        seed.LogInquiryForUsers.Add(inquiry);
        await seed.SaveChangesAsync();
        await InquiryHttpChecks.RunAsync(options, state.Id, city.Id, glass.Id, seller.Mobile);
        var tariffBeforeCompletion = await seed.Market.AsNoTracking().Where(row => row.Id == market.Id)
            .Select(row => row.ActivationTariff).SingleAsync();

        seed.LogForInquiry.AddRange(
            new LogForInquiry { CountryId = country.Id, StateId = state.Id, CityId = city.Id, CreateDate = new DateTime(2024, 3, 20, 0, 0, 0) },
            new LogForInquiry { CountryId = country.Id, StateId = state.Id, CityId = city.Id, CreateDate = new DateTime(2024, 3, 20, 23, 59, 59) },
            new LogForInquiry { CountryId = country.Id, StateId = state.Id, CityId = city.Id, CreateDate = new DateTime(2024, 3, 21, 0, 0, 0) });
        await seed.SaveChangesAsync();
        var adminReport = await new SellerService(seed, null!, null!, null!).FilterLogForInquiryViewModel(
            new FilterLogForInquiryViewModel { FromDate = "۱۴۰۳/۰۱/۰۱", ToDate = "۱۴۰۳/۰۱/۰۱" });
        Check(adminReport.Entities.Count == 2, "A single-day report must include midnight and the full last day, excluding the next midnight.");

        // Sample 10, 1m x 1m: four metres each of two profiles and one square metre of glass.
        await using (var context = new WindowDbContext(options))
        {
            var service = new InquryService(context, null!);
            Check(await service.InitialResultOfUserInquiry(10, 100, 100, 1, null, token, token), "Sample 10 must be added.");
            await CheckPrice(context, inquiry.Id, 800);
            Check(await service.InitialResultOfUserInquiry(9, 100, 100, 1, null, token, token), "Second sample must be added.");
            Check(!await context.LogResultOfUserInquiryWithSellersInfos.AnyAsync(row => row.LogInquiryForUserId == inquiry.Id),
                "An incomplete two-item basket must not retain a one-item quote.");
            var secondItem = await context.logInquiryForUserDetails.SingleAsync(row => row.LogInquiryForUserId == inquiry.Id && row.SampleId == 9);
            Check(await service.DeleteUserLastestInquiryDetail(secondItem.Id, token), "Second item must be removable.");
            await CheckPrice(context, inquiry.Id, 800);
            var firstItem = await context.logInquiryForUserDetails.SingleAsync(row => row.LogInquiryForUserId == inquiry.Id && !row.IsDelete);
            Check(await service.UpdateUserInquiryItrm(firstItem.Id, 10, 100, 100, null, token, 2), "Quantity change must succeed.");
            await CheckPrice(context, inquiry.Id, 1600);
        }

        await Task.WhenAll(CompleteInSeparateContext(), CompleteInSeparateContext());
        await using (var context = new WindowDbContext(options))
        {
            Check(await context.InquirySellerExposures.CountAsync(row => row.InquiryId == inquiry.Id && row.SellerId == seller.Id) == 1,
                "Concurrent completion must record exactly one exposure.");
            Check(await context.Market.Where(row => row.Id == market.Id).Select(row => row.ActivationTariff).SingleAsync() == tariffBeforeCompletion + 7,
                "Concurrent completion must charge the seller once.");

            var service = new InquryService(context, null!);
            for (var i = 0; i < 3; i++)
                Check(await service.InitialResultOfUserInquiry(10, 100, 100, 1, null, token, token), "Fill the basket to four items.");
        }
        var insertions = await Task.WhenAll(AddInSeparateContext(), AddInSeparateContext());
        Check(insertions.Count(added => added) == 1, "Exactly one parallel request may add the fifth item.");

        seed.MarketUser.Add(new MarketUsers { UserId = seller.Id, MarketId = market.Id, RoleId = 3 });
        await seed.SaveChangesAsync();
        await using (var context = new WindowDbContext(options))
        {
            var pricing = new ProductService(context);
            Check(!await pricing.AddPricingForGlass(glass.Id, 0, seller.Id), "Zero glass price must be rejected.");
            Check(!await pricing.AddPricingForSegment(product.Id, 52, -1, seller.Id), "Negative segment price must be rejected.");
            Check(!await pricing.AddPricingForSegment(product.Id, 52, 110, seller.Id + 1000), "Foreign user must not write product rates.");
        }

        await Task.WhenAll(SetRate(225), SetRate(250));
        Check(await seed.SegmentPricings.CountAsync(rate => rate.ProductId == product.Id && rate.SegmentId == 1) == 1,
            "Concurrent first-time writes must create only one rate.");

        await seed.Database.ExecuteSqlRawAsync("""
            ALTER TABLE dbo.SampleSelectedSegments ADD CONSTRAINT CK_FixtureRejectSampleSegment CHECK (SampleId <> 10 OR SegmentId <> 1);
            """);
        try
        {
            await using var context = new WindowDbContext(options);
            var proposed = await context.Samples.AsNoTracking().SingleAsync(sample => sample.Id == 10);
            proposed.SegmentName = "Must roll back";
            try
            {
                await new SampleService(context).EditSample(proposed, [1, 52], null);
                throw new InvalidOperationException("Expected the synthetic sample-part write to fail.");
            }
            catch (DbUpdateException) { }
        }
        finally
        {
            await seed.Database.ExecuteSqlRawAsync("ALTER TABLE dbo.SampleSelectedSegments DROP CONSTRAINT CK_FixtureRejectSampleSegment");
        }
        Check(await seed.Samples.AsNoTracking().AnyAsync(sample => sample.Id == 10 && sample.SegmentName == "Fixture sample 9"),
            "Failed part writes must preserve the old sample title.");
        Check(await seed.SampleSelectedSegments.CountAsync(part => part.SampleId == 10 && (part.SegmentId == 47 || part.SegmentId == 52)) == 2,
            "Failed part writes must preserve existing components.");

        await seed.Products.Where(row => row.Id == product.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(row => row.IsDelete, true));
        await CompleteInSeparateContext();
        Check(!await seed.LogResultOfUserInquiryWithSellersInfos.AnyAsync(row => row.LogInquiryForUserId == inquiry.Id),
            "Rates from a deleted product cannot appear in the results.");
        Check(await seed.LogInquiryForUsers.AnyAsync(row => row.Id == inquiry.Id && row.LastCompletionHasQuote == false),
            "Completion without a quote must be recorded separately.");
        Check(await seed.InquirySellerExposures.CountAsync(row => row.InquiryId == inquiry.Id) == 1,
            "A completion without a quote must not add a seller charge.");

        Check(FilterLogForInquiryViewModel.TryReadDate("۱۴۰۵/۰۹/۰۱", out var date)
            && date == new DateTime(2026, 11, 22), "Persian digits and Jalali date must parse.");
        Check(!FilterLogForInquiryViewModel.TryReadDate("1405/12/30", out _), "Invalid Jalali dates must be rejected.");
        Console.WriteLine("PASS: basket reference price, incomplete coverage, edit/delete repricing, tariff once, item limit and Jalali date parsing.");

        async Task SetRate(int value)
        {
            await using var context = new WindowDbContext(options);
            Check(await new ProductService(context).AddPricingForSegment(product.Id, 1, value, seller.Id), "Seller can register a valid rate.");
        }

        async Task CompleteInSeparateContext()
        {
            await using var context = new WindowDbContext(options);
            await new InquryService(context, null!).CompleteInquiry(token);
        }

        async Task<bool> AddInSeparateContext()
        {
            await using var context = new WindowDbContext(options);
            return await new InquryService(context, null!).InitialResultOfUserInquiry(10, 100, 100, 1, null, token, token);
        }
    }

    private static async Task CheckPrice(WindowDbContext context, ulong inquiryId, double expected)
    {
        var price = await context.LogResultOfUserInquiryWithSellersInfos.AsNoTracking()
            .Where(row => row.LogInquiryForUserId == inquiryId).Select(row => row.Price).SingleAsync();
        Check(price == expected, $"Expected basket price {expected}, got {price}.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
