using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Product;

namespace Window.Domain.Entities.Glass
{
    public class Glass : BaseEntity
    {
        #region properties

        [Display(Name = "نام شیشه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string GlassName { get; set; }

        #endregion

        #region relation 

        public ICollection<GlassPricing> GlassPricings { get; set; }

        #endregion
    }
}
