
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using System.Globalization;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Convertors;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.SiteSetting;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Command;

public record UploadOrderChequeCommandHandler : IRequestHandler<UploadOrderChequeCommand, UploadOrderChequeResult>
{
    #region Ctor

    private static readonly HttpClient client = new HttpClient();
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUserRepository _userRepository;
    private readonly IOrderChequeCommandRepository _orderChequeCommandRepository;
    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadOrderChequeCommandHandler(IOrderChequeCommandRepository orderChequeCommandRepository,
                                           IOrderChequeQueryRepository orderChequeQueryRepository,
                                           IOrderQueryRepository orderQueryRepository,
                                           IShopProductQueryRepository shopProductQueryRepository,
                                           ISiteSettingService siteSettingService,
                                           IUserRepository userRepository,
                                           IUnitOfWork unitOfWork)
    {
        _orderChequeCommandRepository = orderChequeCommandRepository;
        _orderChequeQueryRepository = orderChequeQueryRepository;
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _siteSettingService = siteSettingService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<UploadOrderChequeResult> Handle(UploadOrderChequeCommand request, CancellationToken cancellationToken)
    {
        #region Initial Cheque Date Time 

        var spliteDate = request.ChequeDateTime.Split('/');
        int year = int.Parse(spliteDate[0]);
        int month = int.Parse(spliteDate[1]);
        int day = int.Parse(spliteDate[2]);
        DateTime chequeDateTime = new DateTime(year, month, day, new PersianCalendar());

        #endregion

        #region Get Order By Id 

        //Find Order By Order Id
        var order = await _orderQueryRepository.GetByIdAsync(cancellationToken, request.OrderId);
        if (order == null || order.UserId != request.CustomerUserId) return UploadOrderChequeResult.ThisIsNotYourOrder;

        #endregion

        #region Validations Before Upload New Cheque

        //Get One Of Order Detail's Product Id 
        ulong productId = await _orderQueryRepository.GetLastestProductId_InOrderDetail_ByOrderId(order.Id);
        if (productId == 0) return UploadOrderChequeResult.Faild;

        //Find Seller User Id By Product Id 
        ulong sellerUserId = await _shopProductQueryRepository.GetSellerId_ByProductId(productId, cancellationToken);
        if (sellerUserId == 0) return UploadOrderChequeResult.Faild;

        //Find Seller Order Cheque Information
        var sellerOrderChequeInformation = await _orderChequeQueryRepository.Get_SellerChequeInfo_BySellerUserId(sellerUserId,
                                                                                                                 cancellationToken);
        if (sellerOrderChequeInformation == null) return UploadOrderChequeResult.SellerDosentSupportCheque;

        if (sellerOrderChequeInformation.HasLimitation)
        {
            //Get List Of Customer Cheques
            var customerOrderCheques = await _orderChequeQueryRepository
                                             .Get_ListOfCustomerOrderCheques_ByOrderAndUserId(request.CustomerUserId,
                                                                                              request.CustomerUserId,
                                                                                              cancellationToken);
            if (customerOrderCheques != null && customerOrderCheques.Any())
            {
                //Limitation in Cheques Count 
                if (customerOrderCheques.Count() >= sellerOrderChequeInformation.CountOfCheque)
                    return UploadOrderChequeResult.SellerHasAnLimitationOfChequeCount;

                int lastestChequeDays = chequeDateTime.DayOfYear - DateTime.Now.DayOfYear;

                //Limitation in Maximum Days Of Cheques
                if (lastestChequeDays > sellerOrderChequeInformation.SellerMaximumDays)
                    return UploadOrderChequeResult.SellerHasAnLimitationOFDay;
            }
        }

        #endregion

        #region Add Order Cheque

        Domain.Entities.ShopOrder.OrderCheque orderCheque = new Domain.Entities.ShopOrder.OrderCheque()
        {
            AdminRejectDescription = null,
            ChequeDateTime = chequeDateTime,
            ChequePrice =request.ChequePrice,
            CreateDate = DateTime.Now,
            CustomerNationalId = request.CustomerNationalId,
            CustomerUserId = request.CustomerUserId,
            IsDelete = false,
            OrderChequeAdminState = Domain.Enums.Order.OrderChequeAdminState.WaitingForCheck,
            OrderChequeSellerState = Domain.Enums.Order.OrderChequeSellerState.WaitingForCheck,
            OrderId = request.OrderId,
            SellerRejectDescription = null,
            SellerUserId = sellerUserId
        };

        if (request.ChequeImage != null && request.ChequeImage.IsImage())
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(request.ChequeImage.FileName);
            request.ChequeImage.AddImageToServer(imageName, FilePaths.OrderChequePathServer, 400, 300, FilePaths.OrderChequePathThumbServer);
            orderCheque.ChequeImage = imageName;
        }

        //Add Order Cheque To Data Base
        await _orderChequeCommandRepository.AddOrderCheque(orderCheque , cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        #region Send Alert SMS

        //Send SMS To Admins
        var adminsMobiles = await _siteSettingService.GetListOf_AdminsMobiles(cancellationToken);
        if (adminsMobiles != null && adminsMobiles.Any())
        {
            string adminLink = $"{FilePaths.SiteAddress}/Admin/OrderCheques/ShowOrderChequeDetail?orderId={order.Id}";

            foreach (var adminMobile in adminsMobiles)
            {
                var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={adminMobile}&token=={adminLink}&token2={DateTime.Now.ToShamsi()}&template=NewCheque-ForAdmin";
                var results = client.GetStringAsync(result);
            }
        }

        //Send SMS To Seller
        var sellerMobile = await _userRepository.Get_UserMobile_ByUserId(sellerUserId , cancellationToken);
        if (!string.IsNullOrEmpty(sellerMobile))
        {
            string sellerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ManageShopOrder?orderId={order.Id}";

            var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={sellerMobile}&token=={sellerLink}&token2={DateTime.Now.ToShamsi()}&template=NewCheque-ForSeller";
            var results = client.GetStringAsync(result);
        }

        #endregion

        return UploadOrderChequeResult.Success;
    }
}
