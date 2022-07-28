using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Article
{
    public class FilterArticleAdminSideViewModel : BasePaging<Entities.Article.Article>
    {
        #region Constructor

        public FilterArticleAdminSideViewModel()
        {
            filterArticleAdminSideOrder = FilterArticleAdminSideOrder.CreateDate_Des;
            filterArticleAdminSideState = FilterArticleAdminSideState.All;
        }

        #endregion

        #region Properties

        public string? AuthorName { get; set; }
        public string? Title { get; set; }
        public FilterArticleAdminSideOrder filterArticleAdminSideOrder { get; set; }
        public FilterArticleAdminSideState filterArticleAdminSideState { get; set; }

        #endregion
    }

    public enum FilterArticleAdminSideOrder
    {
        [Display( Name = "تاریخ ثبت نام - نزولی")]
        CreateDate_Des,
        [Display(Name = "تاریخ ثبت نام - صعودی ")]
        CreateDate_Asc
    }

    public enum FilterArticleAdminSideState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        IsActive,
        [Display(Name = " غیرفعال ")]
        NotActive
    }
}
