using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;

public class FilterReciveOrderChequesSellerSideQuery : IRequest<FilterReciveOrderChequesSellerSideDTO>
{
    #region properties

    public FilterReciveOrderChequesSellerSideDTO FilterOrderChequesDTO { get; set; }

    #endregion
}

