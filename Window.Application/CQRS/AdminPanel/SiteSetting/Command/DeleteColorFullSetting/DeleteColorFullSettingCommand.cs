using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditColorFullSetting;

public record DeleteColorFullSettingCommand : IRequest<bool>
{
    #region proeprties

    public ulong ColorFullId { get; set; }

    #endregion
}
