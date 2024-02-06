using System.ComponentModel.DataAnnotations;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.Product;

namespace Window.Domain.Entities.Brand;

public sealed class MainBrand : BaseEntity
{
    #region properties

    public ulong BrandCategorId { get; set; }

    public bool UPVC { get; set; }

    public bool Alominum { get; set; }

    public bool Yaragh { get; set; }

    public bool ShowInSiteMenue { get; set; }

    [Display(Name = "نام برند")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200)]
    public string BrandName { get; set; }

    [Display(Name = "لوگوی برند")]
    [MaxLength(200)]
    public string? BrandLogo { get; set; }

    public int Priority { get; set; } = 0;

    public string? Description { get; set; }

    public string? BrandSite { get; set; }

    #endregion

    #region relations

    public ICollection<ProductMainBrandPrice> ProductMainBrandPrice { get; set; }
    public ICollection<Product.Product> Products { get; set; }
    public ICollection<LogForBrands> LogForBrands { get; set; }

    #endregion
}
