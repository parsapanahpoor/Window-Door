namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditSiteSetting1;

public record AddOrEditSiteSetting1Query : IRequest<bool>
{
    #region proeprties

    public string Title { get; set; }

    public string Description { get; set; }

    #endregion
}
