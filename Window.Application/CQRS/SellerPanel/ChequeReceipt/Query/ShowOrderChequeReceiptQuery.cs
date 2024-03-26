using Window.Domain.ViewModels.Seller.ChequeReceipt;

namespace Window.Application.CQRS.SellerPanel.ChequeReceipt.Query;

public record ShowOrderChequeReceiptQuery : IRequest<ShowOrderChequeReceiptQueryResult>
{
    public ulong OrderChequeId { get; set; }

    public ulong OrderId { get; set; }

    public ulong CustomerUserId { get; set; }
}

public class ShowOrderChequeReceiptQueryResult
{
    #region properties

    public ShowOrderChequeReceiptDTO? ChequeReceipt { get; set; }

    public ShowOrderChequeReceiptQueryResultEnum EnumResult { get; set; }

    #endregion
}

public enum ShowOrderChequeReceiptQueryResultEnum
{
    Success,
    SellerDosentAccept,
    OrderChequeNotFound
}