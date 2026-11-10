using Window.Domain.Entities.Market;

namespace Window.Domain.ViewModels.Seller.PersonalInfo;

public sealed record StoreProfileProgress(bool HasProfile, bool HasLinks, bool HasWorkSample, MarketPersonalsInfoState State)
{
    public const int TotalSteps = 3;
    public int CompletedSteps => (HasProfile ? 1 : 0) + (HasLinks ? 1 : 0) + (HasWorkSample ? 1 : 0);
    public int Percent => (int)Math.Round(100d * CompletedSteps / TotalSteps);
}