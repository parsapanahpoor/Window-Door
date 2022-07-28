using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.Segment
{
    public class FilterSegmentViewModel : BasePaging<Domain.Entities.Segment.Segment>
    {
        #region properties

        public string? SegmentName { get; set; }

        #endregion
    }
}
