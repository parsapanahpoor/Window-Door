using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Product
{
    public class GlassPricing : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong GlassId { get; set; }

        [Display(Name = "قیمت شیشه")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public int Price { get; set; }

        #endregion

        #region relation

        public User User { get; set; }

        public Glass.Glass Glass { get; set; }

        #endregion
    }
}
