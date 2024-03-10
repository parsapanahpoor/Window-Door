using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopOrder;
using Window.Domain.ViewModels.Site.Shop.Landing;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Qeuries;

public record ManageShopOrderDetailQueryHandler : IRequestHandler<ManageShopOrderDetailQuery, ManageShopOrderDetailDTO>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public ManageShopOrderDetailQueryHandler(IOrderQueryRepository orderQueryRepository,
                                             IShopProductQueryRepository shopProductQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<ManageShopOrderDetailDTO?> Handle(ManageShopOrderDetailQuery request, CancellationToken cancellationToken)
    {
        #region Fill Model 

        var model = await _orderQueryRepository.FillManageShopOrderDetailDTO(request.userId, cancellationToken);
        if (model == null) return null;

        //Fill Customer Cheque Information
        CustomerChequeInformation customerChequeInfo = new CustomerChequeInformation();
        if (model.OrderCheques != null && model.OrderCheques.Any())
        {
            var lastestChequeDate = model.OrderCheques.OrderByDescending(p => p.ChequeDateTime)
                                                      .Select(p => p.ChequeDateTime)
                                                      .FirstOrDefault();
            //Cheque Days
            customerChequeInfo.ChequeDays = lastestChequeDate.DayOfYear - DateTime.Now.DayOfYear;

            //Count Of Cheques
            customerChequeInfo.CountOfCheque = model.OrderCheques.Count();

            model.CustomerChequeInformation = customerChequeInfo;
        }

        //Fill Seller Informations
        SellerInformations sellerInformations = new SellerInformations();
        if (model.OrderDetails != null && model.OrderDetails.Any())
        {
            var sellerIdOfProduct = await _shopProductQueryRepository.GetSellerId_ByProductId(model.OrderDetails
                                                                                              .Select(p => p.ProductId)
                                                                                              .FirstOrDefault(),
                                                                                              cancellationToken);


        }

        #endregion

        return model;
    }
}
