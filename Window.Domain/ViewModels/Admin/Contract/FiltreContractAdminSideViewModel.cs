using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.Contract
{
    public class FiltreContractAdminSideViewModel : BasePaging<Domain.Entities.Contract.RequestForContract>
    {
        #region properties

        [Display(Name = "استان")]
        public ulong? StateId { get; set; }

        [Display(Name = "شهر")]
        public ulong? CityId { get; set; }

        [Display(Name = "کشور")]
        public ulong? CountryId { get; set; }

        public string? SellerUsername { get; set; }

        public string? SellerMobile { get; set; }

        public string? CustomerUsername { get; set; }

        public string? CustomerMobile { get; set; }

        #endregion
    }
}
