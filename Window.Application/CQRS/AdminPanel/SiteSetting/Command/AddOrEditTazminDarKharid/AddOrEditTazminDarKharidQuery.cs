namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditTazminDarKharid;

public record AddOrEditTazminDarKharidQuery : IRequest<bool>
{
    #region proeprties

    public string Description { get; set; }

    #endregion
}
