namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.CreateFreeConsultant;

public record CreateFreeConsultantCommand : IRequest<bool>
{
    #region properties

    public string Title { get; set; }

    public string Description { get; set; }

    #endregion
}
