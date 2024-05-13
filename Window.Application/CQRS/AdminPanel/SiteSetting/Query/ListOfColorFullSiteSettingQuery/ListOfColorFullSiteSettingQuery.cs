using Window.Domain.Entities.SiteSetting;
namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfColorFullSiteSettingQuery;

public record ListOfColorFullSiteSettingQuery : IRequest<List<ColorFullSiteSetting>>
{

}
