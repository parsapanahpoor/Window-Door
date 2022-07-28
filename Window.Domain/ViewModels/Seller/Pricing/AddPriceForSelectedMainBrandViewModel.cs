using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Product;

namespace Window.Domain.ViewModels.Seller.Pricing
{
    public class AddPriceForSelectedMainBrandViewModel
    {
        public ulong ProductId { get; set; }

        public ulong MainBrandId { get; set; }

        public int Price { get; set; }

        public ulong SegmentId { get; set; }

        public List<ProductMainBrandPrice?> ProductMainBrandPrices { get; set; }
    }
}
