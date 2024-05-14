using Window.Domain.Entities.SiteSetting;
namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.ListOfLastestComponent;

public record ListOfLastestComponentQuery : IRequest<List<LastestComponent>>
{

}
