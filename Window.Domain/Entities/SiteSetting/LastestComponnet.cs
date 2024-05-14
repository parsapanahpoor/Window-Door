using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.SiteSetting;

public sealed class LastestComponent : BaseEntity
{
    #region properties

    public string TagClass { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    #endregion
}
