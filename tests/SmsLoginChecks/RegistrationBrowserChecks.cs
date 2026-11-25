using Microsoft.Playwright;

internal static class RegistrationBrowserChecks
{
    public static async Task RunAsync(string baseUrl, OfflineSms sender)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Channel = Environment.GetEnvironmentVariable("SELLER_CHECK_BROWSER_CHANNEL") ?? "msedge",
            Headless = true
        });
        var screenshotDirectory = Path.GetFullPath("artifacts/screenshots/registration");
        Directory.CreateDirectory(screenshotDirectory);
        foreach (var width in new[] { 360, 768, 1440 })
        {
            await using var context = await browser.NewContextAsync(new() { ViewportSize = new() { Width = width, Height = 900 } });
            var page = await context.NewPageAsync();
            async Task Capture(string name)
            {
                await page.EvaluateAsync("document.fonts.ready");
                if (await page.EvaluateAsync<bool>("document.documentElement.scrollWidth > innerWidth + 1"))
                    throw new InvalidOperationException($"Horizontal overflow: {name} at {width}");
                await page.ScreenshotAsync(new() { Path = Path.Combine(screenshotDirectory, $"{name}-{width}.png"), FullPage = true });
            }
            await page.GotoAsync(baseUrl + "/register");
            await page.Locator("input[value='Seller']").CheckAsync();
            if (!await page.Locator("#MarketName").IsVisibleAsync())
                throw new InvalidOperationException("Seller store input did not appear.");
            await Capture("password-seller");
            await page.Locator("input[value='Customer']").CheckAsync();
            if (await page.Locator("#MarketName").IsVisibleAsync())
                throw new InvalidOperationException("Customer store input should be hidden.");
            await Capture("password-customer");
            await page.GotoAsync(baseUrl + "/login/sms");
            await page.Locator("input[name='Mobile']").FillAsync($"09100008{width % 1000:000}");
            await page.Locator("button[type='submit']").ClickAsync();
            await page.Locator("input[value='Seller']").WaitForAsync();
            await Capture("role");
            await page.Locator("input[value='Seller']").CheckAsync();
            await page.Locator("button[type='submit']").ClickAsync();
            await page.Locator("#marketName").WaitForAsync();
            await Capture("store");
            await page.Locator("#marketName").FillAsync("فروشگاه آزمایشی");
            await page.Locator("button[type='submit']").ClickAsync();
            await page.WaitForURLAsync("**/login/sms/code", new() { WaitUntil = WaitUntilState.DOMContentLoaded });
            await Capture("code");
            await page.Locator("input[name=\"Code\"]").FillAsync("000000");
            await page.Locator("button[type=\"submit\"]").First.ClickAsync();
            await Capture("code-error");
            await page.GotoAsync(baseUrl + "/Active-mobile/09100000302");
            await Capture("activation");
            var resumePhone = $"09100009{width % 1000:000}";
            await page.GotoAsync(baseUrl + "/login/sms");
            await page.Locator("input[name='Mobile']").FillAsync(resumePhone);
            await page.Locator("button[type='submit']").ClickAsync();
            await page.Locator("input[value='Seller']").CheckAsync();
            await page.Locator("button[type='submit']").ClickAsync();
            await page.Locator("#marketName").WaitForAsync();
            await using var resumedContext = await browser.NewContextAsync(new() { ViewportSize = new() { Width = width, Height = 900 } });
            page = await resumedContext.NewPageAsync();
            await page.GotoAsync(baseUrl + "/login/sms");
            await page.Locator("input[name='Mobile']").FillAsync(resumePhone);
            await page.Locator("button[type='submit']").ClickAsync();
            await page.WaitForURLAsync("**/login/sms/code", new() { WaitUntil = WaitUntilState.DOMContentLoaded });
            await page.Locator("#Code").FillAsync(sender.Code(resumePhone));
            await page.Locator("button[type='submit']").First.ClickAsync();
            await page.WaitForURLAsync("**/register/store", new() { WaitUntil = WaitUntilState.DOMContentLoaded });
            await Capture("resume-store");
        }
        Console.WriteLine("PASS: Edge registration pages at 360/768/1440, role controls and no horizontal overflow.");
    }
}
