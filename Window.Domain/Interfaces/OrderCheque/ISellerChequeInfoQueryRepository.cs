namespace Window.Domain.Interfaces.OrderCheque;

public interface IOrderChequeQueryRepository
{
    #region Seller Side

    Task<Domain.Entities.Market.SellerChequeInfo?> Get_SellerChequeInfo_BySellerUserId(ulong sellerUserId,
                                                                                       CancellationToken cancellationToken);

    Task<List<Domain.Entities.ShopOrder.OrderCheque>> Get_ListOfCustomerOrderCheques_ByOrderAndUserId(ulong userId,
                                                                                                      ulong orderId,
                                                                                                      CancellationToken cancellationToken);

    #endregion
}
