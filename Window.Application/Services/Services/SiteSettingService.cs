using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.Services.Services;

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

    public async Task<FreeConsultant?> Get_FreeConsultant_ById(ulong consultatntId, CancellationToken cancellation)
    {
        return await _context.FreeConsultants
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.Id == consultatntId);
    }

    public async Task<List<FreeConsultant>?> ListOf_FreeConsultant()
    {
        return await _context.FreeConsultants
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .ToListAsync();
    }

    public void Update_FreeConsultant(FreeConsultant freeConsultant)
    {
        _context.FreeConsultants.Update(freeConsultant);
    }

    public async Task Add_FreeConsultant(FreeConsultant freeConsultant,
                                         CancellationToken cancellationToken = default)
    {
        await _context.FreeConsultants.AddAsync(freeConsultant);
    }

    public async Task<ColorFullSiteSetting?> Get_ColorFullSiteSetting_ById(ulong colorId , CancellationToken cancellation)
    {
        return await _context.ColorFullSiteSetting
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p=> !p.IsDelete && 
                                                  p.Id == colorId);
    }

    public async Task<List<ColorFullSiteSetting>?> ListOf_ColorFullSiteSetting()
    {
        return await _context.ColorFullSiteSetting
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .ToListAsync();
    }

    public async Task<SiteSetting?> FillSiteSettingModel()
    {
        #region Get Site Setting 

        var siteSetting = await _context.SiteSettings.FirstOrDefaultAsync(p=> !p.IsDelete);

        #endregion

        return siteSetting;
    }

    public void Update_ColorFullSiteSetting(ColorFullSiteSetting colorFullSiteSetting)
    {
        _context.ColorFullSiteSetting.Update(colorFullSiteSetting);
    }

    public async Task Add_ColorFullSiteSetting(ColorFullSiteSetting colorFullSiteSetting , 
                                               CancellationToken cancellationToken = default)
    {
        await _context.ColorFullSiteSetting.AddAsync(colorFullSiteSetting);
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
            site.ChargeTariffAboutSellerDetail = siteSetting.ChargeTariffAboutSellerDetail;
            site.ChargeTariffAboutListOfInquiry = siteSetting.ChargeTariffAboutListOfInquiry;
            site.SMSForDisActiveFromToday = siteSetting.SMSForDisActiveFromToday;
            site.SMSForDisActiveFrom3Day = siteSetting.SMSForDisActiveFrom3Day;
            site.SMSForDisActiveFrom15Day = siteSetting.SMSForDisActiveFrom15Day;
            site.ChargeOfNewMarkets = siteSetting.ChargeOfNewMarkets;

            _context.SiteSettings.Update(site);
            await _context.SaveChangesAsync();
        }

        #endregion

        return true;
    }

    public async Task<List<AdminMobiles>> ListOfAdminMobiles()
    {
        return await _context.AdminMobiles
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete)
                             .ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Add_AdminMobiles(AdminMobiles adminMobiles)
    {
        await _context.AdminMobiles.AddAsync(adminMobiles);
        await SaveChanges();
    }

    public async Task<AdminMobiles?> Get_AdminMobile_ById(ulong adminMobileId , 
                                                          CancellationToken cancellationToken)
    {
        return await _context.AdminMobiles
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.Id == adminMobileId);
    }

    public async Task<bool> Delete_AdminMobile(ulong adminMobileId , 
                                               CancellationToken cancellationToken)
    {
        //Get Mobile Admin 
        var mobileAdmin = await Get_AdminMobile_ById(adminMobileId , cancellationToken);
        if (mobileAdmin == null) return false;

        mobileAdmin.IsDelete = true;

        _context.AdminMobiles.Update(mobileAdmin);
        await SaveChanges();

        return true;
    }

    public async Task<List<SalesScale>> ListOfSalesScales()
    {
        return await _context.SalesScales
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete)
                             .ToListAsync();
    }

    public async Task Add_SaleScale(SalesScale salesScale)
    {
        await _context.SalesScales.AddAsync(salesScale);
        await SaveChanges();
    }

    public async Task<SalesScale?> Get_SalesScale_ById(ulong saleScaleId,
                                                       CancellationToken cancellationToken)
    {
        return await _context.SalesScales
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.Id == saleScaleId);
    }

    public async Task<bool> Delete_SaleScaleId(ulong saleScaleId,
                                               CancellationToken cancellationToken)
    {
        //Get Sale Scale 
        var saleScale = await Get_SalesScale_ById(saleScaleId, cancellationToken);
        if (saleScale == null) return false;

        saleScale.IsDelete = true;

        _context.SalesScales.Update(saleScale);
        await SaveChanges();

        return true;
    }

    public async Task<string?> Get_SaleScaleTitle_ById(ulong scaleId , 
                                                      CancellationToken cancellation)
    {
        return await _context.SalesScales
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == scaleId)
                             .Select(p => p.ScaleTitle)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<string>> GetListOf_AdminsMobiles(CancellationToken cancellation)
    {
        return await _context.AdminMobiles
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete)
                             .Select(p=> p.AdminMobile)
                             .ToListAsync();
    }

    #endregion

    #region Landing Componnets

    public async Task<Domain.Entities.SiteSetting.SiteSetting1?> Show_SiteSetting1() 
    {
        return await _context.SiteSetting1.FirstOrDefaultAsync(p => !p.IsDelete);
    }

    public async Task Add_SiteSetting1(SiteSetting1 siteSetting)
    {
        await _context.SiteSetting1.AddAsync(siteSetting);
    }

    public void Edit_SiteSetting(SiteSetting1 siteSetting)
    {
        _context.SiteSetting1.Update(siteSetting);
    }

    #endregion
}
