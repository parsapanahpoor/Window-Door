using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;

namespace Window.Domain.ViewModels.Admin.PersonalInfo
{
    public class PersonalInfoDetailAdminSideViewModel
    {
        #region properties

        public ulong UserId { get; set; }

        public Domain.Entities.Account.User User { get; set; }

        [Display(Name = "کد ملی ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public int NationalCode { get; set; }

        [Display(Name = "عکس جواز کسب")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string PhotoOfBusinessLicense { get; set; }

        [Display(Name = " عکس کارت ملی ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string PhotoOfNationalCode { get; set; }

        [Display(Name = " نام شرکت")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string CompanyName { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string State { get; set; }

        [Display(Name = "شهرستان")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string City { get; set; }

        [Display(Name = "لوگوی شرکت")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string CompanyLogo { get; set; }

        [Display(Name = " رزومه کاری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string Resume { get; set; }

        [Display(Name = " ساعات کاری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string PeriodTimesOfWork { get; set; }

        [Display(Name = "آدرس محل کار ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string WorkAddress { get; set; }

        public string? RejectedNote { get; set; }

        public MarketPersonalsInfoState MarketPersonalsInfoState { get; set; }

        public List<MarketLinks> MarketLinks { get; set; }

        public List<MarketWorkSamle> MarketWorkSamples { get; set; }

        #endregion
    }
}
