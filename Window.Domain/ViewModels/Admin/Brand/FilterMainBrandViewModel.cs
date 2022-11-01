using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;
using Window.Domain.Enums.BrandType;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.Brand
{
    public class FilterMainBrandViewModel : BasePaging<MainBrand>
    {
        #region properties

        public string? BrandName { get; set; }

        public BrandType BrandType { get; set; }

        #endregion
    }
}
