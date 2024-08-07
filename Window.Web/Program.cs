using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Window.Application.SiteServices;
using Window.Data.Context;
using Window.Domain.SharedResource;
using Window.IOC.Container;
using Window.Application;

var builder = WebApplication.CreateBuilder(args);

#region Service

#region Localizer

builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

builder.Services.Configure<RequestLocalizationOptions>(
    opts =>
    {
        CultureInfo faIR = CultureInfo.CreateSpecificCulture("fa-IR");
        faIR.DateTimeFormat.Calendar = new GregorianCalendar();
        faIR.NumberFormat.CurrencyDecimalSeparator = ".";
        faIR.NumberFormat.NumberDecimalSeparator = ".";
        faIR.NumberFormat.PercentDecimalSeparator = ".";
        faIR.NumberFormat.NumberGroupSeparator = ",";
        faIR.NumberFormat.CurrencyGroupSeparator = ",";
        faIR.NumberFormat.NegativeSign = "-";

        var supportedCultures = new List<CultureInfo>
        {
             new CultureInfo("en-US"),
             new CultureInfo("en"),
             faIR
        };

        opts.DefaultRequestCulture = new RequestCulture("en-US");
        opts.SupportedCultures = supportedCultures;
        opts.SupportedUICultures = supportedCultures;
    });

#endregion

#region PWA

builder.Services.AddProgressiveWebApp("/Site/Manifest/manifest.json");

#endregion

#region mvc

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, option => { option.ResourcesPath = "Resources"; })
    .AddDataAnnotationsLocalization(o =>
    {
        o.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            return factory.Create(typeof(DataAnnotationResource));
        };
    });

builder.Services.AddResumingFileResult();
builder.Services.AddMvc();

builder.Services.AddControllers();
{
    builder.Services.RegisterApplicationServices();
}


#endregion

builder.Services.AddHttpContextAccessor();

#region Session

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

#endregion

#region Add DBContext

builder.Services.AddDbContext<WindowDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WindowConnection"));
});

#endregion

#region Data Protection

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + "\\wwwroot\\Auth\\"))
    .SetApplicationName("Window")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(30));

#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
// Add Cookie settings
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

#endregion

#region Encoder

builder.Services.AddSingleton<HtmlEncoder>(
    HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));

#endregion

#region Register Services

DependencyContainer.ConfigureDependencies(builder.Services);

#endregion

#region Localizer

builder.Services.AddLocalization(
    opts =>
    {
        opts.ResourcesPath = "Resources";
    }
);

builder.Services.Configure<RequestLocalizationOptions>(
    opts =>
    {


        CultureInfo faIR = CultureInfo.CreateSpecificCulture("fa-IR");
        faIR.DateTimeFormat.Calendar = new GregorianCalendar();
        faIR.NumberFormat.CurrencyDecimalSeparator = ".";
        faIR.NumberFormat.NumberDecimalSeparator = ".";
        faIR.NumberFormat.PercentDecimalSeparator = ".";
        faIR.NumberFormat.NumberGroupSeparator = ",";
        faIR.NumberFormat.CurrencyGroupSeparator = ",";
        faIR.NumberFormat.NegativeSign = "-";

        var supportedCultures = new List<CultureInfo>
        {
             new CultureInfo("en-US"),
             new CultureInfo("en"),
             faIR,
        };

        opts.DefaultRequestCulture = new RequestCulture("en-US");
        opts.SupportedCultures = supportedCultures;
        opts.SupportedUICultures = supportedCultures;
    });

#endregion

#region API Configuration

builder.Services.AddEndpointsApiExplorer();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver(),
};

#endregion

#region cors

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            "ApiCORS",
            builder => builder
                .AllowAnyOrigin()
                .SetIsOriginAllowed(domains => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(43200))
        );
    });

#endregion

#region IP Address

builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
    });

#endregion

#endregion

#region MiddleWares

var app = builder.Build();

app.UseDeveloperExceptionPage();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

#region Localizer

CultureInfo faIR = CultureInfo.CreateSpecificCulture("fa-IR");
faIR.DateTimeFormat.Calendar = new GregorianCalendar();
faIR.NumberFormat.CurrencyDecimalSeparator = ".";
faIR.NumberFormat.NumberDecimalSeparator = ".";
faIR.NumberFormat.NumberGroupSeparator = ",";
faIR.NumberFormat.CurrencyGroupSeparator = ",";
faIR.NumberFormat.PercentDecimalSeparator = ".";
faIR.NumberFormat.NegativeSign = "-";

var supportedCultures = new List<CultureInfo>() { faIR, new CultureInfo("en-US") };

var options = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                }
};

app.UseRequestLocalization(options);

#endregion

SiteCurrentContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("ApiCORS");

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

#endregion
