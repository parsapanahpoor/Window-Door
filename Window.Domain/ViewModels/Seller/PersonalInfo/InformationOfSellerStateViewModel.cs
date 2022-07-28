using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;

namespace Window.Domain.ViewModels.Seller.PersonalInfo
{
    public class InformationOfSellerStateViewModel
    {
        #region properties

        public string? RejectNote { get; set; }

        public MarketPersonalsInfoState MarketPersonalsInfoState { get; set; }

        public int? DayOfCharge { get; set; }

        #endregion
    }
}
