using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Query.GetColorFull;

public record GetColorFullSettingQuery : IRequest<ColorFullSiteSetting>
{
    #region properties

    public ulong ColorFullSettingId { get; set; }

    #endregion
}
