using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetLastestComponent;

public record LastestComponentQuery : IRequest<LastestComponent>
{
    #region properties

    public ulong LastestComponentId { get; set; }

    #endregion
}
