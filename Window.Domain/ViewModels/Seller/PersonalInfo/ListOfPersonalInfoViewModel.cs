using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;
using Window.Domain.Enums.SellerType;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Domain.ViewModels.Seller.PersonalInfo
{
    public class ListOfPersonalInfoViewModel
    {
        #region properties

        [Display(Name = "مبلغ شارژ ماه بعد کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ActivationTariff { get; set; }

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
        public string PhotoOfBusinessLicense { get; set; }
        public IFormFile? ImageOfBusinessLicense { get; set; }

        [Display(Name = " عکس کارت ملی ")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string PhotoOfNationalCode { get; set; }
        public IFormFile? ImageOfNationalCode { get; set; }

        [Display(Name = " نام شرکت")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string CompanyName { get; set; }

        [Display(Name = "لوگوی شرکت")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string CompanyLogo { get; set; }
        public IFormFile? ImageCompanyLogo { get; set; }

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

        public List<AddSellerLinksViewModel> MarketLinks { get; set; }

        public List<AddSellerWorkSampleViewModel> MarketWorkSamples { get; set; }

        public List<MarketChargeInfo?> MarketChargeInfos { get; set; }

        public SellerType SellerType { get; set; }

        public AddScoreToTheSellerViewModel Score { get; set; }

        #endregion
    }
}
