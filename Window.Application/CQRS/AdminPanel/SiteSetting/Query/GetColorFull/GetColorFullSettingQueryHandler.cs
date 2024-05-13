using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetColorFull;

public record GetColorFullSettingQueryHandler : IRequestHandler<GetColorFullSettingQuery, ColorFullSiteSetting>
{
    private readonly ISiteSettingService _siteSettingService;

    public GetColorFullSettingQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    public async Task<ColorFullSiteSetting?> Handle(GetColorFullSettingQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.Get_ColorFullSiteSetting_ById(request.ColorFullSettingId , cancellationToken);
    }
}
