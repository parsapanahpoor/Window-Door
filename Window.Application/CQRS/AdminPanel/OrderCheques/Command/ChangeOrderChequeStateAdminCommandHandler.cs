
using Window.Application.Common.IUnitOfWork;
using Window.Application.Convertors;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Enums.Order;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Command;

public record ChangeOrderChequeStateAdminCommandHandler : IRequestHandler<ChangeOrderChequeStateAdminCommand, bool>
{
    #region Ctor

    private static readonly HttpClient client = new HttpClient();
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUserRepository _userRepository;
    private readonly IOrderChequeCommandRepository _commandRepository;
    private readonly IOrderChequeQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderChequeStateAdminCommandHandler(IOrderChequeCommandRepository commandRepository ,
                                                     IOrderChequeQueryRepository queryRepository ,
                                                     ISiteSettingService siteSettingService ,
                                                     IUserRepository userRepository ,
                                                     IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _siteSettingService = siteSettingService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(ChangeOrderChequeStateAdminCommand request, CancellationToken cancellationToken)
    {
        #region Get Order Cheque By Id 

        var cheque = await _queryRepository.Get_OrderCheque_ByIdAsync(request.OrderChequeId , cancellationToken);
        if (cheque == null) return false;

        #endregion

        #region Update Cheque

        cheque.OrderChequeAdminState = request.OrderChequeAdminState;
        cheque.AdminRejectDescription = request.AdminRejectDescription;

        if (request.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Accept)
                                                                cheque.AdminRejectDescription = null;

        //Update Method
        _commandRepository.Update_OrderCheque(cheque);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        #region Send Alert SMS

        var sellerMobile = await _userRepository.Get_UserMobile_ByUserId(cheque.SellerUserId , cancellationToken);

        var customerMobile = await _userRepository.Get_UserMobile_ByUserId (cheque.CustomerUserId , cancellationToken);

        if (request.OrderChequeAdminState == OrderChequeAdminState.Accept)
        {
            //Send SMS To Seller
            if (!string.IsNullOrEmpty(sellerMobile))
            {
                string sellerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={cheque.OrderId}";

                var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={sellerMobile}&token=={sellerLink}&token2={DateTime.Now.ToShamsi()}&template=Accept-FromAdmin-ForSeller";
                var results = client.GetStringAsync(result);
            }

            //Send SMS For Customer
            if (!string.IsNullOrEmpty(customerMobile))
            {
                string customerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={cheque.OrderId}";

                var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={customerLink}&token2={DateTime.Now.ToShamsi()}&template=Accept-FromAdmin-ForCustomer";
                var results = client.GetStringAsync(result);
            }
        }
        if (request.OrderChequeAdminState == OrderChequeAdminState.Reject)
        {
            //Send SMS To Seller
            if (!string.IsNullOrEmpty(sellerMobile))
            {
                string sellerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={cheque.OrderId}";

                var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={sellerMobile}&token=={sellerLink}&token2={DateTime.Now.ToShamsi()}&template=Reject-FromAdmin-ForSeller";
                var results = client.GetStringAsync(result);
            }

            //Send SMS For Customer
            if (!string.IsNullOrEmpty(customerMobile))
            {
                string customerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={cheque.OrderId}";

                var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={customerLink}&token2={DateTime.Now.ToShamsi()}&template=Reject-FromAdmin-ForCustomer";
                var results = client.GetStringAsync(result);
            }
        }

        #endregion

        return true;
    }
}
