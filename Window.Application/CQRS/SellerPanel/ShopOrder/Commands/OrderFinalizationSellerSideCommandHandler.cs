
using Window.Application.Common.IUnitOfWork;
using Window.Application.Convertors;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Services;
using Window.Application.StticTools;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.ShopOrder.Commands;

public record OrderFinalizationSellerSideCommandHandler : IRequestHandler<OrderFinalizationSellerSideCommand, bool>
{
    #region Ctor

    private static readonly HttpClient client = new HttpClient();
    private readonly IUserRepository _userRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderFinalizationSellerSideCommandHandler(IOrderQueryRepository orderQueryRepository ,
                                                     IOrderCommandRepository orderCommandRepository ,
                                                     IShopProductQueryRepository shopProductQueryRepository ,
                                                     IUserRepository userRepository,
                                                     IUnitOfWork unitOfWork)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(OrderFinalizationSellerSideCommand request, CancellationToken cancellationToken)
    {
        //Get Order by Id
        var order = await _orderQueryRepository.GetByIdAsync(cancellationToken , request.OrderId);
        if (order == null) return false;

        //Check That Is Product Owner And Current Seller Are The Same
        var productId = await _orderQueryRepository.GetLastestProductId_InOrderDetail_ByOrderId(request.OrderId);
        if (productId == 0) return false;

        //Get Product
        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, productId);
        if (product == null) return false;
        if (product.SellerUserId != request.SellerUserId) return false;

        //Order Finalization 
        order.IsFinally = true;

        _orderCommandRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        #region Send Alert SMS

        //Send SMS To Customer
        var customerMobile = await _userRepository.Get_UserMobile_ByUserId(order.UserId, cancellationToken);
        if (!string.IsNullOrEmpty(customerMobile))
        {
            string sellerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={order.Id}";

            var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={sellerLink}&token2={DateTime.Now.ToShamsi()}&template=FinalizeOrder-FromSeller-ForCustomer";
            var results = client.GetStringAsync(result);
        }

        #endregion

        return true;
    }
}
