using Window.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Entities.Account
{
    public class Role : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string  Title { get; set; }

        [Display(Name = "نام یکتا")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string  RoleUniqueName { get; set; }
        #endregion

        #region Relation

        public ICollection<RolePermission> RolePermissions { get; set; }

        public ICollection<Market.Market> Markets { get; set; }

        #endregion
    }
}
