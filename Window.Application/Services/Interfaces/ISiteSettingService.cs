using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.Services.Interfaces
{
    public interface ISiteSettingService
    {
        #region Admin Side 

        Task<SiteSetting?> FillSiteSettingModel();

        Task<bool> AddOrUpdateSiteSetting(SiteSetting siteSetting);

        #endregion
    }
}
