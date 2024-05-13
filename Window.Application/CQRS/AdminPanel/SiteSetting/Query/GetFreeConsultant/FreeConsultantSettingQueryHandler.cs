using Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetFreeConsultant;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetFreeConsultant;

public record FreeConsultantSettingQueryHandler : IRequestHandler<FreeConsultantSettingQuery, FreeConsultant>
{
    private readonly ISiteSettingService _siteSettingService;

    public FreeConsultantSettingQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    public async Task<FreeConsultant?> Handle(FreeConsultantSettingQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.Get_FreeConsultant_ById(request.FreeConsultantSettingId , cancellationToken);
    }
}
