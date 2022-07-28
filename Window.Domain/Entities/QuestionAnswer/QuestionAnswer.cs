using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.QuestionAnswer
{
    public class QuestionAnswer : BaseEntity
    {
        #region Properties

        [Display(Name = "سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Question { get; set; }

        [Display(Name = "جواب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Answer { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        #endregion

        #region Relations

        #endregion
    }
}
