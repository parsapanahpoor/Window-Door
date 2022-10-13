using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Common;
using static Window.Domain.ViewModels.User.FilterUserViewModel;

namespace Window.Domain.ViewModels.Admin.PersonalInfo
{
    public class ListOfSellersInfoViewModel : BasePaging<MarketPersonalInfo>
    {
        #region properties

        public string? Username { get; set; }

        public string? Mobile { get; set; }

        public string? Email { get; set; }

        public string? NationalCode { get; set; }

        public MarketPersonalsInfoState? MarketPersonalsInfoState { get; set; }

        public ulong? UserId { get; set; }

        [RegularExpression(@"^\d{4}\/(0?[1-9]|1[012])\/(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "The entered date is not valid")]
        public string? FromDate { get; set; }

        [RegularExpression(@"^\d{4}\/(0?[1-9]|1[012])\/(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "The entered date is not valid")]
        public string? ToDate { get; set; }

        [Display(Name = "کشور")]
        public ulong? CountryId { get; set; }

        [Display(Name = "استان")]
        public ulong? StateId { get; set; }

        [Display(Name = "شهر")]
        public ulong? CityId { get; set; }

        public FilterUserOrderType OrderType { get; set; }

        #endregion
    }
}
