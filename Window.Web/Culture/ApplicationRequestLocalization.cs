using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace Window.Web.Culture;

public static class ApplicationRequestLocalization
{
    public static void Configure(RequestLocalizationOptions options)
    {
        var persian = CultureInfo.CreateSpecificCulture("fa-IR");
        persian.DateTimeFormat.Calendar = new GregorianCalendar();
        persian.NumberFormat.CurrencyDecimalSeparator = ".";
        persian.NumberFormat.NumberDecimalSeparator = ".";
        persian.NumberFormat.PercentDecimalSeparator = ".";
        persian.NumberFormat.NumberGroupSeparator = ",";
        persian.NumberFormat.CurrencyGroupSeparator = ",";
        persian.NumberFormat.NegativeSign = "-";

        CultureInfo[] supported = [persian, new("en-US")];
        options.DefaultRequestCulture = new RequestCulture(persian);
        options.SupportedCultures = supported;
        options.SupportedUICultures = supported;
        options.RequestCultureProviders =
        [
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        ];
    }
}
