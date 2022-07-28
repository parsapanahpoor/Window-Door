using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;

namespace Window.Domain.ViewModels.Seller.PersonalInfo
{
    public class MarketChargeInfoViewModel
    {
        #region properties

        public ulong marketId { get; set; }

        public int? ChargePrice { get; set; }

        public int? DaysOfCharge { get; set; }

        public MarketPersonalsInfoState MarketPersonalsInfoState { get; set; }

        public string MarketName { get; set; }

        #endregion
    }
}
