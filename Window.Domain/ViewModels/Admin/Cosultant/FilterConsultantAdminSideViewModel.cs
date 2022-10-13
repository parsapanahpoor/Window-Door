using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.Entities.Counseling;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.Cosultant
{
    public class FilterConsultantAdminSideViewModel : BasePaging<Counseling>
    {
        #region Constructor

        public FilterConsultantAdminSideViewModel()
        {
            FilterConcultantAdminSideOrder = FilterConcultantAdminSideOrder.CreateDate_Des;
        }

        #endregion

        #region Properties

        public string? Title { get; set; }
        public FilterConcultantAdminSideOrder FilterConcultantAdminSideOrder { get; set; }

        #endregion
    }
    public enum FilterConcultantAdminSideOrder
    {
        [Display(Name = "تاریخ ثبت نام - نزولی")]
        CreateDate_Des,
        [Display(Name = "تاریخ ثبت نام - صعودی ")]
        CreateDate_Asc
    }
}
