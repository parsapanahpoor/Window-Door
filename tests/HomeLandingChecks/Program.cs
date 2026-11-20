using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Window.Web.Controllers;
var home = new HomeController(null!, null!, null!, null!, null!, null!, null!);
var result = (ViewResult)await home.Index(CancellationToken.None);
if (result.Model != null || result.ViewName != null) throw new Exception("Root must select standalone Index without a catalog model.");
var root = Directory.GetCurrentDirectory();
var page = File.ReadAllText(Path.Combine(root, "Window.Web/Views/Home/Index.cshtml"));
if (!page.Contains("Layout = null") || !page.Contains("مرجع قیمت گذاری درب و پنجره در ایران") || !page.Contains("asp-action=\"ShopLanding\"")) throw new Exception("Landing content or shop link missing.");
if (Regex.Matches(page, "<img ").Count != 4)
    throw new Exception("Home must contain exactly four image cards.");
if (!page.Contains("asp-controller=\"InquiryProduct\" asp-action=\"InquiryStep1\""))
    throw new Exception("Inquiry card destination missing.");
var sellerCard = page.IndexOf("asp-controller=\"Identity\" asp-action=\"Login\"", StringComparison.Ordinal);
var assistantCard = page.IndexOf("landing-card landing-card-disabled", StringComparison.Ordinal);
if (sellerCard < 0 || assistantCard <= sellerCard || !page[sellerCard..assistantCard].Contains("ورود به پنل فروشندگان"))
    throw new Exception("Seller sign-in must link to Login and precede the disabled assistant.");
var assistant = Regex.Match(page, "<div class=\"landing-card landing-card-disabled\".*?</div>", RegexOptions.Singleline).Value;
if (!assistant.Contains("aria-disabled=\"true\"") || !assistant.Contains("دستیار هوشمند پنجره یاب") || assistant.Contains("<a ") || !page.Contains("filter: grayscale(1)"))
    throw new Exception("Assistant must be labeled, grayscale and noninteractive.");
if (page.Contains("fara-zaman.ir") || page.Contains("TermsConditions"))
    throw new Exception("Removed footer links must not appear on the landing.");
foreach (Match asset in Regex.Matches(page, "(?:src|href)=\"(/Landing/[^\"]+)\""))
    if (!File.Exists(Path.Combine(root, "Window.Web/wwwroot", asset.Groups[1].Value.TrimStart('/')))) throw new Exception("Missing asset: " + asset.Groups[1].Value);
var shop = File.ReadAllText(Path.Combine(root, "Window.Web/Views/Shop/ShopLanding.cshtml"));
foreach (var name in new[] { "_LastestProducts", "_LastestSellers", "_PopularBrands" })
    if (!shop.Contains($"~/Views/Home/{name}.cshtml") || !File.Exists(Path.Combine(root, $"Window.Web/Views/Home/{name}.cshtml"))) throw new Exception("Shop partial missing: " + name);
Console.WriteLine("PASS: database-free home action, historical content, four cards and disabled assistant, Landing assets and shop partial paths.");
