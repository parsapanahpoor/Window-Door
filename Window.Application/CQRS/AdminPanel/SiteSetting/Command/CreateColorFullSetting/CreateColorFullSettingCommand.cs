namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.CreateColorFullSetting;

public record CreateColorFullSettingCommand : IRequest<bool>
{
    #region properties

    public string CircleClass { get; set; }

    public string TagClass { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    #endregion
}
