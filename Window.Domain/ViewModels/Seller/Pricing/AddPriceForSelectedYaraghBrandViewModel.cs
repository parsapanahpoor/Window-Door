using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Product;

namespace Window.Domain.ViewModels.Seller.Pricing
{
    public class AddPriceForSelectedYaraghBrandViewModel
    {
        public ulong ProductId { get; set; }

        public ulong YaraghBrandId { get; set; }

        public int Price { get; set; }

        public ulong SegmentId { get; set; }

        public List<ProductYaraghBrandPrice?> ProductYaraghBrandPrice { get; set; }
    }
}
