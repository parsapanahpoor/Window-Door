using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditFreeConsultant;

public record EditFreeConsultantCommand : IRequest<bool>
{
    #region proeprties

    public FreeConsultant FreeConsultant { get; set; }

    #endregion
}
