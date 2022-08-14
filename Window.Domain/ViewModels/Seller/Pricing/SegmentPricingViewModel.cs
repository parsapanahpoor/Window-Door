using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.Segment;

namespace Window.Domain.ViewModels.Seller.Pricing
{
    public class SegmentPricingViewModel
    {
        #region properties

        public string  ProductName { get; set; }

        public ulong ProductId { get; set; }

        public List<SegmentPRicingEntityViewModel> Segments { get; set; }

        #endregion
    }
}
