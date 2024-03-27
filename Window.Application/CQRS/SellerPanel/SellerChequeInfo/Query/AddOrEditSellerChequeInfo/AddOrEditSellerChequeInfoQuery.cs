using Window.Domain.ViewModels.Seller.SellerChequeInfo;

namespace Window.Application.CQRS.SellerPanel.SellerChequeInfo.Query.AddOrEditSellerChequeInfo;

public class AddOrEditSellerChequeInfoQuery : IRequest<SellerChequeInfoSellerSideDTO>
{
    #region properties

    public ulong SellerUserId { get; set; }

    #endregion
}
