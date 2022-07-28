using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Glass;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.Segment
{
    public class FilterGlassesViewModel : BasePaging<Glass>
    {
        #region properties

        public string? GlassName { get; set; }

        #endregion
    }
}
