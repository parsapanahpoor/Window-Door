using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Market
{
    public class MarketLinks : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong MarketId { get; set; }

        public string LinkTitle { get; set; }

        public string LinkValue { get; set; }

        #endregion

        #region relation 

        public User User { get; set; }

        public Market Market { get; set; }

        #endregion
    }
}
