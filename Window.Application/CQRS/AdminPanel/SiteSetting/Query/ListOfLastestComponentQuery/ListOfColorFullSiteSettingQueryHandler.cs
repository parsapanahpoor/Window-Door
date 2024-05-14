using Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfLastestComponent;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfLastestComponent;

public record ListOfLastestComponentQueryHandler : IRequestHandler<ListOfLastestComponentQuery, List<LastestComponent>>
{
    private readonly ISiteSettingService _siteSettingService;

    public ListOfLastestComponentQueryHandler(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    public async Task<List<LastestComponent>?> Handle(ListOfLastestComponentQuery request, CancellationToken cancellationToken)
    {
        return await _siteSettingService.ListOf_LastestComponent();
    }
}
