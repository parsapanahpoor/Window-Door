using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Seller.PersonalInfo
{
    public class AddSellerWorkSampleViewModel
    {
        #region properties

        public ulong? Id { get; set; }

        public ulong? UserId { get; set; }

        [Display(Name = "عنوان نمونه کار ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string WorkSampleTitle { get; set; }

        [Display(Name = "تصویر نمونه کار")]
        public string? WorkSampleImage { get; set; }

        #endregion
    }

    public enum AddSellerWorkSampleResult
    {
        Success,
        Faild,
        ImageNotFound
    }
}
