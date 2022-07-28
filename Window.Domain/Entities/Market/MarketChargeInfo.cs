using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Market
{
    public class MarketChargeInfo : BaseEntity
    {
        #region properties

        public ulong MarketId { get; set; }

        public ulong UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Price { get; set; }

        public bool CurrentAccountCharge { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        public Market MArket { get; set; }

        #endregion
    }
}
