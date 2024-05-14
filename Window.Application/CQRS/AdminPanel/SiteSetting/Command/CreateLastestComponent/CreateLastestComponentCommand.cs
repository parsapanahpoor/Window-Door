namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.LastestComponent;

public record CreateLastestComponentCommand : IRequest<bool>
{
    #region properties

    public string TagClass { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    #endregion
}
