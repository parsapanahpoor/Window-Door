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

        public ProductType ProductType { get; set; }

        public ProductKind ProductKind { get; set; }

        public SellerType SellerType { get; set; }

        public ulong ProductTypeCategory { get; set; }

        public ulong BrandId { get; set; }

        #endregion
    }
}
