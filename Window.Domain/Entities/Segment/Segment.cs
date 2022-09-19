using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;

namespace Window.Domain.Entities.Segment
{
    public class Segment : BaseEntity
    {
        #region properties

        public bool UPVC { get; set; }

        public bool Aluminum { get; set; }

        public bool Yaragh { get; set; }

        [Display(Name = "نام قطعه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string SegmentName { get; set; }

        public int Priority { get; set; } = 0;

        #endregion

        #region relations

        public List<Product.ProductMainBrandPrice> ProductMainBrandPrice { get; set; }

        public List<ProductYaraghBrandPrice> ProductYaraghBrandPrice { get; set; }

        public List<SegmentPricing> SegmentPricings { get; set; }

        public ICollection<SampleSelectedSegment> SampleSelectedSegment { get; set; }

        #endregion
    }
}
