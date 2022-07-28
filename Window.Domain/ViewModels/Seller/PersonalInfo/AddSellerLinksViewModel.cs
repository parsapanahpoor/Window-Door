using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Seller.PersonalInfo
{
    public class AddSellerLinksViewModel
    {
        #region properties

        public ulong? Id { get; set; }
        public ulong? UserId { get; set; }

        [Display(Name = "عنوان لینک")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string LinkTitle { get; set; }

        [Display(Name = "آدرس لینک")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string LinkValue { get; set; }

        #endregion
    }

    public enum AddSellerLinksResult
    {
        Success,
        Faild
    }
}
