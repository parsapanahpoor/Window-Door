using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.PersonalInfo
{
    public class ListOfSellersInfoViewModel : BasePaging<MarketPersonalInfo>
    {
        #region properties

        public string? Username { get; set; }

        public string? Mobile { get; set; }

        public string? Email { get; set; }

        public string? NationalCode { get; set; }

        public MarketPersonalsInfoState? MarketPersonalsInfoState { get; set; }

        public ulong? UserId { get; set; }

        #endregion
    }
}
