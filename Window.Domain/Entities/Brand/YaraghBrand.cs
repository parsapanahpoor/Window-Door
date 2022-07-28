using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Product;
using Window.Domain.Enums.SellerType;

namespace Window.Domain.Entities.Brand
{
    public class YaraghBrand : BaseEntity
    {
        #region properties

        public SellerType SellerType { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string YaraghBrandName { get; set; }

        [Display(Name = "لوگوی برند")]
        [MaxLength(200)]
        public string? YaraghBrandLogo { get; set; }

        #endregion

        #region relations

        public ICollection<ProductYaraghBrandPrice> ProductYaraghBrandPrice { get; set; }

        #endregion
    }
}
