using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text.Encodings.Web;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Window.Application.Interfaces;
using Window.Application.CQRS.SellerPanel.SellerChequeInfo.Query.AddOrEditSellerChequeInfo;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.User.Ticket;
using Window.Domain.ViewModels.Seller.PersonalInfo;
using Window.Domain.ViewModels.Seller.SellerChequeInfo;
using Window.Web.Culture;
using SellerHome = Window.Web.Areas.Seller.Controllers.HomeController;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(SellerHome).Assembly.GetName().Name,
    ContentRootPath = Path.GetFullPath("../../../../../Window.Web", AppContext.BaseDirectory)
});
builder.WebHost.UseUrls("http://127.0.0.1:0");
builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(ApplicationRequestLocalization.Configure);
builder.Services.AddControllersWithViews().AddViewLocalization();
builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
builder.Services.AddAuthentication("Fixture").AddScheme<AuthenticationSchemeOptions, FixtureAuthentication>("Fixture", _ => { });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(provider =>
{
    var fixture = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!.Request.Headers["X-Seller-Fixture"].ToString();
    return FixtureService.Create<ISellerService>(name => name switch
    {
        nameof(ISellerService.GetMarketByUserId) => Task.FromResult<Market?>(new Market { Id = 1, UserId = fixture == "employee" ? 2UL : 1UL }),
        nameof(ISellerService.GetStoreProfileProgressAsync) => Task.FromResult<StoreProfileProgress?>(fixture switch
        {
            "pending" => new(false, false, false, MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller),
            "setup-links" => new(true, false, false, MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller),
            "setup-sample" => new(true, true, false, MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller),
            "review" => new(true, true, true, MarketPersonalsInfoState.WaitingForConfirmPersonalInformations),
            "rejected" => new(true, true, true, MarketPersonalsInfoState.Rejected),
            "inactive" => new(true, true, true, MarketPersonalsInfoState.DisAcctiveMarketAccount),
            "needs-completion" => new(true, true, true, MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller),
            "employee" => throw new InvalidOperationException("Employee dashboard queried owner-only progress"),
            _ => new(true, true, true, MarketPersonalsInfoState.ActiveMarketAccount)
        }),
        nameof(ISellerService.FillListOfPersonalInfoViewModel) => Task.FromResult(new ListOfPersonalInfoViewModel
        {
            CompanyName = "فروشگاه نمونه", NationalCode = 123456789, PeriodTimesOfWork = "۸ تا ۱۶",
            CountryId = 1, StateId = 2, CityId = 3, Resume = "رزومهٔ نمونه", WorkAddress = "آدرس نمونه",
            MarketPersonalsInfoState = MarketPersonalsInfoState.Rejected, RejectedNote = "اصلاح کنید",
            MarketLinks = fixture == "rejected"
                ? [new AddSellerLinksViewModel { Id = 7, LinkTitle = "وب‌سایت فروشگاه", LinkValue = "https://example.test" }]
                : [],
            MarketWorkSamples = fixture == "rejected"
                ? [new AddSellerWorkSampleViewModel { Id = 9, WorkSampleTitle = "پنجرهٔ دوجدارهٔ نمونه", WorkSampleImage = null }]
                : []
        }),
        nameof(ISellerService.UpdateSellerStateAfterEditPersonalInfo) => TestCounters.RecordReviewTransition(),
        nameof(ISellerService.CheckUserCharge) => Task.FromResult(true),
        nameof(ISellerService.FillInformationOfSellerStateViewModel) => Task.FromResult(new Window.Domain.ViewModels.Seller.PersonalInfo.InformationOfSellerStateViewModel
        {
            MarketPersonalsInfoState = MarketPersonalsInfoState.ActiveMarketAccount
        }),
        nameof(ISellerService.UpdateSellerPersonalInfoFromSellerPanel) => Task.FromResult(fixture == "invalid-profile-image" ? UpdateSellerPersonalInfoResul.ImagesNotFound : UpdateSellerPersonalInfoResul.Success),

        nameof(ISellerService.GetSellersInfosState) => Task.FromResult(fixture switch
        {
            "pending" or "setup-links" or "setup-sample" => "NewUser",
            "review" => "WatingForConfirmInformations",
            "rejected" => "Rejected",
            "inactive" => "DisAcctiveSellerAccount",
            "needs-completion" => "WaitingForCompleteInfoFromSeller",
            _ => "ActiveSellerAccount"
        }),
        nameof(ISellerService.IsSellerMaster) => Task.FromResult(fixture != "employee"),
        nameof(ISellerService.CountOfTodayUserInInquiry) => Task.FromResult(12),
        nameof(ISellerService.CountOfMonthUserInInquiry) => Task.FromResult(34),
        nameof(ISellerService.CountOfYearUserInInquiry) => Task.FromResult(56),
        _ => throw new NotSupportedException(name)
    });
});
builder.Services.AddScoped(provider =>
{
    var fixture = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!.Request.Headers["X-Seller-Fixture"].ToString();
    return FixtureService.Create<IUserService>(name => name == nameof(IUserService.GetUserById)
        ? Task.FromResult(new User { Id = 1, Mobile = "09120000000", ShopName = fixture == "clear" ? "فروشگاه آزمایشی" : "" })
        : throw new NotSupportedException(name));
});
builder.Services.AddSingleton(FixtureService.Create<IPermissionService>(name => name == nameof(IPermissionService.IsUserSeller)
    ? Task.FromResult(true)
    : throw new NotSupportedException(name)));
builder.Services.AddSingleton(FixtureService.Create<IContractService>(name => name == nameof(IContractService.CountOfUserWaitingContractRequest)
    ? Task.FromResult(78)
    : throw new NotSupportedException(name)));
builder.Services.AddSingleton(FixtureService.Create<IProductService>(name => throw new NotSupportedException(name)));
builder.Services.AddSingleton(FixtureService.Create<ITicketService>(name => name == nameof(ITicketService.FilterSiteTicket)
    ? Task.FromResult(new FilterSiteTicketViewModel { Entities = [] })
    : throw new NotSupportedException(name)));

builder.Services.AddSingleton(FixtureService.Create<ISender>((name, args) =>
{
    if (name != nameof(ISender.Send)) throw new NotSupportedException(name);
    return args![0] is AddOrEditSellerChequeInfoQuery
        ? Task.FromResult<SellerChequeInfoSellerSideDTO?>(null)
        : Task.FromResult(true);
}));

builder.Services.AddSingleton(FixtureService.Create<IStateService>(name => name switch
{
    nameof(IStateService.GetAllCountries) => Task.FromResult(new List<Window.Domain.ViewModels.Common.SelectListViewModel> { new() { Id = 1, Title = "ایران" } }),
    nameof(IStateService.GetStateChildren) => Task.FromResult(new List<Window.Domain.ViewModels.Common.SelectListViewModel>
    {
        new() { Id = 2, Title = "استان نمونه" }, new() { Id = 3, Title = "شهر نمونه" }
    }),
    _ => throw new NotSupportedException(name)
}));
builder.Services.AddSingleton(FixtureService.Create<IWalletService>(name => throw new NotSupportedException(name)));
builder.Services.AddSingleton(FixtureService.Create<ISellerPersonalVideoService>(name => name switch
{
    nameof(ISellerPersonalVideoService.FillAddOrEditSellerPersonalVideoDTO) => Task.FromResult<AddOrEditSellerPersonalVideoDTO?>(new AddOrEditSellerPersonalVideoDTO
    {
        Title = "ویدیوی آزمایشی",
        AttachmentFileName = "fixture.mp4"
    }),
    nameof(ISellerPersonalVideoService.EditSellerPersonalVideo) => Task.FromResult(true),
    _ => throw new NotSupportedException(name)
}));

await using var app = builder.Build();
app.UseRequestLocalization();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/__culture", () => string.Join("|", CultureInfo.CurrentCulture.Name, CultureInfo.CurrentUICulture.Name,
    CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetType().Name, 1.5m.ToString("0.0")));
app.MapGet("/__seller-flash", (HttpContext context, ITempDataDictionaryFactory factory) =>
{
    var messages = factory.GetTempData(context);
    messages["success"] = "Fixture success";
    messages["InfoMessage"] = "Fixture information";
    messages["ErrorMessage"] = "Fixture <b>error</b>";
    messages.Save();
    return Results.Redirect("/Seller");
}).RequireAuthorization();
await app.StartAsync();

var baseUrl = new Uri(app.Urls.Single());
using var client = new HttpClient { BaseAddress = baseUrl };
client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
Require(await client.GetStringAsync("/__culture") == "fa-IR|fa-IR|GregorianCalendar|1.5", "Persian default and compatible numeric formatting");
Require(await client.GetStringAsync("/__culture?culture=de-DE") == "fa-IR|fa-IR|GregorianCalendar|1.5", "unsupported culture fallback");
Require(await client.GetStringAsync("/__culture?culture=en-US") == "en-US|en-US|GregorianCalendar|1.5", "explicit query preference");
using var cookieRequest = new HttpRequestMessage(HttpMethod.Get, "/__culture");
cookieRequest.Headers.Add("Cookie", ".AspNetCore.Culture=c=en-US|uic=en-US");
using var cookieResponse = await client.SendAsync(cookieRequest);
Require((await cookieResponse.Content.ReadAsStringAsync()).StartsWith("en-US|en-US|"), "explicit cookie preference");
Require((await client.GetAsync("/Seller")).StatusCode == System.Net.HttpStatusCode.Unauthorized, "anonymous seller route challenge");

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Channel = Environment.GetEnvironmentVariable("SELLER_CHECK_BROWSER_CHANNEL")
});
await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
{
    Locale = "en-US",
    HasTouch = true,
    ExtraHTTPHeaders = new Dictionary<string, string> { ["X-Seller-Fixture"] = "master" }
});
await context.RouteAsync("**/*", route => new Uri(route.Request.Url).Host == baseUrl.Host ? route.ContinueAsync() : route.AbortAsync());
var page = await context.NewPageAsync();
var response = await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
Require(response?.Status == 200, "authenticated dashboard response");
var brand = page.Locator(".seller-brand");
var brandLogo = brand.Locator("img");
await Assertions.Expect(brand).ToHaveAttributeAsync("href", "/Seller");
Require(await brandLogo.EvaluateAsync<bool>("logo => logo.complete && logo.naturalWidth === 144 && new URL(logo.src).pathname === '/Site/Manifest/logo144.png'"),
    "seller header loads the original Panjerehyab logo");
await Assertions.Expect(page.Locator("html")).ToHaveAttributeAsync("lang", "fa-IR");
await Assertions.Expect(page.Locator("html")).ToHaveAttributeAsync("dir", "rtl");
await Assertions.Expect(page.Locator(".seller-page-heading h1")).ToHaveTextAsync("داشبورد فروشنده");
var storeLink = page.Locator(".seller-page-heading" ).GetByRole(AriaRole.Link, new() { Name = "رفتن به فروشگاه", Exact = true });
await Assertions.Expect(storeLink).ToHaveAttributeAsync("href", "/" );
Require((await page.Locator(".seller-metric dd").AllTextContentsAsync()).SequenceEqual(["12", "34", "56", "78"]), "dashboard counts");
await Assertions.Expect(page.Locator(".seller-notice a")).ToHaveAttributeAsync("href", "/Seller/Account/EditProfile");
await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/ShopProduct/CreateProduct']")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/ShopOrder/ManageOrderPage']")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/User/AddNewUser']")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator(".seller-shortcuts a").First).ToHaveClassAsync("seller-shortcut seller-store-shortcut");
await Assertions.Expect(page.Locator(".seller-shortcuts .seller-store-badge")).ToHaveTextAsync("فعال");
Require(await page.Locator(".seller-metric").EvaluateAllAsync<bool>("""
    async cards => {
        const backgrounds = cards.map(card => getComputedStyle(card).backgroundImage);
        const illustrations = backgrounds.map(background => background.match(/url\("([^"]+)"\)/)?.[1]);
        if (new Set(backgrounds).size !== 4 || illustrations.some(url => !url)) return false;
        await Promise.all([...new Set(illustrations)].map(async url => {
            const image = new Image();
            image.src = url;
            await image.decode();
        }));
        return new Set(illustrations).size >= 2;
    }
    """), "activity cards have distinct tinted backgrounds and load their illustrations");
var headerStoreStatus = page.Locator("#seller-store-status-button");
await Assertions.Expect(headerStoreStatus).ToHaveAttributeAsync("aria-label", new System.Text.RegularExpressions.Regex("فعال"));
await Assertions.Expect(headerStoreStatus).ToHaveAttributeAsync("data-status", "active");
Require(await headerStoreStatus.EvaluateAsync<bool>("button => getComputedStyle(button).backdropFilter === 'blur(10px)' && getComputedStyle(button).backgroundImage.includes('rgba')"),
    "active status uses a translucent glass background");
await Assertions.Expect(page.Locator(".seller-store-progress")).ToHaveCountAsync(0);

foreach (var width in new[] { 320, 390, 767, 768, 1024, 1440 })
{
    await page.SetViewportSizeAsync(width, 720);
    await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToBeVisibleAsync();
    await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToHaveTextAsync("فعال");
    var statusIcon = await headerStoreStatus.Locator(".seller-store-status-icon").BoundingBoxAsync();
    var statusLabel = await headerStoreStatus.Locator(".seller-store-status-label").BoundingBoxAsync();
    Require(statusIcon is not null && statusLabel is not null
        && statusLabel.X + statusLabel.Width <= statusIcon.X
        && Math.Abs(statusIcon.Y + statusIcon.Height / 2 - statusLabel.Y - statusLabel.Height / 2) < 1,
        $"Persian store status label sits beside the icon at {width}px");
    var logoBox = await brandLogo.BoundingBoxAsync();
    Require(logoBox is { X: >= 0 } && logoBox.X + logoBox.Width <= width
        && Math.Abs(logoBox.Width - logoBox.Height) < 1, $"seller logo fits without distortion at {width}px");
    Require(await page.EvaluateAsync<bool>("document.documentElement.scrollWidth <= window.innerWidth"), $"no horizontal overflow at {width}px");
    Require(await page.EvaluateAsync<bool>("""
        () => {
            const style = selector => getComputedStyle(document.querySelector(selector));
            const size = selector => parseFloat(style(selector).fontSize);
            const weight = selector => Number(style(selector).fontWeight);
            return size('.seller-dashboard h2') > size('.seller-store-card-copy strong')
                && size('.seller-store-card-copy strong') > size('.seller-store-card-copy small')
                && weight('.seller-store-card-copy strong') > weight('.seller-store-card-copy small')
                && size('.seller-metric dd') > size('.seller-metric dt')
                && weight('.seller-metric dd') > weight('.seller-metric dt')
                && weight('.seller-navigation-inner > a[aria-current]') > weight('.seller-navigation-inner > a:not([aria-current])');
        }
        """), $"seller dashboard typography hierarchy at {width}px");
    var grid = page.Locator(".seller-shortcuts");
    var columns = await grid.EvaluateAsync<int>("element => getComputedStyle(element).gridTemplateColumns.split(' ').length");
    Require(columns == (width < 768 ? 2 : 4), $"responsive shortcut columns at {width}px");
    var card = grid.Locator("a").First;
    var box = await card.BoundingBoxAsync();
    Require(box is { Height: >= 44, Width: >= 44 }, $"touch target at {width}px");

    var nav = page.Locator(".seller-navigation");
    if (width < 768)
    {
        await Assertions.Expect(nav).ToBeVisibleAsync();
        await Assertions.Expect(nav).ToHaveCSSAsync("position", "fixed");
        await Assertions.Expect(nav.Locator("[aria-current=page]")).ToHaveCountAsync(1);
        await page.EvaluateAsync("document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight");
        var navBox = await nav.BoundingBoxAsync();
        var noticeBox = await page.Locator(".seller-shortcut").Last.BoundingBoxAsync();
        Require(navBox != null && noticeBox != null && noticeBox.Y + noticeBox.Height < navBox.Y,
            $"last dashboard content remains above navigation at {width}px (notice y={noticeBox?.Y}, height={noticeBox?.Height}, nav y={navBox?.Y})");
    }
    else
    {
        await Assertions.Expect(nav).ToBeVisibleAsync();
        await Assertions.Expect(nav).ToHaveCSSAsync("position", "static");
        await Assertions.Expect(page.Locator(".seller-account-name")).ToBeVisibleAsync();
    }
}

await page.SetViewportSizeAsync(320, 720);
await page.EvaluateAsync("document.scrollingElement.scrollTop = 0");
var navigation = page.GetByRole(AriaRole.Navigation, new() { Name = "ناوبری پنل فروشنده" });
await Assertions.Expect(navigation.GetByRole(AriaRole.Link)).ToHaveCountAsync(3);
await navigation.GetByRole(AriaRole.Link, new() { Name = "سفارش‌ها", Exact = true }).TapAsync();
await Assertions.Expect(page).ToHaveURLAsync(new Uri(baseUrl, "/Seller/ShopOrder/ManageOrderPage").AbsoluteUri);
await Assertions.Expect(navigation.GetByRole(AriaRole.Link, new() { Name = "سفارش‌ها" })).ToHaveAttributeAsync("aria-current", "page");
await Assertions.Expect(navigation).ToHaveCSSAsync("position", "fixed");
await navigation.GetByRole(AriaRole.Link, new() { Name = "پشتیبانی", Exact = true }).TapAsync();
await Assertions.Expect(page).ToHaveURLAsync(new Uri(baseUrl, "/Seller/Ticket/FilterTickets").AbsoluteUri);
await Assertions.Expect(navigation.GetByRole(AriaRole.Link, new() { Name = "پشتیبانی" })).ToHaveAttributeAsync("aria-current", "page");
var ticketResponse = await page.GotoAsync(new Uri(baseUrl, "/Seller/Ticket/CreateTicket").AbsoluteUri);
Require(ticketResponse?.Status == 200, "create ticket route");
await Assertions.Expect(navigation.GetByRole(AriaRole.Link, new() { Name = "پشتیبانی" })).ToHaveAttributeAsync("aria-current", "true");
await navigation.GetByRole(AriaRole.Link, new() { Name = "داشبورد", Exact = true }).TapAsync();
await Assertions.Expect(page.Locator(".seller-page-heading h1")).ToBeVisibleAsync();

var more = navigation.GetByRole(AriaRole.Button, new() { Name = "بیشتر" });
var moreItems = navigation.Locator("#seller-more-items");
await Assertions.Expect(moreItems).ToBeHiddenAsync();
var collapsedNav = await navigation.BoundingBoxAsync();
await more.FocusAsync();
await page.Keyboard.PressAsync("Enter");
await Assertions.Expect(more).ToHaveAttributeAsync("aria-expanded", "true");
await Assertions.Expect(navigation.GetByRole(AriaRole.Link, new() { Name = "محصولات فروشگاه", Exact = true })).ToBeVisibleAsync();
var moreBox = await moreItems.BoundingBoxAsync();
var expandedNav = await navigation.BoundingBoxAsync();
Require(moreBox is { X: >= 0 } && moreBox.X + moreBox.Width <= 320 && expandedNav != null
    && collapsedNav != null && expandedNav.Height > collapsedNav.Height
    && moreBox.Y >= expandedNav.Y && moreBox.Y + moreBox.Height <= expandedNav.Y + expandedNav.Height,
    "additional links expand the fixed navigation in place at 320px");
await Assertions.Expect(moreItems).ToHaveCSSAsync("position", "static");
var dashboardShortcuts = page.Locator(".seller-shortcuts a");
await Assertions.Expect(moreItems.GetByRole(AriaRole.Link)).ToHaveCountAsync(await dashboardShortcuts.CountAsync() - 2);
await page.EvaluateAsync("document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight");
var lastShortcut = await dashboardShortcuts.Last.BoundingBoxAsync();
Require(lastShortcut != null && lastShortcut.Y + lastShortcut.Height < expandedNav!.Y,
    "expanded navigation leaves enough room to reach the last dashboard shortcut");
await page.Keyboard.PressAsync("Escape");
await Assertions.Expect(moreItems).ToBeHiddenAsync();
await Assertions.Expect(more).ToBeFocusedAsync();
await more.TapAsync();
await navigation.GetByRole(AriaRole.Link, new() { Name = "افزودن کارمند", Exact = true }).TapAsync();
await Assertions.Expect(page).ToHaveURLAsync(new Uri(baseUrl, "/Seller/User/AddNewUser").AbsoluteUri);
var pageHeading = page.Locator(".seller-page-heading");
await Assertions.Expect(pageHeading.GetByRole(AriaRole.Button, new() { Name = "بازگشت" })).ToBeVisibleAsync();
await Assertions.Expect(pageHeading.Locator(".seller-page-back .fa-arrow-left" )).ToBeVisibleAsync();
var pageTitleBox = await pageHeading.Locator("h1").BoundingBoxAsync();
var pageBackBox = await pageHeading.Locator("[data-seller-back]").BoundingBoxAsync();
var backLabelBox = await pageHeading.Locator(".seller-page-back span").BoundingBoxAsync();
var backArrowBox = await pageHeading.Locator(".seller-page-back i").BoundingBoxAsync();
Require(backLabelBox is not null && backArrowBox is not null && backArrowBox.X + backArrowBox.Width <= backLabelBox.X,
    "back arrow points left and sits before the label");
Require(pageTitleBox is not null && pageBackBox is not null && pageTitleBox.X > pageBackBox.X,
    "RTL title sits right of the back button in the glass header");
Require(await pageHeading.EvaluateAsync<bool>("element => getComputedStyle(element).backdropFilter.includes('blur')"), "page heading uses glass blur" );
Require(await pageHeading.EvaluateAsync<bool>("element => getComputedStyle(element).backgroundImage.includes('255, 231, 208')"), "glass heading keeps a warm peach tint");
await pageHeading.Locator("[data-seller-back]").ClickAsync();
await Assertions.Expect(page).ToHaveURLAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await page.GotoAsync(new Uri(baseUrl, "/Seller/User/AddNewUser").AbsoluteUri);
var formResponse = await page.ReloadAsync();
await Assertions.Expect(more).ToHaveAttributeAsync("data-section-active", "true");
Require(formResponse?.Status == 200, "employee form response");
await Assertions.Expect(navigation.GetByRole(AriaRole.Link)).ToHaveCountAsync(3);
await Assertions.Expect(navigation.Locator(".seller-navigation-inner > a[aria-current]")).ToHaveCountAsync(0);
await Assertions.Expect(navigation).ToHaveCSSAsync("position", "fixed");
Require(await page.EvaluateAsync<bool>("document.documentElement.scrollWidth <= window.innerWidth"), "employee form fits narrow viewport");
Require(await page.EvaluateAsync<bool>("async () => (await Promise.all([400, 500, 700].map(w => document.fonts.load(w + ' 16px SellerShabnam', 'پنجره')))).every(faces => faces.length > 0)"),
    "local Shabnam files load in all weights");
Require(await page.Locator("#Username").EvaluateAsync<bool>("field => getComputedStyle(field).fontFamily.startsWith('SellerShabnam')"), "form field uses Shabnam");
await Assertions.Expect(page.Locator("#Username")).ToHaveCSSAsync("font-size", "15px");
await Assertions.Expect(page.Locator("label[for=Username]")).ToHaveCSSAsync("font-weight", "500");
await Assertions.Expect(page.Locator(".seller-page-title")).ToHaveCSSAsync("font-weight", "700");
Require(await navigation.Locator(".fa").First.EvaluateAsync<bool>("icon => getComputedStyle(icon).fontFamily.includes('FontAwesome')"), "navigation retains icon font");
await page.EvaluateAsync("document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight");
var footerEdge = await navigation.BoundingBoxAsync();
var lastField = await page.Locator("label[for=NewAvatar]").BoundingBoxAsync();
Require(footerEdge != null && lastField != null && lastField.Y + lastField.Height <= footerEdge.Y,
    "mobile navigation does not cover employee form");
await page.EvaluateAsync("document.scrollingElement.scrollTop = 0");

var account = page.GetByRole(AriaRole.Button, new() { Name = "حساب فروشنده" });
await Assertions.Expect(account.Locator(".seller-avatar")).ToBeVisibleAsync();
Require(await account.Locator(".seller-avatar").EvaluateAsync<bool>("avatar => avatar.complete && avatar.naturalWidth > 0"), "profile image loaded");
await account.FocusAsync();
await page.Keyboard.PressAsync("Enter");
await Assertions.Expect(account).ToHaveAttributeAsync("aria-expanded", "true");
await Assertions.Expect(page.Locator(".seller-account a[href='/Seller/Account/EditProfile']")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator(".seller-account a[href='/Logout']")).ToBeVisibleAsync();
await page.Keyboard.PressAsync("Escape");
await Assertions.Expect(account).ToHaveAttributeAsync("aria-expanded", "false");

var language = page.GetByRole(AriaRole.Button, new() { Name = "زبان پنل" });
await language.ClickAsync();
await Assertions.Expect(page.Locator(".seller-language a[lang='fa-IR']")).ToHaveAttributeAsync("aria-current", "true");
var english = page.Locator(".seller-language a[lang='en-US']");
await Assertions.Expect(english).ToBeVisibleAsync();
Require((await english.GetAttributeAsync("href"))?.Contains("culture=en-US") == true, "English preference action is available");

await page.GotoAsync(new Uri(baseUrl, "/Seller?culture=en-US").AbsoluteUri);
await Assertions.Expect(page.Locator("html")).ToHaveAttributeAsync("dir", "ltr");
await Assertions.Expect(page.Locator(".seller-page-heading h1")).ToHaveTextAsync("Seller dashboard");
foreach (var fixture in new[] { "employee", "pending" })
{
    await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = fixture });
    await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
    await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/ShopProduct/CreateProduct']")).ToHaveCountAsync(0);
    await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/Ticket/FilterTickets']")).ToBeVisibleAsync();
    await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/User'], .seller-shortcuts a[href='/Seller/User/AddNewUser'], #seller-more-items a[href='/Seller/User'], #seller-more-items a[href='/Seller/User/AddNewUser']")).ToHaveCountAsync(0);
    if (fixture == "pending")
    {
        await Assertions.Expect(page.Locator("#seller-overview-title")).ToHaveCountAsync(0);
        await Assertions.Expect(page.Locator(".seller-store-progress progress")).ToHaveAttributeAsync("value", "0");
    }
    else
    {
        await Assertions.Expect(page.Locator(".seller-store-shortcut")).ToHaveCountAsync(0);
        await Assertions.Expect(headerStoreStatus).ToHaveCountAsync(0);
        await Assertions.Expect(page.Locator(".seller-metric")).ToHaveCountAsync(4);
    }
}

foreach (var (fixture, percent, nextAction) in new[]
{
    ("pending", "0", "AddPersonalInfo"),
    ("setup-links", "33", "AddSellerPersonalLinks"),
    ("setup-sample", "67", "AddSellerWorkSampleTitle")
})
{
    await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = fixture });
    await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
    var progressCard = page.Locator(".seller-notice").Filter(new() { Has = page.Locator("progress") });
    await Assertions.Expect(progressCard.Locator("progress")).ToHaveAttributeAsync("value", percent);
    await Assertions.Expect(progressCard.Locator("a")).ToHaveAttributeAsync("href", "/Seller/SellerPersonalInfo/" + nextAction);
    await Assertions.Expect(page.Locator(".seller-shortcuts .seller-store-badge")).ToHaveTextAsync("نیاز به تکمیل");
}

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "inactive" });
await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await Assertions.Expect(headerStoreStatus).ToHaveAttributeAsync("data-status", "inactive");
await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToHaveTextAsync("غیرفعال");
Require(await headerStoreStatus.EvaluateAsync<bool>("button => getComputedStyle(button).backdropFilter === 'blur(10px)' && getComputedStyle(button).backgroundImage.includes('rgba')"),
    "inactive status uses a translucent glass background");
foreach (var (fixture, badge, shortLabel) in new[] { ("review", "در انتظار بررسی", "بررسی"), ("rejected", "نیاز به اصلاح", "اصلاح") })
{
    await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = fixture });
    await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
    await Assertions.Expect(headerStoreStatus).ToHaveAttributeAsync("data-status", fixture == "review" ? "review" : "correction");
    Require(await headerStoreStatus.EvaluateAsync<bool>("button => getComputedStyle(button).backdropFilter === 'blur(10px)' && getComputedStyle(button).backgroundImage.includes('rgba')"),
        "pending/correction status uses a translucent glass background");
    await Assertions.Expect(page.Locator(".seller-shortcuts .seller-store-badge")).ToHaveTextAsync(badge);
    await Assertions.Expect(headerStoreStatus).ToHaveAttributeAsync("aria-label", new System.Text.RegularExpressions.Regex(badge));

    await page.GotoAsync(new Uri(baseUrl, "/Seller/ShopOrder/ManageOrderPage").AbsoluteUri);
    await Assertions.Expect(headerStoreStatus).ToHaveAttributeAsync("aria-label", new System.Text.RegularExpressions.Regex(badge));
    await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToBeVisibleAsync();
    await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToHaveTextAsync(shortLabel);
    await headerStoreStatus.FocusAsync();
    await page.Keyboard.PressAsync("Enter");
    var storeStatusMenu = page.Locator("#seller-store-status-menu");
    await Assertions.Expect(storeStatusMenu).ToBeVisibleAsync();
    await Assertions.Expect(storeStatusMenu.Locator(".seller-store-badge")).ToHaveTextAsync(badge);
    await Assertions.Expect(storeStatusMenu.Locator("a")).ToHaveAttributeAsync("href", "/Seller/SellerPersonalInfo/PageOfManageSellerInformation");
    var menuBox = await storeStatusMenu.BoundingBoxAsync();
    Require(menuBox is { X: >= 0 } && menuBox.X + menuBox.Width <= 320, "store approval menu fits on narrow screens");
}
await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "master" });
await page.GotoAsync(new Uri(baseUrl, "/__seller-flash").AbsoluteUri);
var notices = page.Locator(".seller-notices .seller-notice");
var bell = page.Locator("#seller-notifications-button");
await Assertions.Expect(page.Locator(".seller-dashboard-hero")).ToHaveCountAsync(0);
await Assertions.Expect(page.Locator(".page-header.position-relative")).ToHaveCountAsync(0);
Require(await page.Locator("main > .seller-dashboard > :first-child").GetAttributeAsync("class") == "seller-page-heading",
    "the page title is the first dashboard content");
Require(await page.Locator(".seller-breadcrumb").CountAsync() == 0, "seller pages do not render redundant breadcrumbs");
Require((await notices.EvaluateAllAsync<string[]>("cards => cards.map(card => card.dataset.level)"))
    .SequenceEqual(["warning"]), "operation messages do not become account notices");
var operationNotices = page.Locator(".seller-operation-notices");
await Assertions.Expect(operationNotices).ToContainTextAsync("Fixture <b>error</b>");
await Assertions.Expect(operationNotices.Locator("b")).ToHaveCountAsync(0);
await Assertions.Expect(operationNotices).ToContainTextAsync("Fixture success");
await page.WaitForTimeoutAsync(3500);
await Assertions.Expect(operationNotices).ToBeVisibleAsync();
var operationNoticeBox = await operationNotices.BoundingBoxAsync();
var mobileNavigationBox = await page.Locator(".seller-navigation").BoundingBoxAsync();
Require(operationNoticeBox is { Y: >= 0 } && mobileNavigationBox is not null && operationNoticeBox.Y + operationNoticeBox.Height <= mobileNavigationBox.Y,
    "operation notices remain above the fixed mobile navigation");
await Assertions.Expect(operationNotices).ToHaveCountAsync(0, new() { Timeout = 2500 });
await Assertions.Expect(notices.First.Locator("h3")).ToHaveCSSAsync("font-weight", "700");
await Assertions.Expect(notices.First.Locator("p")).ToHaveCSSAsync("font-size", "15px");
Require((await notices.EvaluateAllAsync<string[]>("cards => cards.map(card => getComputedStyle(card).backgroundColor)"))
    .Distinct().Count() == 1, "the account notice has its severity color");
Require(await notices.EvaluateAllAsync<bool>("""
    cards => {
        const luminance = rgb => rgb.match(/\d+/g).map(channel => Number(channel) / 255)
            .map(channel => channel <= 0.04045 ? channel / 12.92 : ((channel + 0.055) / 1.055) ** 2.4)
            .reduce((value, channel, index) => value + channel * [0.2126, 0.7152, 0.0722][index], 0);
        const gradients = cards.map(card => getComputedStyle(card).backgroundImage);
        return new Set(gradients).size === cards.length && cards.every((card, index) => {
            const stops = gradients[index].match(/rgb\([^)]+\)/g);
            if (!stops?.length) return false;
            const text = luminance(getComputedStyle(card.querySelector('p')).color);
            const backgrounds = [getComputedStyle(card).backgroundColor, ...stops];
            return backgrounds.every(color => {
                const surface = luminance(color);
                return (Math.max(text, surface) + 0.05) / (Math.min(text, surface) + 0.05) >= 4.5;
            });
        });
    }
    """), "each severity has a distinct gradient and readable body copy at its lightest stop");
await Assertions.Expect(bell.Locator(".seller-notification-count")).ToHaveTextAsync("1");
await bell.FocusAsync();
await page.Keyboard.PressAsync("Enter");
await Assertions.Expect(bell).ToHaveAttributeAsync("aria-expanded", "true");
var bellMenu = page.Locator(".seller-notification-list");
await Assertions.Expect(bellMenu.GetByRole(AriaRole.Link)).ToHaveCountAsync(1);
var bellBox = await bellMenu.BoundingBoxAsync();
Require(bellBox is { X: >= 0 } && bellBox.X + bellBox.Width <= 320, "notification menu fits narrow viewport");
await bellMenu.GetByRole(AriaRole.Link).First.TapAsync();
Require(new Uri(page.Url).Fragment == "#seller-notice-0", "notification menu links to the matching card");
await page.ReloadAsync();
await Assertions.Expect(notices).ToHaveCountAsync(1);

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "clear" });
await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await Assertions.Expect(page.Locator(".seller-dashboard-hero")).ToBeVisibleAsync();
Require(await page.EvaluateAsync<bool>("""
    () => {
        const title = getComputedStyle(document.querySelector('.seller-page-title'));
        const section = getComputedStyle(document.querySelector('.seller-dashboard h2'));
        return parseFloat(title.fontSize) > parseFloat(section.fontSize) && title.fontWeight === '700';
    }
    """), "page title leads the dashboard typography");
var heroArt = page.Locator(".seller-dashboard-hero-art");
await Assertions.Expect(heroArt).ToBeVisibleAsync();
Require(await heroArt.EvaluateAsync<bool>("image => image.complete && image.naturalWidth > 0 && new URL(image.src).pathname.endsWith('/seller-dashboard-window.svg')"),
    "dashboard welcome card loads its window illustration");
await page.SetViewportSizeAsync(1024, 720);
var content = page.Locator("main");
var contentBefore = await content.BoundingBoxAsync();
await more.ClickAsync();
await Assertions.Expect(moreItems).ToBeVisibleAsync();
var contentAfter = await content.BoundingBoxAsync();
Require(contentBefore != null && contentAfter != null && contentAfter.Y > contentBefore.Y,
    "expanded desktop navigation makes space above page content");
await more.ClickAsync();
await page.SetViewportSizeAsync(320, 720);
await Assertions.Expect(notices).ToHaveCountAsync(0);
await bell.ClickAsync();
await Assertions.Expect(bellMenu).ToHaveTextAsync("فعلاً اعلانی برای نمایش وجود ندارد.");

await page.GotoAsync(new Uri(baseUrl, "/Seller?OrderFinalization=true").AbsoluteUri);
await Assertions.Expect(notices).ToHaveCountAsync(0);
Require(await page.EvaluateAsync<bool>("async () => (await fetch('/Seller/ShopOrder/OrderFinalization?orderId=1')).ok"),
    "order confirmation endpoint responds with fixture success");
await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await Assertions.Expect(notices).ToHaveCountAsync(0);
await Assertions.Expect(operationNotices).ToContainTextAsync("سفارش با موفقیت تأیید شد");
await page.ReloadAsync();
await Assertions.Expect(notices).ToHaveCountAsync(0);


var profilePath = new Uri(baseUrl, "/Seller/SellerPersonalInfo/ListOfPersonalInfo").AbsoluteUri;
await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "rejected" });
Require((await page.GotoAsync(profilePath))?.Status == 200, "owner can open the rejected profile");
await Assertions.Expect(page.Locator("#CompanyName")).ToBeEditableAsync();
await Assertions.Expect(page.Locator(".seller-profile input[type=file]")).ToHaveCountAsync(3);
await Assertions.Expect(page.Locator("#ImageOfNationalCode")).ToBeVisibleAsync();
var workSampleRow = page.Locator(".seller-work-sample-row");
var sellerLinkRow = page.Locator(".seller-link-row");
await Assertions.Expect(workSampleRow).ToHaveCountAsync(1);
await Assertions.Expect(workSampleRow).ToContainTextAsync("پنجرهٔ دوجدارهٔ نمونه");
await Assertions.Expect(page.Locator(".seller-sample-placeholder")).ToBeVisibleAsync();
await Assertions.Expect(sellerLinkRow).ToContainTextAsync("وب‌سایت فروشگاه");
await Assertions.Expect(sellerLinkRow).ToContainTextAsync("https://example.test");
await Assertions.Expect(page.Locator(".seller-related-add")).ToHaveCountAsync(2);
await Assertions.Expect(page.Locator(".seller-table-delete")).ToHaveCountAsync(2);
await page.Locator(".seller-related-add").First.ClickAsync();
await Assertions.Expect(page.Locator("#NormalModal")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator("#NormalModalTitle")).ToHaveTextAsync("افزودن نمونه‌کار");
await Assertions.Expect(page.Locator("#PersonalWorkSampleForm")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator("#WorkSampleTitle")).ToBeEditableAsync();
await Assertions.Expect(page.Locator("#image")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator("#PersonalWorkSampleForm input[name=__RequestVerificationToken]")).ToHaveCountAsync(1);
await page.Locator("#NormalModal .seller-modal-cancel").ClickAsync();
await Assertions.Expect(page.Locator("#NormalModal")).ToBeHiddenAsync();
var standaloneSamplePage = await page.APIRequest.GetAsync(new Uri(baseUrl, "/Seller/SellerPersonalInfo/AddWorkSample").AbsoluteUri);
Require(standaloneSamplePage.Status == 404, "work sample creation has no standalone page");
var profileNotice = page.Locator("main .seller-notices");
Require(await profileNotice.CountAsync() > 0, "profile fixture includes an account notice");
if (await profileNotice.CountAsync() > 0)
{
    var noticeBox = await profileNotice.BoundingBoxAsync();
    var profileFormBox = await page.Locator("form.seller-profile-card").BoundingBoxAsync();
    Require(noticeBox is not null && profileFormBox is not null && profileFormBox.Y - (noticeBox.Y + noticeBox.Height) >= 24,
        "seller profile notice has 24px separation from the form");
}
foreach (var width in new[] { 320, 390, 1440 })
{
    await page.SetViewportSizeAsync(width, 720);
    Require(await page.EvaluateAsync<bool>("document.documentElement.scrollWidth <= window.innerWidth"), $"seller profile fits {width}px");
    var expectedRowDisplay = width <= 640 ? "grid" : "table-row";
    Require(await workSampleRow.EvaluateAsync<string>("row => getComputedStyle(row).display") == expectedRowDisplay, $"sample table responsive layout at {width}px");
    Require(await sellerLinkRow.EvaluateAsync<string>("row => getComputedStyle(row).display") == expectedRowDisplay, $"link table responsive layout at {width}px");
}

await page.Locator("#CompanyName").FillAsync("نام آزمایشی");
await page.Locator("#CityId").SelectOptionAsync("");
await page.EvaluateAsync("document.querySelector('form.seller-profile-card').submit()");
await Assertions.Expect(page.Locator("[data-valmsg-for=CityId]")).ToContainTextAsync("شهر");
await Assertions.Expect(page.Locator("#CompanyName")).ToHaveValueAsync("نام آزمایشی");
await page.Locator("#CityId").SelectOptionAsync("3");
await page.Locator(".seller-profile-save").ClickAsync();
await Assertions.Expect(page).ToHaveURLAsync(profilePath);
await Assertions.Expect(operationNotices).ToContainTextAsync("تغییرات اطلاعات فروشگاه ذخیره شد");

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "employee" });
Require((await page.APIRequest.GetAsync(profilePath, new() { Headers = new Dictionary<string, string> { ["X-Seller-Fixture"] = "employee" } })).Status == 403, "employee cannot open the owner profile");
var updatePath = new Uri(baseUrl, "/Seller/SellerPersonalInfo/UpdateSellerPersonlInfo").AbsoluteUri;
Require((await page.APIRequest.PostAsync(updatePath)).Status == 400, "profile POST requires an antiforgery token");
await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "invalid-profile-image" });
await page.GotoAsync(profilePath);
await page.Locator("#ImageCompanyLogo").SetInputFilesAsync(new FilePayload
{
    Name = "invalid.png",
    MimeType = "image/png",
    Buffer = System.Text.Encoding.UTF8.GetBytes("not an image")
});
await page.Locator(".seller-profile-save").ClickAsync();
await Assertions.Expect(page.Locator("[role=alert].validation-summary-errors")).ToContainTextAsync("JPG یا PNG");

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "needs-completion" });
await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToHaveTextAsync("ناقص");
await page.GotoAsync(new Uri(baseUrl, "/Seller/ShopOrder/ManageOrderPage").AbsoluteUri);
await Assertions.Expect(headerStoreStatus.Locator(".seller-store-status-label")).ToHaveTextAsync("ناقص");
Require(TestCounters.ReviewTransitions == 0, "view-only navigation does not queue store review");

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "active-edit" });
var reviewTransitionBaseline = TestCounters.ReviewTransitions;
await page.GotoAsync(new Uri(baseUrl, "/Seller/SellerPersonalInfo/EditAndCreateVideos").AbsoluteUri);
await page.Locator("#Title").FillAsync("ویدیوی معرفی جدید");
await page.Locator("form button[type=submit]").ClickAsync();
Require(TestCounters.ReviewTransitions == reviewTransitionBaseline + 1, "successful seller video edit queues store review");

await page.GotoAsync(new Uri(baseUrl, "/Seller/SellerChequeInfo/AddOrEditSellerChequeInfo").AbsoluteUri);
await page.Locator("#SellerMaximumDays").FillAsync("300");
await page.Locator("#CountOfCheque").FillAsync("3");
await page.Locator("form button[type=submit]").ClickAsync();
Require(TestCounters.ReviewTransitions == reviewTransitionBaseline + 2, "successful received-cheque policy edit queues store review");
Console.WriteLine("PASS: seller dashboard/UI checks, responsive profile, and store-state transitions for video and cheque edits.");

static void Require(bool condition, string description)
{
    if (!condition) throw new InvalidOperationException("Failed: " + description);
}

public class FixtureService : DispatchProxy
{
    private Func<string, object?[]?, object> _respond = null!;

    public static T Create<T>(Func<string, object> respond) where T : class
        => Create<T>((name, _) => respond(name));

    public static T Create<T>(Func<string, object?[]?, object> respond) where T : class
    {
        var proxy = Create<T, FixtureService>();
        ((FixtureService)(object)proxy)._respond = respond;
        return proxy;
    }

    protected override object Invoke(MethodInfo? targetMethod, object?[]? args)
        => _respond(targetMethod!.Name, args);
}

public static class TestCounters
{
    public static int ReviewTransitions;

    public static Task RecordReviewTransition()
    {
        Interlocked.Increment(ref ReviewTransitions);
        return Task.CompletedTask;
    }
}

public class FixtureAuthentication(
    IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("X-Seller-Fixture"))
            return Task.FromResult(AuthenticateResult.NoResult());

        var principal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")], Scheme.Name));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
    }
}
