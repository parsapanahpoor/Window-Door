using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Seller.ChequeReceipt;

namespace Window.Application.CQRS.SellerPanel.ChequeReceipt.Query;

public record ShowOrderChequeReceiptQueryHandler : IRequestHandler<ShowOrderChequeReceiptQuery, ShowOrderChequeReceiptQueryResult>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;

    public ShowOrderChequeReceiptQueryHandler(IOrderChequeQueryRepository orderChequeQueryRepository)
    {
        _orderChequeQueryRepository = orderChequeQueryRepository;
    }

    #endregion

    public async Task<ShowOrderChequeReceiptQueryResult> Handle(ShowOrderChequeReceiptQuery request, CancellationToken cancellationToken)
    {
        //Get Chque By Id 
        var cheque = await _orderChequeQueryRepository.Get_OrderCheque_ByIdAsync(request.OrderChequeId , cancellationToken);
        if (cheque == null) return new ShowOrderChequeReceiptQueryResult()
        {
            ChequeReceipt = null ,
            EnumResult = ShowOrderChequeReceiptQueryResultEnum.OrderChequeNotFound
        };

        if (cheque.CustomerUserId != request.CustomerUserId) return new ShowOrderChequeReceiptQueryResult()
        {
            ChequeReceipt = null,
            EnumResult = ShowOrderChequeReceiptQueryResultEnum.OrderChequeNotFound
        };

        if (cheque.OrderChequeSellerState != Domain.Enums.Order.OrderChequeSellerState.Accept) return new ShowOrderChequeReceiptQueryResult()
        {
            ChequeReceipt = null,
            EnumResult = ShowOrderChequeReceiptQueryResultEnum.SellerDosentAccept
        };

        return new ShowOrderChequeReceiptQueryResult()
        {
            ChequeReceipt = new ShowOrderChequeReceiptDTO()
            {
                ChequeReceiptDescription = cheque.CustomerChequeReceiptDescription,
                ChequeReceiptImageFile = null , 
                ChequeReceiptImageName = cheque.ChequeReceiptFileName,
                OrderChequeId = request.OrderChequeId ,
                OrderId = request.OrderId ,
            },
            EnumResult = ShowOrderChequeReceiptQueryResultEnum.Success
        };
    }
}
