using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Seller.Contract
{
    public class FilterContractRequestSellerSideViewModel : BasePaging<Entities.Contract.RequestForContract>
    {
        #region properties

        public ulong SellerUserId { get; set; }

        #endregion
    }
}
