using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;

namespace Window.Domain.ViewModels.Seller.Product
{
    public class CreateProductSellerSideViewModel
    {
        #region properties

        public SellerType SellerType { get; set; }

        public ulong BrandId { get; set; }

        #endregion
    }
}
