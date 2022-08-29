using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Inquiry;

namespace Window.Domain.Entities.Sample
{
    public class Sample : BaseEntity
    {
        #region properties

        public int MaxHeight { get; set; }

        public int MinHeight { get; set; }

        public int MaxWidth { get; set; }

        public int MinWidth { get; set; }

        public bool UPVC { get; set; }

        public bool Aluminum { get; set; }

        public bool Keshoie { get; set; }

        public bool Lolaie { get; set; }

        public bool Door { get; set; }

        public bool Window { get; set; }

        [Display(Name = "نام نمونه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string SegmentName { get; set; }

        [Display(Name = "لوگوی نمونه")]
        [MaxLength(200)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Image { get; set; }

        #endregion

        #region relations

        public ICollection<SampleSelectedSegment> SampleSelectedSegment { get; set; }

        public ICollection<LogInquiryForUserDetail> logInquiryForUserDetails { get; set; }

        #endregion
    }
}
