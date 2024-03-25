using Window.Domain.ViewModels.Seller.OrderCheque;

public class FilterDepositOrderChequesSellerSideQuery : IRequest<FilterReciveOrderChequesSellerSideDTO>
{
    #region properties

    public FilterReciveOrderChequesSellerSideDTO FilterOrderChequesDTO { get; set; }

    #endregion
}
