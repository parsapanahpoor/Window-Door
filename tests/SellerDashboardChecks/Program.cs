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
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.User.Ticket;
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
        nameof(ISellerService.GetMarketByUserId) => Task.FromResult<Market?>(new Market { Id = 1 }),
        nameof(ISellerService.GetSellersInfosState) => Task.FromResult(fixture == "pending" ? "NewUser" : "ActiveSellerAccount"),
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

builder.Services.AddSingleton(FixtureService.Create<ISender>(name => name == nameof(ISender.Send)
    ? Task.FromResult(true)
    : throw new NotSupportedException(name)));

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
await Assertions.Expect(page.Locator(".seller-dashboard h1")).ToHaveTextAsync("داشبورد فروشنده");
Require((await page.Locator(".seller-metric dd").AllTextContentsAsync()).SequenceEqual(["12", "34", "56", "78"]), "dashboard counts");
await Assertions.Expect(page.Locator(".seller-notice a")).ToHaveAttributeAsync("href", "/Seller/Account/EditProfile");
await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/ShopProduct/CreateProduct']")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/ShopOrder/ManageOrderPage']")).ToBeVisibleAsync();
await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/User/AddNewUser']")).ToBeVisibleAsync();

foreach (var width in new[] { 320, 390, 767, 768, 1024, 1440 })
{
    await page.SetViewportSizeAsync(width, 720);
    var logoBox = await brandLogo.BoundingBoxAsync();
    Require(logoBox is { X: >= 0 } && logoBox.X + logoBox.Width <= width
        && Math.Abs(logoBox.Width - logoBox.Height) < 1, $"seller logo fits without distortion at {width}px");
    Require(await page.EvaluateAsync<bool>("document.documentElement.scrollWidth <= window.innerWidth"), $"no horizontal overflow at {width}px");
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
await Assertions.Expect(page.Locator(".seller-dashboard h1")).ToBeVisibleAsync();

var more = navigation.GetByRole(AriaRole.Button, new() { Name = "بیشتر" });
await more.FocusAsync();
await page.Keyboard.PressAsync("Enter");
await Assertions.Expect(more).ToHaveAttributeAsync("aria-expanded", "true");
await Assertions.Expect(navigation.GetByRole(AriaRole.Link, new() { Name = "محصولات فروشگاه", Exact = true })).ToBeVisibleAsync();
var moreBox = await navigation.Locator(".seller-more-menu").BoundingBoxAsync();
Require(moreBox is { X: >= 0 } && moreBox.X + moreBox.Width <= 320, "more menu fits narrow viewport");
await navigation.GetByRole(AriaRole.Link, new() { Name = "افزودن کارمند", Exact = true }).TapAsync();
await Assertions.Expect(page).ToHaveURLAsync(new Uri(baseUrl, "/Seller/User/AddNewUser").AbsoluteUri);
var formResponse = await page.ReloadAsync();
await Assertions.Expect(more).ToHaveAttributeAsync("data-section-active", "true");
Require(formResponse?.Status == 200, "employee form response");
await Assertions.Expect(navigation.GetByRole(AriaRole.Link)).ToHaveCountAsync(3);
await Assertions.Expect(navigation.Locator(".seller-navigation-inner > a[aria-current]")).ToHaveCountAsync(0);
await Assertions.Expect(navigation).ToHaveCSSAsync("position", "fixed");
Require(await page.EvaluateAsync<bool>("document.documentElement.scrollWidth <= window.innerWidth"), "employee form fits narrow viewport");
Require(await page.EvaluateAsync<bool>("async () => (await Promise.all([400, 500, 700].map(w => document.fonts.load(w + ' 16px SellerVazir', 'پنجره')))).every(faces => faces.length > 0)"),
    "local Vazir files load in all weights");
Require(await page.Locator("#Username").EvaluateAsync<bool>("field => getComputedStyle(field).fontFamily.startsWith('SellerVazir')"), "form field uses Vazir");
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
await Assertions.Expect(page.Locator(".seller-dashboard h1")).ToHaveTextAsync("Seller dashboard");
foreach (var fixture in new[] { "employee", "pending" })
{
    await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = fixture });
    await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
    await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/ShopProduct/CreateProduct']")).ToHaveCountAsync(0);
    await Assertions.Expect(page.Locator(".seller-shortcuts a[href='/Seller/Ticket/FilterTickets']")).ToBeVisibleAsync();
    if (fixture == "pending")
        await Assertions.Expect(page.GetByText("اطلاعات فروشگاه را تکمیل کنید", new() { Exact = true }).Last).ToBeVisibleAsync();
}

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "master" });
await page.GotoAsync(new Uri(baseUrl, "/__seller-flash").AbsoluteUri);
var notices = page.Locator(".seller-notices .seller-notice");
var bell = page.Locator("#seller-notifications-button");
await Assertions.Expect(page.Locator(".seller-dashboard-hero")).ToHaveCountAsync(0);
await Assertions.Expect(page.Locator(".page-header.position-relative")).ToHaveCountAsync(0);
Require(await page.Locator("main > .seller-dashboard > :first-child").GetAttributeAsync("class") == "seller-notices",
    "notices are the first dashboard content");
Require((await notices.EvaluateAllAsync<string[]>("cards => cards.map(card => card.dataset.level)"))
    .SequenceEqual(["error", "warning", "info", "success"]), "notices sorted by severity");
await Assertions.Expect(notices.First.Locator("p")).ToHaveTextAsync("Fixture <b>error</b>");
await Assertions.Expect(notices.First.Locator("p b")).ToHaveCountAsync(0);
Require((await notices.EvaluateAllAsync<string[]>("cards => cards.map(card => getComputedStyle(card).backgroundColor)"))
    .Distinct().Count() == 4, "severity colors are distinct");
await Assertions.Expect(bell.Locator(".seller-notification-count")).ToHaveTextAsync("4");
await bell.FocusAsync();
await page.Keyboard.PressAsync("Enter");
await Assertions.Expect(bell).ToHaveAttributeAsync("aria-expanded", "true");
var bellMenu = page.Locator(".seller-notification-list");
await Assertions.Expect(bellMenu.GetByRole(AriaRole.Link)).ToHaveCountAsync(4);
var bellBox = await bellMenu.BoundingBoxAsync();
Require(bellBox is { X: >= 0 } && bellBox.X + bellBox.Width <= 320, "notification menu fits narrow viewport");
await bellMenu.GetByRole(AriaRole.Link).First.TapAsync();
Require(new Uri(page.Url).Fragment == "#seller-notice-0", "notification menu links to the matching card");
await page.ReloadAsync();
await Assertions.Expect(notices).ToHaveCountAsync(1);

await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string> { ["X-Seller-Fixture"] = "clear" });
await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await Assertions.Expect(page.Locator(".seller-dashboard-hero")).ToBeVisibleAsync();
await Assertions.Expect(notices).ToHaveCountAsync(0);
await bell.ClickAsync();
await Assertions.Expect(bellMenu).ToHaveTextAsync("فعلاً اعلانی برای نمایش وجود ندارد.");

await page.GotoAsync(new Uri(baseUrl, "/Seller?OrderFinalization=true").AbsoluteUri);
await Assertions.Expect(notices).ToHaveCountAsync(0);
Require(await page.EvaluateAsync<bool>("async () => (await fetch('/Seller/ShopOrder/OrderFinalization?orderId=1')).ok"),
    "order confirmation endpoint responds with fixture success");
await page.GotoAsync(new Uri(baseUrl, "/Seller").AbsoluteUri);
await Assertions.Expect(notices).ToHaveCountAsync(1);
await Assertions.Expect(notices.Locator("p")).ToContainTextAsync("سفارش با موفقیت تأیید شد");
await page.ReloadAsync();
await Assertions.Expect(notices).ToHaveCountAsync(0);

Console.WriteLine("PASS: Persian default and overrides, seller authentication and role variants, dashboard layout (320–1440px), shared navigation, local fonts, accessible menus and prioritized seller notices.");

static void Require(bool condition, string description)
{
    if (!condition) throw new InvalidOperationException("Failed: " + description);
}

public class FixtureService : DispatchProxy
{
    private Func<string, object> _respond = null!;

    public static T Create<T>(Func<string, object> respond) where T : class
    {
        var proxy = Create<T, FixtureService>();
        ((FixtureService)(object)proxy)._respond = respond;
        return proxy;
    }

    protected override object Invoke(MethodInfo? targetMethod, object?[]? args) => _respond(targetMethod!.Name);
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
