using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Enums.SellerType;

namespace Window.Domain.ViewModels.Seller.PersonalInfo
{
    public class AddSellerPersonalInfoViewModel
    {
        #region properties

        [Display(Name = "کشور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong CountryId { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong StateId { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong CityId { get; set; }

        [Display(Name = "کد ملی ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public int NationalCode { get; set; }

        [Display(Name = "عکس جواز کسب")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public IFormFile? PhotoOfBusinessLicense { get; set; }

        [Display(Name = " عکس کارت ملی ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public IFormFile? PhotoOfNationalCode { get; set; }

        [Display(Name = " نام شرکت")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string CompanyName { get; set; }

        [Display(Name = "لوگوی شرکت")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public IFormFile? CompanyLogo { get; set; }

        [Display(Name = " رزومه کاری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string Resume { get; set; }

        [Display(Name = " ساعات کاری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string PeriodTimesOfWork { get; set; }

        [Display(Name = "آدرس محل کار ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string WorkAddress { get; set; }

        [Display(Name = "تخصص فروش")]
        public SellerType SellerType { get; set; }

        #endregion
    }

    public enum AddSellerPersonalInfoResul
    {
        Success,
        Faild,
        ImagesNotFound
    }
    public enum UpdateSellerPersonalInfoResul
    {
        Success,
        Faild,
        ImagesNotFound,
        NotFound
    }
}
