using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Sample
{
    public class SampleSelectedSegment : BaseEntity
    {
        #region properties

        public Segment.Segment SegmentId { get; set; }

        public Sample SampleId { get; set; }

        #endregion

        #region realtions

        public Segment.Segment Segment { get; set; }

        public Sample Sample { get; set; }

        #endregion
    }
}
