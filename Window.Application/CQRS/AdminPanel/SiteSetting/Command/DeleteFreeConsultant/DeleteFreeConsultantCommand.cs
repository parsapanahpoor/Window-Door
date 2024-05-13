namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.DeleteFreeConsultant;

public record DeleteFreeConsultantCommand : IRequest<bool>
{
    #region proeprties

    public ulong FreeConsultantId { get; set; }

    #endregion
}
