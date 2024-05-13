using Window.Domain.Entities.SiteSetting;
namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfFreeConsultantSiteSettingQuery;

public record ListOfFreeConsultantSiteSettingQuery : IRequest<List<FreeConsultant>>
{

}
