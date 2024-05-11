
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.SiteSetting1;

public record SiteSetting1QueryHandler : IRequestHandler<SiteSetting1Query, Domain.Entities.SiteSetting.SiteSetting1>
{
    #region ctor

    private readonly ISiteSettingService _siteSettingService;

    public SiteSetting1QueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    #endregion

    public async Task<Domain.Entities.SiteSetting.SiteSetting1> Handle(SiteSetting1Query request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.Show_SiteSetting1();
    }
}
