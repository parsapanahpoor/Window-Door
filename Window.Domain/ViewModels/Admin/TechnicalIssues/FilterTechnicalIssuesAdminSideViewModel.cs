using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.ViewModels.Article;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.TechnicalIssues
{
    public class FilterTechnicalIssuesAdminSideViewModel : BasePaging<Entities.TechnicalIssues.TechnicalIssues>
    {
        #region Constructor

        public FilterTechnicalIssuesAdminSideViewModel()
        {
            filterTechnicalIssuesAdminSideOrder = FilterTechnicalIssuesAdminSideOrder.CreateDate_Des;
        }

        #endregion

        #region Properties

        public string? Title { get; set; }
        public FilterTechnicalIssuesAdminSideOrder filterTechnicalIssuesAdminSideOrder { get; set; }

        #endregion
    }
    public enum FilterTechnicalIssuesAdminSideOrder
    {
        [Display(Name = "تاریخ ثبت نام - نزولی")]
        CreateDate_Des,
        [Display(Name = "تاریخ ثبت نام - صعودی ")]
        CreateDate_Asc
    }
}
