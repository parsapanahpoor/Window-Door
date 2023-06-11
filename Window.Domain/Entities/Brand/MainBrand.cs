using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.Product;
using Window.Domain.Enums.SellerType;

namespace Window.Domain.Entities.Brand
{
    public class MainBrand : BaseEntity
    {
        #region properties

        public bool UPVC { get; set; }

        public bool Alominum { get; set; }

        public bool Yaragh { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string BrandName { get; set; }

        [Display(Name = "لوگوی برند")]
        [MaxLength(200)]
        public string? BrandLogo { get; set; }

        public int Priority { get; set; } = 0;

        public string? Description { get; set; }

        #endregion

        #region relations

        public ICollection<ProductMainBrandPrice> ProductMainBrandPrice { get; set; }
        public ICollection<Product.Product> Products { get; set; }
        public ICollection<LogForBrands> LogForBrands { get; set; }

        #endregion
    }
}
