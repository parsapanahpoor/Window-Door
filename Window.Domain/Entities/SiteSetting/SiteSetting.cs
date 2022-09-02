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

        public int ChargeTariffAboutListOfInquiry { get; set; }

        public int ChargeTariffAboutSellerDetail { get; set; }

        public string SMSForDisActiveFromToday { get; set; }

        public string SMSForDisActiveFrom3Day { get; set; }

        public string SMSForDisActiveFrom15Day { get; set; }

        #endregion
    }
}
