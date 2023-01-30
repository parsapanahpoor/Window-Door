using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;
using Window.Domain.Enums.RequestForContract;

namespace Window.Domain.Entities.Contract
{
    public class RequestForContract : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong SellerId { get; set; }

        public RequestForContractType RequestForContractType { get; set; }

        public RequestForContractStatus? RequestForContractStatus { get; set; }

        #endregion

        #region relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("SellerId")]
        public User Seller { get; set; }

        #endregion
    }
}
