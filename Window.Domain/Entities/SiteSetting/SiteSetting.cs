using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.SiteSetting
{
    public class SiteSetting : BaseEntity
    {
        #region properties

        public string AlertForSellerForSeenProfile { get; set; }

        #endregion
    }
}
