using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Segment;

namespace Window.Domain.ViewModels.Seller.Pricing
{
    public class SegmentPRicingEntityViewModel
    {
        #region properties

        public Segment Segment { get; set; }

        [Display(Name = "قیمت قطعه")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public int Price { get; set; }

        #endregion
    }
}
