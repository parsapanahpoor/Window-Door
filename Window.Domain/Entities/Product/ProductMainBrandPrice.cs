using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Segment;

namespace Window.Domain.Entities.Product
{
    public class ProductMainBrandPrice : BaseEntity
    {
        #region properties

        public ulong ProductId { get; set; }

        public ulong MainBrandId { get; set; }

        public ulong SegmentId { get; set; }

        public int Price { get; set; }

        #endregion

        #region relations

        public Product Product { get; set; }

        public MainBrand MainBrand { get; set; }

        public Segment.Segment Segment { get; set; }

        #endregion
    }
}
