using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Window.Domain.Enums.SellerType;
using static Microsoft.Playwright.Assertions;

internal static class InquiryBrowserChecks
{
    public static async Task RunAsync(Uri origin, ulong state, ulong city, ulong glass)
    {
        var channel = Environment.GetEnvironmentVariable("INQUIRY_CHECK_BROWSER");
        if (string.IsNullOrWhiteSpace(channel)) return;

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = channel, Headless = true });
        foreach (var width in new[] { 390, 1366 })
        {
            await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = width, Height = 844 }
            });
            var page = await context.NewPageAsync();
            page.SetDefaultTimeout(12000);
            await page.GotoAsync(new Uri(origin, "/InquiryProduct/InquiryStep1").ToString());
            await page.WaitForFunctionAsync("navigator.serviceWorker.getRegistration().then(registration => !!registration?.active)");
            var manifestUrl = await page.Locator("link[rel='manifest']").First.GetAttributeAsync("href");
            if (string.IsNullOrEmpty(manifestUrl)) throw new InvalidOperationException("Missing PWA manifest.");
            var manifest = await page.APIRequest.GetAsync(new Uri(origin, manifestUrl).ToString());
            if (!manifest.Ok) throw new InvalidOperationException("PWA manifest failed to load.");
            using var manifestJson = System.Text.Json.JsonDocument.Parse(await manifest.TextAsync());
            foreach (var icon in manifestJson.RootElement.GetProperty("icons").EnumerateArray())
            {
                var iconUrl = new Uri(new Uri(origin, manifestUrl), icon.GetProperty("src").GetString()!);
                var iconResponse = await page.APIRequest.GetAsync(iconUrl.ToString());
                if (!iconResponse.Ok) throw new InvalidOperationException("PWA icon failed to load: " + iconUrl.AbsolutePath);
            }
            await page.Locator("#StateId").SelectOptionAsync(state.ToString(CultureInfo.InvariantCulture));
            await Expect(page.Locator($"#CityId option[value='{city}']")).ToHaveCountAsync(1);
            await page.Locator("#CityId").SelectOptionAsync(city.ToString(CultureInfo.InvariantCulture));
            await page.Locator("#GlassId").SelectOptionAsync(glass.ToString(CultureInfo.InvariantCulture));
            await page.Locator("#SellerType").SelectOptionAsync(((int)SellerType.Aluminium).ToString(CultureInfo.InvariantCulture));
            await page.Locator("form[action$='InquiryStep2'] button[type='submit']").ClickAsync();
            await page.Locator("form[action$='SelectInquiryBrand'] button").First.ClickAsync();

            var form = page.Locator("form").Filter(new LocatorFilterOptions { Has = page.Locator("input[name='SampleId'][value='10']") });
            var widthInput = form.Locator("input[name='Width']");
            await widthInput.FillAsync("۱۰۰");
            await Expect(widthInput).ToHaveValueAsync("100");
            await form.Locator("input[name='Height']").FillAsync("۱۰۰");
            await form.Locator("input[name='SampleCount']").FillAsync("۱");
            await widthInput.FillAsync("۹۹۹");
            await form.Locator("button[type='submit']").ClickAsync();
            await Expect(widthInput).ToHaveValueAsync("999");
            await widthInput.FillAsync("۱۰۰");
            await form.Locator("button[type='submit']").ClickAsync();
            await page.Locator("form[action$='CompleteInquiry'] button").ClickAsync();
            await Expect(page).ToHaveURLAsync(new Regex("InquiryStep4"));
            await Expect(page.GetByText("800 ریال", new PageGetByTextOptions { Exact = true })).ToBeVisibleAsync();
        }

        Console.WriteLine("PASS: Edge desktop and mobile inquiry flow, dependent city selection, Persian digits, complete quote and reference amount.");
    }
}
