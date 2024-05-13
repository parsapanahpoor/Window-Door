using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfFreeConsultantSiteSettingQuery;

public record ListOfFreeConsultantSiteSettingQueryHandler : IRequestHandler<ListOfFreeConsultantSiteSettingQuery, List<FreeConsultant>>
{
    private readonly ISiteSettingService _siteSettingService;

    public ListOfFreeConsultantSiteSettingQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    public async Task<List<FreeConsultant>?> Handle(ListOfFreeConsultantSiteSettingQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.ListOf_FreeConsultant();
    }
}
