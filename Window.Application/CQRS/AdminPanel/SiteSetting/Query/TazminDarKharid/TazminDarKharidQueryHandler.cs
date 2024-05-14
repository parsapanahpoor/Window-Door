
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.TazminDarKharid;

public record TazminDarKharidQueryHandler : IRequestHandler<TazminDarKharidQuery, Domain.Entities.SiteSetting.TazminDarKharid>
{
    #region ctor

    private readonly ISiteSettingService _siteSettingService;

    public TazminDarKharidQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    #endregion

    public async Task<Domain.Entities.SiteSetting.TazminDarKharid?> Handle(TazminDarKharidQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.Show_TazminDarKharid();
    }
}
