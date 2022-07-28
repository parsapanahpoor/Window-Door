using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Common;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;

namespace Window.Domain.Entities.Product
{
    public class Product : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong CountryId { get; set; }

        public ulong StateId { get; set; }

        public ulong CityId { get; set; }

        public SellerType SellerType { get; set; }

        public ProductType ProductType{ get; set; }

        public ProductKind ProductKind { get; set; }

        public ulong ProductTypeCategoryId  { get; set; }

        public ulong MainBrandId { get; set; }

        #endregion

        #region relations

        public MainBrand MainBrand { get; set; }

        public User User { get; set; }

        public ProductTypeCategory ProductTypeCategory { get; set; }

        public ICollection<ProductMainBrandPrice> ProductMainBrandPrice { get; set; }

        public ICollection<ProductYaraghBrandPrice> ProductYaraghBrandPrice { get; set; }

        public ICollection<SegmentPricing> SegmentPricings { get; set; }

        #endregion
    }
}
