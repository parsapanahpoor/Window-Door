using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Market;

public sealed class SellerChequeInfo : BaseEntity
{
    #region properties

    public ulong SellerUserId{ get; set; }

    public int CountOfCheque { get; set; }

    public int SellerMaximumDays { get; set; }

    public bool HasLimitation { get; set; }

    #endregion
}
