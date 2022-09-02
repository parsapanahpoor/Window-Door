using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Market
{
    public class Market : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public string MarketName { get; set; }

        public MarketPersonalsInfoState MarketPersonalsInfoState { get; set; }

        public int ActivationTariff { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        public MarketPersonalInfo MarketPersonalInfo { get; set; }

        public List<MarketLinks> MarketLinks { get; set; }

        public List<MarketWorkSamle> MarketWorkSamle { get; set; }

        public ICollection<MarketUsers> MarketUsers { get; set; }

        public ICollection<MarketChargeInfo> MarketChargeInfos { get; set; }

        #endregion
    }
}
