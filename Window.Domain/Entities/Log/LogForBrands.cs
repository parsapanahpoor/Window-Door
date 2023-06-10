using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Log
{
    public class LogForBrands : BaseEntity
    {
        #region properties

        public ulong MainBrandId { get; set; }

        public ulong? CountryId { get; set; }

        public ulong? StateId { get; set; }

        public ulong? CityId { get; set; }

        #endregion

        #region relations 

        public Brand.MainBrand MainBrand { get; set; }

        #endregion
    }
}
