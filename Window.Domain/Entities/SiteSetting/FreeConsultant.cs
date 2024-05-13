using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.SiteSetting;

public sealed class FreeConsultant : BaseEntity
{
    #region properties

    public string Title{ get; set; }

    public string Description { get; set; }

    #endregion
}
