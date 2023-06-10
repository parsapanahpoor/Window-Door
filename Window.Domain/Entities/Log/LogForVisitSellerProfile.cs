using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Log
{
    public class LogForVisitSellerProfile : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        #endregion
    }
}
