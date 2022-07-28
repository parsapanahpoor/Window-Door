using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Wallet
{
    public class WalletData:BaseEntity
    {

        
        public string? TrackingCode { get; set; }
        public string? ReferenceCode { get; set; }

        public string? Token { get; set; }

        public GatewayType GatewayType { get; set; }

        [Required]
        public ulong WalletId { get; set; }

        #region Relations

        public Wallet Wallet { get; set; }

        #endregion
    }
}
