using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.MarketInfo
{
    public class TaAhodeZamaneTahvil : BaseEntity
    {
        #region properties

        public string macAddress { get; set; }

        public ulong UserId { get; set; }

        public int Score { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        #endregion
    }
}
