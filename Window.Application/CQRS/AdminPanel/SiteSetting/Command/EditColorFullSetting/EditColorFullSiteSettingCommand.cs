using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditColorFullSetting;

public record EditColorFullSiteSettingCommand : IRequest<bool>
{
    #region proeprties

    public ColorFullSiteSetting ColorFullSiteSetting { get; set; }

    #endregion
}
