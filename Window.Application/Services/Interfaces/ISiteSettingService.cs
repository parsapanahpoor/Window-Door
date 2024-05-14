using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.SiteSetting;
namespace Window.Application.Services.Interfaces;

public interface ISiteSettingService
{
    #region Admin Side 

    Task<LastestComponent?> Get_LastestComponent_ById(ulong lastestComponentId, CancellationToken cancellation);

    Task<List<LastestComponent>?> ListOf_LastestComponent();

    void Update_LastestComponent(LastestComponent lastestComponent);

    Task Add_LastestComponent(LastestComponent lastestComponent,
                              CancellationToken cancellationToken = default);

    Task<FreeConsultant?> Get_FreeConsultant_ById(ulong consultatntId, CancellationToken cancellation);

    Task<List<FreeConsultant>?> ListOf_FreeConsultant();

    void Update_FreeConsultant(FreeConsultant freeConsultant);

    Task Add_FreeConsultant(FreeConsultant freeConsultant,
                            CancellationToken cancellationToken = default);

    Task<ColorFullSiteSetting?> Get_ColorFullSiteSetting_ById(ulong colorId, CancellationToken cancellation);

    void Update_ColorFullSiteSetting(ColorFullSiteSetting colorFullSiteSetting);

    Task Add_ColorFullSiteSetting(ColorFullSiteSetting colorFullSiteSetting,
                                  CancellationToken cancellationToken = default);

    Task<List<ColorFullSiteSetting>?> ListOf_ColorFullSiteSetting();

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

    #region Landing Componnets

    Task<MohasebeyeOnlineGheymat?> Show_MohasebeyeOnlineGheymat();

    Task<TazminDarKharid?> Show_TazminDarKharid();

    Task<SiteSetting1?> Show_SiteSetting1();

    Task Add_SiteSetting1(SiteSetting1 siteSetting);

    void Edit_SiteSetting(SiteSetting1 siteSetting);

    Task Add_TazminDarKharid(TazminDarKharid tazmin);

    void Edit_TazminDarKhrid(TazminDarKharid tazmin);

    Task Add_MohasebeyeGheymat(MohasebeyeOnlineGheymat mohasebeyeOnline);

    void Edit_MohasebeyeGheymat(MohasebeyeOnlineGheymat mohasebeyeOnline);

    #endregion
}
