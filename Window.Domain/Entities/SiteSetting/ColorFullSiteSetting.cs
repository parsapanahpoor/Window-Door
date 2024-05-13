using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.SiteSetting;

public sealed class ColorFullSiteSetting : BaseEntity
{
    #region properties

    public string CircleClass{ get; set; }

    public string TagClass { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    #endregion
}
