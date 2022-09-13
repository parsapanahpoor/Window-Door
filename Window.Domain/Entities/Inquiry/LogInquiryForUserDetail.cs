using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Sample;

namespace Window.Domain.Entities.Inquiry
{
    public class LogInquiryForUserDetail : BaseEntity
    {
        #region properties

        public ulong LogInquiryForUserId { get; set; }

        public ulong? SampleId { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? KatibeSize { get; set; }

        public int CountOfSample { get; set; } = 1;

        #endregion

        #region relations 

        public LogInquiryForUser LogInquiryForUser { get; set; }

        public Sample.Sample Sample { get; set; }

        #endregion
    }
}
