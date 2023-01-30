using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Comment
{
    public class Comment : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong SellerId { get; set; }

        public string Description { get; set; }

        #endregion

        #region relation

        public User User { get; set; }

        #endregion
    }
}
