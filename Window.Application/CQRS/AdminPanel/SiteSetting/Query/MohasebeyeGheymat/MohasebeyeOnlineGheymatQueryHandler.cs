
using Window.Application.CQRS.AdminPanel.SiteSetting.Query.MohasebeyeGheymat;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.MohasebeyeGheymat;

public record MohasebeyeOnlineGheymatQueryHandler : IRequestHandler<MohasebeyeOnlineGheymatQuery, Domain.Entities.SiteSetting.MohasebeyeOnlineGheymat>
{
    #region ctor

    private readonly ISiteSettingService _siteSettingService;

    public MohasebeyeOnlineGheymatQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    #endregion

    public async Task<Domain.Entities.SiteSetting.MohasebeyeOnlineGheymat?> Handle(MohasebeyeOnlineGheymatQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.Show_MohasebeyeOnlineGheymat();
    }
}
