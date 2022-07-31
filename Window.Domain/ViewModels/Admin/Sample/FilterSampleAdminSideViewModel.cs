using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.Sample
{
    public class FilterSampleAdminSideViewModel : BasePaging<Domain.Entities.Sample.Sample>
    {
        #region properties

        public string? SampleName { get; set; }

        #endregion
    }
}
