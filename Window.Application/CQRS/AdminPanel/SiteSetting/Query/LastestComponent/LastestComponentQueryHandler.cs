using Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetFreeConsultant;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetLastestComponent;

public record LastestComponentQueryHandler : IRequestHandler<LastestComponentQuery, LastestComponent>
{
    private readonly ISiteSettingService _siteSettingService;

    public LastestComponentQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    public async Task<LastestComponent?> Handle(LastestComponentQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.Get_LastestComponent_ById(request.LastestComponentId , cancellationToken);
    }
}
