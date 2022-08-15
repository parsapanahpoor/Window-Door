using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;

namespace Window.Domain.Entities.Inquiry
{
    public class LogInquiryForUser : BaseEntity
    {
        #region properties

        public ulong? CountryId { get; set; }

        public ulong? StateId { get; set; }

        public ulong? CityId { get; set; }

        public ulong? BrandId { get; set; }

        public SellerType? SellerType { get; set; }

        public ProductKind? ProductKind { get; set; }

        public ProductType? ProductType { get; set; }

        public string? UserMAcAddress { get; set; }

        #endregion

        #region relations

        public ICollection<LogInquiryForUserDetail> logInquiryForUserDetails { get; set; }

        #endregion
    }
}
