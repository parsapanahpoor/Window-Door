using Window.Domain.Interfaces;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopOrder;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;

public record ManageShopOrderDetailQueryHandler : IRequestHandler<ManageShopOrderDetailQuery, ManageShopOrderDetailDTO>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IOrderChequeQueryRepository _sellerChequeInfo;
    private readonly IUserRepository _userRepository;

    public ManageShopOrderDetailQueryHandler(IOrderQueryRepository orderQueryRepository,
                                             IShopProductQueryRepository shopProductQueryRepository,
                                             IOrderChequeQueryRepository sellerChequeInfo,
                                             IUserRepository userRepository)
    {
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _sellerChequeInfo = sellerChequeInfo;
        _userRepository = userRepository;
    }

    #endregion

    public async Task<ManageShopOrderDetailDTO?> Handle(ManageShopOrderDetailQuery request, CancellationToken cancellationToken)
    {
        #region Fill Model 

        ManageShopOrderDetailDTO? model = null;

        if (request.orderId.HasValue)
        {
            model = await _orderQueryRepository.FillManageShopOrderDetailDTO(request.userId, request.orderId.Value , cancellationToken);
        }
        else
        {
            model = await _orderQueryRepository.FillManageShopOrderDetailDTO(request.userId, cancellationToken);
        }

        if (model == null) return null;

        //Fill Customer Cheque Information
        if (model.OrderCheques != null && model.OrderCheques.Any())
        {
            var lastestChequeDate = model.OrderCheques.OrderByDescending(p => p.ChequeDateTime)
                                                      .Select(p => p.ChequeDateTime)
                                                      .FirstOrDefault();

            CustomerChequeInformation customerChequeInfo = new CustomerChequeInformation();

            //Cheque Days
            customerChequeInfo.ChequeDays = lastestChequeDate.DayOfYear - DateTime.Now.DayOfYear;

            //Count Of Cheques
            customerChequeInfo.CountOfCheque = model.OrderCheques.Count();

            model.CustomerChequeInformation = customerChequeInfo;
        }

        //Fill Seller Informations && Fill Seller ChequeInfo
        if (model.OrderDetails != null && model.OrderDetails.Any())
        {
            var sellerIdOfProduct = await _shopProductQueryRepository.GetSellerId_ByProductId(model.OrderDetails
                                                                                              .Select(p => p.ProductId)
                                                                                              .FirstOrDefault(),
                                                                                              cancellationToken);

            model.SellerInformations = await _userRepository.Fill_SellerInformations(sellerIdOfProduct, cancellationToken);

            model.sellerChequeInfo = await _sellerChequeInfo.Get_SellerChequeInfo_BySellerUserId(sellerIdOfProduct,
                                                                                                 cancellationToken);

        }

        #endregion

        return model;
    }
}
