using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetFreeConsultant;

public record FreeConsultantSettingQuery : IRequest<FreeConsultant>
{
    #region properties

    public ulong FreeConsultantSettingId { get; set; }

    #endregion
}
