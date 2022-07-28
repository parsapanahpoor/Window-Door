using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Product;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Seller.Product
{
    public class FilterProductSellerSideViewModel : BasePaging<Entities.Product.Product>
    {
        #region properties

        public ulong? UserId { get; set; }

        #endregion
    }
}
