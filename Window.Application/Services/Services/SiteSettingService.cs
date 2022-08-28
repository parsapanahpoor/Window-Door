using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.Services.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public SiteSettingService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        public async Task<SiteSetting?> FillSiteSettingModel()
        {
            #region Get Site Setting 

            var siteSetting = await _context.SiteSettings.FirstOrDefaultAsync(p=> !p.IsDelete);

            #endregion

            return siteSetting;
        }

        public async Task<bool> AddOrUpdateSiteSetting(SiteSetting siteSetting)
        {
            #region Get Site Setting 

            var site = await _context.SiteSettings.FirstOrDefaultAsync(p => !p.IsDelete);

            #endregion

            #region Add Site Setting 

            if (site == null)
            {
                await _context.SiteSettings.AddAsync(siteSetting);
                await _context.SaveChangesAsync();
            }

            #endregion

            #region Update Site Setting 

            if (site != null)
            {
                site.AlertForSellerForSeenProfile = siteSetting.AlertForSellerForSeenProfile;

                _context.SiteSettings.Update(site);
                await _context.SaveChangesAsync();
            }

            #endregion

            return true;
        }

        #endregion
    }
}
