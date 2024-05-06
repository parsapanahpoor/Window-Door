using OfficeOpenXml.Style;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Convertors;
using Window.Application.CQRS.SellerPanel.ChequeReceipt.Query;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.ChequeReceipt.Command;

public record UploadChequeReceiptCommandHandler : IRequestHandler<UploadChequeReceiptCommand, UploadChequeReceiptResult>
{
    #region Ctor

    private static readonly HttpClient client = new HttpClient();
    private readonly ISiteSettingService _siteSettingService;
    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;
    private readonly IOrderChequeCommandRepository _orderChequeCommandRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadChequeReceiptCommandHandler(IOrderChequeQueryRepository orderChequeQueryRepository,
                                             IOrderChequeCommandRepository orderChequeCommandRepository,
                                             IUnitOfWork unitOfWork,
                                             ISiteSettingService siteSettingService,
                                             IUserRepository userRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
        _orderChequeCommandRepository = orderChequeCommandRepository;
        _unitOfWork = unitOfWork;
        _siteSettingService = siteSettingService;
        _userRepository = userRepository;
    }

    #endregion

    public async Task<UploadChequeReceiptResult> Handle(UploadChequeReceiptCommand request, CancellationToken cancellationToken)
    {
        //Get Chque By Id 
        var cheque = await _orderChequeQueryRepository.Get_OrderCheque_ByIdAsync(request.ChequeReceipt.OrderChequeId, cancellationToken);

        if (cheque == null) return UploadChequeReceiptResult.OrderChequeNotfound;
        if (cheque.CustomerUserId != request.CustomerUserId) return UploadChequeReceiptResult.OrderChequeNotfound;
        if (cheque.OrderChequeSellerState != Domain.Enums.Order.OrderChequeSellerState.Accept) return UploadChequeReceiptResult.SellerDosentAccept;

        //Uplaod Receipt Image 
        if (request.ChequeReceipt.ChequeReceiptImageFile != null && request.ChequeReceipt.ChequeReceiptImageFile.IsImage())
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(request.ChequeReceipt.ChequeReceiptImageFile.FileName);
            request.ChequeReceipt.ChequeReceiptImageFile.AddImageToServer(imageName, FilePaths.ChequeReceiptPathServer, 400, 300, FilePaths.ChequeReceiptPathThumbServer);
            cheque.ChequeReceiptFileName = imageName;
        }

        cheque.CustomerChequeReceiptDescription = request.ChequeReceipt.ChequeReceiptDescription;

        //Update Cheque
        _orderChequeCommandRepository.Update_OrderCheque(cheque);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        #region Send Alert SMS

        //Send SMS To Seller
        var sellerMobile = await _userRepository.Get_UserMobile_ByUserId(cheque.SellerUserId, cancellationToken);
        if (!string.IsNullOrEmpty(sellerMobile))
        {
            string sellerLink = $"{FilePaths.SiteAddress}/Seller/ShopOrder/ChequeDetail?orderChequeId={cheque.Id}";

            var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={sellerMobile}&token=={sellerLink}&token2={DateTime.Now.ToShamsi()}&template=NewChequeReceipt-ForSeller";
            var results = client.GetStringAsync(result);
        }

        #endregion

        return UploadChequeReceiptResult.Success;
    }
}
