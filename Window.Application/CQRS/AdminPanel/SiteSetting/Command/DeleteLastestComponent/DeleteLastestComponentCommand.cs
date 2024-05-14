using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.DeleteLastestComponent;

public record DeleteLastestComponentCommand : IRequest<bool>
{
    #region proeprties

    public ulong LastestComponentId { get; set; }

    #endregion
}
