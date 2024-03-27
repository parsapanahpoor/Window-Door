namespace Window.Application.CQRS.SellerPanel.SellerChequeInfo.Command.AddOrEditSellerChequeInfo;

public record AddOrEditSellerChqueInfoCommand : IRequest<bool>
{
    #region properties

    public ulong SellerUserId { get; set; }

    public int CountOfCheque { get; set; }

    public int SellerMaximumDays { get; set; }

    public bool HasLimitation { get; set; }

    #endregion
}
