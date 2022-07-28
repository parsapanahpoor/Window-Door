using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class FilterInquiryViewModel : BasePaging<InquiryViewModel>
    {
        #region properties

        [Display(Name = "کشور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong? CountryId { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong? StateId { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong? CityId { get; set; }

        public ProductType? ProductType { get; set; }

        public ProductKind? ProductKind { get; set; }

        public SellerType? SellerType { get; set; }

        public ulong? MainBrandId { get; set; }

        #endregion
    }
}
