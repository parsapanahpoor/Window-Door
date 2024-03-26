using Window.Application.Common.IUnitOfWork;
using Window.Application.CQRS.SellerPanel.ChequeReceipt.Query;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.ChequeReceipt.Command;

public record UploadChequeReceiptCommandHandler : IRequestHandler<UploadChequeReceiptCommand, UploadChequeReceiptResult>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;
    private readonly IOrderChequeCommandRepository _orderChequeCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadChequeReceiptCommandHandler(IOrderChequeQueryRepository orderChequeQueryRepository ,
                                             IOrderChequeCommandRepository orderChequeCommandRepository ,
                                             IUnitOfWork unitOfWork)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
        _orderChequeCommandRepository = orderChequeCommandRepository;
        _unitOfWork = unitOfWork;
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

        return UploadChequeReceiptResult.Success;
    }
}
