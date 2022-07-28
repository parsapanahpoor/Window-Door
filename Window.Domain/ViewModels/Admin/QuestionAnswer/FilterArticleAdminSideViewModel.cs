using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.QuestionAnswer
{
    public class FilterQuestionAnswerAdminSideViewModel : BasePaging<Entities.QuestionAnswer.QuestionAnswer>
    {
        #region Constructor

        public FilterQuestionAnswerAdminSideViewModel()
        {
            FilterQuestionAnswerAdminSideOrder = FilterQuestionAnswerAdminSideOrder.CreateDate_Des;
            FilterQuestionAnswerAdminSideState = FilterQuestionAnswerAdminSideState.All;
        }

        #endregion

        #region Properties

        public string? Title { get; set; }
        public FilterQuestionAnswerAdminSideOrder FilterQuestionAnswerAdminSideOrder { get; set; }
        public FilterQuestionAnswerAdminSideState FilterQuestionAnswerAdminSideState { get; set; }

        #endregion
    }

    public enum FilterQuestionAnswerAdminSideOrder
    {
        [Display( Name = "تاریخ ثبت نام - نزولی")]
        CreateDate_Des,
        [Display(Name = "تاریخ ثبت نام - صعودی ")]
        CreateDate_Asc
    }

    public enum FilterQuestionAnswerAdminSideState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        IsActive,
        [Display(Name = " غیرفعال ")]
        NotActive
    }
}
