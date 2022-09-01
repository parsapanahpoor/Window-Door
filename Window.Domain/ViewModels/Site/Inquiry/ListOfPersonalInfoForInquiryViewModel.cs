using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.Entities.Market;
using Window.Domain.Enums.SellerType;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class ListOfPersonalInfoForInquiryViewModel
    {
        #region properties

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

        public List<AddSellerLinksViewModel> MarketLinks { get; set; }

        public List<AddSellerWorkSampleViewModel> MarketWorkSamples { get; set; }

        #endregion
    }
}
