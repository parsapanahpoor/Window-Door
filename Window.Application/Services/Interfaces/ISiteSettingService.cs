using Window.Domain.Entities.SiteSetting;
namespace Window.Application.Services.Interfaces;

public interface ISiteSettingService
{
    #region Admin Side 

    Task<SiteSetting?> FillSiteSettingModel();

    Task<bool> AddOrUpdateSiteSetting(SiteSetting siteSetting);

    Task<List<AdminMobiles>> ListOfAdminMobiles();

    Task Add_AdminMobiles(AdminMobiles adminMobiles);

    Task<AdminMobiles?> Get_AdminMobile_ById(ulong adminMobileId,
                                             CancellationToken cancellationToken);

    Task<bool> Delete_AdminMobile(ulong adminMobileId,
                                  CancellationToken cancellationToken);

    Task<List<SalesScale>> ListOfSalesScales();

    Task Add_SaleScale(SalesScale salesScale);

    Task<SalesScale?> Get_SalesScale_ById(ulong saleScaleId,
                                                       CancellationToken cancellationToken);

    Task<bool> Delete_SaleScaleId(ulong saleScaleId,
                                               CancellationToken cancellationToken);

    Task<string?> Get_SaleScaleTitle_ById(ulong scaleId,
                                          CancellationToken cancellation);

    Task<List<string>> GetListOf_AdminsMobiles(CancellationToken cancellation);

    #endregion
}
