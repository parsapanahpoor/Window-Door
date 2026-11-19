using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Window.Web.Controllers;
var home = new HomeController(null!, null!, null!, null!, null!, null!, null!);
var result = (ViewResult)await home.Index(CancellationToken.None);
if (result.Model != null || result.ViewName != null) throw new Exception("Root must select standalone Index without a catalog model.");
var root = Directory.GetCurrentDirectory();
var page = File.ReadAllText(Path.Combine(root, "Window.Web/Views/Home/Index.cshtml"));
if (!page.Contains("Layout = null") || !page.Contains("مرجع قیمت گذاری درب و پنجره در ایران") || !page.Contains("asp-action=\"ShopLanding\"")) throw new Exception("Landing content or shop link missing.");
for (var i = 1; i <= 7; i++)
    if (!page.Contains($"/Landing/images/{i}.jpg") || !File.Exists(Path.Combine(root, $"Window.Web/wwwroot/Landing/images/{i}.jpg"))) throw new Exception($"Tile {i} missing.");
foreach (Match asset in Regex.Matches(page, "(?:src|href)=\"(/Landing/[^\"]+)\""))
    if (!File.Exists(Path.Combine(root, "Window.Web/wwwroot", asset.Groups[1].Value.TrimStart('/')))) throw new Exception("Missing asset: " + asset.Groups[1].Value);
var shop = File.ReadAllText(Path.Combine(root, "Window.Web/Views/Shop/ShopLanding.cshtml"));
foreach (var name in new[] { "_LastestProducts", "_LastestSellers", "_PopularBrands" })
    if (!shop.Contains($"~/Views/Home/{name}.cshtml") || !File.Exists(Path.Combine(root, $"Window.Web/Views/Home/{name}.cshtml"))) throw new Exception("Shop partial missing: " + name);
Console.WriteLine("PASS: database-free home action, historical content, seven tiles, Landing assets and shop partial paths.");
