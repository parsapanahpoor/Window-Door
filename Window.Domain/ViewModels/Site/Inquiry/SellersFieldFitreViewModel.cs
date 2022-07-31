using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Enums.SellerType;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class SellersFieldFitreViewModel
    {
        #region properties

        public ulong? CountryId { get; set; }

        public ulong? StateId { get; set; }

        public ulong? CityId { get; set; }

        public ulong? BrandId { get; set; }

        public SellerType? SellerType { get; set; }

        public ulong UserId { get; set; }

        #endregion
    }
}
