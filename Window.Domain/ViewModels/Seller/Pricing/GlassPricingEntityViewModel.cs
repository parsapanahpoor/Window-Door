using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Glass;

namespace Window.Domain.ViewModels.Seller.Pricing
{
    public class GlassPricingEntityViewModel
    {
        #region properties

        public Glass Glass { get; set; }

        [Display(Name = "قیمت شیشه")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public int Price { get; set; }

        #endregion
    }
}
