using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditLastestComponent;

public record EditLastestComponentCommand : IRequest<bool>
{
    #region proeprties

    public Domain.Entities.SiteSetting.LastestComponent EditLastestComponent { get; set; }

    #endregion
}
