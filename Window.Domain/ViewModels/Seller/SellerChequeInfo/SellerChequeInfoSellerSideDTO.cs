namespace Window.Domain.ViewModels.Seller.SellerChequeInfo;

public record SellerChequeInfoSellerSideDTO
{
    #region properties

    public ulong SellerUserId { get; set; }

    public int CountOfCheque { get; set; }

    public int SellerMaximumDays { get; set; }

    public bool HasLimitation { get; set; }

    #endregion
}
