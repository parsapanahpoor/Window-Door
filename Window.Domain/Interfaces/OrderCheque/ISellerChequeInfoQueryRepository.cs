using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;
using Window.Domain.ViewModels.Seller.SellerChequeInfo;

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

    Task<SellerChequeInfoSellerSideDTO?> Fill_SellerChequeInfoSellerSide_DTO(ulong sellerUserId,
                                                                             CancellationToken cancellationToken);

    Task<Domain.Entities.Market.SellerChequeInfo?> Get_SellerChequeInfo_BySellerUserId(ulong sellerUserId, 
                                                                                       CancellationToken cancellationToken);

    Task<Domain.Entities.Market.SellerChequeInfo>? Get_SellerChequeInfo_BySellerUserId_Sync(ulong sellerUserId);

    Task<List<Domain.Entities.ShopOrder.OrderCheque>> Get_ListOfCustomerOrderCheques_ByOrderAndUserId(ulong userId,
                                                                                                      ulong orderId,
                                                                                                      CancellationToken cancellationToken);

    Task<FilterOrderChequesDTO> FillFilterOrderChequesDTO(FilterOrderChequesDTO filter,
                                                          CancellationToken cancellation);

    Task<FilterReciveOrderChequesSellerSideDTO> FillFilterReciveOrderChequesDTO(FilterReciveOrderChequesSellerSideDTO filter,
                                                                                CancellationToken cancellation);

    Task<FilterReciveOrderChequesSellerSideDTO> FillFilterDepositOrderChequesDTO(FilterReciveOrderChequesSellerSideDTO filter,
                                                                                 CancellationToken cancellation);

    Task<ChequeDetailSellerSideDTO?> Fill_SellerSideChequeDetailDTO_ByOrderChequeId(ulong orderChequeId,
                                                                                    ulong sellerUserId,
                                                                                    CancellationToken cancellationToken);

    #endregion
}
