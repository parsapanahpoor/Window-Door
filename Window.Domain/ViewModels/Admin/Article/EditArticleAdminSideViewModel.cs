using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Article.Admin
{
    public class EditArticleAdminSideViewModel
    {

        #region Properties

        public ulong Id { get; set; }

        [Display(Name = " نویسنده ی مقاله ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong? AuthorId { get; set; }
        public string? AuthorName { get; set; }

        [Display(Name = "عنوان مقاله")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string Title { get; set; }

        [Display(Name = "توضیح مختصر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500)]
        public string ShortDescription { get; set; }

        [Display(Name = "متن مقاله")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public string imagename { get; set; }

        #endregion

    }

    public enum EditArticleFromAdminPanelResponse
    {
        [Display(Name = "موفق")]
        Success,
        [Display(Name = "ناموفق")]
        Faild,
        [Display(Name = "ناموفق")]
        ImageNotUploaded,
        [Display(Name = " نویسنده یافت نشده است ")]
        AuthorNotFound,
        [Display(Name = "توضیحات کامل وارد نشد  ")]
        DescriptionNotFound,
        [Display(Name = "عکس یافت نشده است ")]
        ImageNotFound,
        [Display(Name = "مقاله ای یافت نشده است ")]
        ArticleNotFound,
    }
}
