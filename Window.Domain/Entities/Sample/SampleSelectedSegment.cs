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

        public ulong SegmentId { get; set; }

        public ulong SampleId { get; set; }

        #endregion

        #region realtions

        public Segment.Segment Segment { get; set; }

        public Sample Sample { get; set; }

        #endregion
    }
}
