using Window.Application.Common.IUnitOfWork;
using Window.Application.Convertors;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Enums.Order;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Command;

internal class ChangeOrderChequeStateSellerCommandHandler : IRequestHandler<ChangeOrderChequeStateSellerCommand, bool>
{
    #region Ctor

    private static readonly HttpClient client = new HttpClient();
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUserRepository _userRepository;
    private readonly IOrderChequeCommandRepository _commandRepository;
    private readonly IOrderChequeQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderChequeStateSellerCommandHandler(IOrderChequeCommandRepository commandRepository,
                                                     IOrderChequeQueryRepository queryRepository,
                                                     ISiteSettingService siteSettingService,
                                                     IUserRepository userRepository,
                                                     IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _siteSettingService = siteSettingService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(ChangeOrderChequeStateSellerCommand request, CancellationToken cancellationToken)
    {
        #region Get Order Cheque By Id 

        var cheque = await _queryRepository.Get_OrderCheque_ByIdAsync(request.OrderChequeId, cancellationToken);
        if (cheque == null) return false;
        if (cheque.SellerUserId != request.SellerUserId) return false;

        #endregion

        #region Update Cheque

        cheque.OrderChequeSellerState = request.OrderChequeSellerState;
        cheque.SellerRejectDescription = request.SellerRejectDescription;

        if (request.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Accept)
            cheque.SellerRejectDescription = null;

        //Update Method
        _commandRepository.Update_OrderCheque(cheque);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        #region Send Alert SMS

        var adminsMobiles = await _siteSettingService.GetListOf_AdminsMobiles(cancellationToken);

        var customerMobile = await _userRepository.Get_UserMobile_ByUserId(cheque.CustomerUserId, cancellationToken);

        if (request.OrderChequeSellerState == OrderChequeSellerState.Accept)
        {
            //Send SMS To Admins
            if (adminsMobiles != null && adminsMobiles.Any())
            {
                string adminLink = $"{FilePaths.SiteAddress}/Admin/OrderCheques/ShowOrderChequeDetail?orderId={cheque.OrderId}";

                foreach (var adminMobile in adminsMobiles)
                {
                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={adminMobile}&token=={adminLink}&token2={DateTime.Now.ToShamsi()}&template=AcceptCheque-FromSeller-ForAdmin";
                    var results = client.GetStringAsync(result);
                }
            }

            //Send SMS For Customer
            if (!string.IsNullOrEmpty(customerMobile))
            {
                string customerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={cheque.OrderId}";

                if (string.IsNullOrEmpty(cheque.ChequeReceiptFileName))
                {

                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={customerLink}&token2={DateTime.Now.ToShamsi()}&template=Accept-FromSeller-ForCustomer";
                    var results = client.GetStringAsync(result);  
                }
                if (!string.IsNullOrEmpty(cheque.ChequeReceiptFileName))
                {
                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={customerLink}&token2={DateTime.Now.ToShamsi()}&template=Accept-ChequeReceipt-FromSeller-ForCustomer";
                    var results = client.GetStringAsync(result);
                }

                if (!string.IsNullOrEmpty(request.SellerRejectDescription))
                {
                    var description = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token={request.SellerRejectDescription}&token2={customerLink}&template=SellerDescription-FromSeller-ForCustomer";
                    var sendDescription = client.GetStringAsync(description);
                }
            }
        }
        if (request.OrderChequeSellerState == OrderChequeSellerState.Reject)
        {
            //Send SMS To Admins
            if (adminsMobiles != null && adminsMobiles.Any())
            {
                string adminLink = $"{FilePaths.SiteAddress}/Admin/OrderCheques/ShowOrderChequeDetail?orderId={cheque.OrderId}";

                foreach (var adminMobile in adminsMobiles)
                {
                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={adminMobile}&token=={adminLink}&token2={DateTime.Now.ToShamsi()}&template=RejectCheque-FromSeller-ForAdmin";
                    var results = client.GetStringAsync(result);
                }
            }

            //Send SMS For Customer
            if (!string.IsNullOrEmpty(customerMobile))
            {
                string customerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={cheque.OrderId}";

                if (string.IsNullOrEmpty(cheque.ChequeReceiptFileName))
                {
                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={customerLink}&token2={DateTime.Now.ToShamsi()}&template=Reject-FromSeller-ForCustomer";
                    var results = client.GetStringAsync(result);
                }
                if (!string.IsNullOrEmpty(cheque.ChequeReceiptFileName))
                {
                    var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token=={customerLink}&token2={DateTime.Now.ToShamsi()}&template=Reject-ChequeReceipt-FromSeller-ForCustomer";
                    var results = client.GetStringAsync(result);
                }

                if (!string.IsNullOrEmpty(request.SellerRejectDescription))
                {
                    var description = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={customerMobile}&token={request.SellerRejectDescription}&token2={customerLink}&template=SellerDescription-FromSeller-ForCustomer";
                    var sendDescription = client.GetStringAsync(description);
                }
            }
        }

        #endregion

        return true;
    }
}
