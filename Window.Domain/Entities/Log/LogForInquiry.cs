using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;

namespace Window.Domain.Entities.Log
{
    public class LogForInquiry : BaseEntity
    {
        #region properties

        [Display(Name = "نام کشور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong CountryId { get; set; }

        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong StateId { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public ulong CityId { get; set; }

        public SellerType SellerType { get; set; }

        #endregion

        #region ralations 

        [InverseProperty("LogCountries")]
        [ForeignKey("CountryId")]
        public State Country { get; set; }

        [InverseProperty("LogCities")]
        [ForeignKey("CityId")]
        public State City { get; set; }

        [InverseProperty("LogStates")]
        [ForeignKey("StateId")]
        public State State { get; set; }

        #endregion
    }
}
