using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.MarketInfo;

public sealed class SelersPersonalVideos : BaseEntity
{
    #region properties

    public string Title { get; set; }

    public ulong UserId { get; set; }

    public string Videos { get; set; }

    public string BanerImage { get; set; }

    #endregion
}
