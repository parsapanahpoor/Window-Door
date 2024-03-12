using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Domain.Interfaces.OrderCheque;

public interface IOrderChequeQueryRepository
{
    #region General Methods

    Task<Domain.Entities.ShopOrder.OrderCheque?> Get_OrderCheque_ByIdAsync(ulong chequeId,
                                                                           CancellationToken cancellation);

    #endregion

    #region Admin Side 

    Task<ChequeDetailDTO?> Fill_ChequeDetailDTO_ByOrderChequeId(ulong orderChequeId,
                                                                CancellationToken cancellationToken);

    #endregion

    #region Seller Side

    Task<Domain.Entities.Market.SellerChequeInfo?> Get_SellerChequeInfo_BySellerUserId(ulong sellerUserId,
                                                                                       CancellationToken cancellationToken);

    Task<List<Domain.Entities.ShopOrder.OrderCheque>> Get_ListOfCustomerOrderCheques_ByOrderAndUserId(ulong userId,
                                                                                                      ulong orderId,
                                                                                                      CancellationToken cancellationToken);

    Task<FilterOrderChequesDTO> FillFilterOrderChequesDTO(FilterOrderChequesDTO filter,
                                                          CancellationToken cancellation);

    #endregion
}
