using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Market
{
    public class MarketUsers : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong MarketId { get; set; }

        public ulong RoleId { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        public Market Market { get; set; }

        public Role Role { get; set; }

        #endregion
    }
}
