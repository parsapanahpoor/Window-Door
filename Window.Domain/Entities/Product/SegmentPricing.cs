using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Segment;

namespace Window.Domain.Entities.Product
{
    public class SegmentPricing : BaseEntity
    {
        #region properties

        public ulong SegmentId { get; set; }

        public ulong ProductId { get; set; }

        [Display(Name = "قیمت قطعه")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public int  Price { get; set; }

        #endregion

        #region relation

        public Segment.Segment Segment { get; set; }

        public Product Product { get; set; }

        #endregion
    }
}
