using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfColorFullSiteSettingQuery;

public record ListOfColorFullSiteSettingQueryHandler : IRequestHandler<ListOfColorFullSiteSettingQuery, List<ColorFullSiteSetting>>
{
    private readonly ISiteSettingService _siteSettingService;

    public ListOfColorFullSiteSettingQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    public async Task<List<ColorFullSiteSetting>?> Handle(ListOfColorFullSiteSettingQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.ListOf_ColorFullSiteSetting();
    }
}
