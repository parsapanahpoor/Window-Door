using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.TechnicalIssues
{
    public class TechnicalIssues : BaseEntity
    {
        #region properties


        [Display(Name = "عنوان ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string Title { get; set; }

        [Display(Name = "متن ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        #endregion
    }
}
