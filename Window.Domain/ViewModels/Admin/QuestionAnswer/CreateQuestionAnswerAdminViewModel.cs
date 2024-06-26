﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Admin.QuestionAnswer
{
    public class CreateQuestionAnswerAdminViewModel
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
    }

    public enum CreateQuestionAnswerFromAdminPanelResponse
    {
        [Display(Name = "موفق")]
        Success,
        [Display(Name = "ناموفق")]
        Faild
    }
}
