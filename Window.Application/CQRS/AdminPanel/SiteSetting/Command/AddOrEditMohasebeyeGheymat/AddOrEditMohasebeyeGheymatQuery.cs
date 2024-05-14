namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditMohasebeyeGheymat;

public record AddOrEditMohasebeyeGheymatQuery : IRequest<bool>
{
    #region proeprties

    public string Description { get; set; }

    #endregion
}
