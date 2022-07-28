using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Market
{
    public class MarketWorkSamle : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong MarketId { get; set; }

        public string WorkSampleImage { get; set; }

        public string WorkSampleTitle { get; set; }

        #endregion

        #region properties

        public User User { get; set; }

        public Market Market { get; set; }

        #endregion
    }
}
