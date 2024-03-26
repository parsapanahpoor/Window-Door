using Window.Domain.ViewModels.Seller.ChequeReceipt;

namespace Window.Application.CQRS.SellerPanel.ChequeReceipt.Command;

public class UploadChequeReceiptCommand : IRequest<UploadChequeReceiptResult>
{
    #region properties

    public ulong CustomerUserId{ get; set; }

    public ShowOrderChequeReceiptDTO ChequeReceipt { get; set; }

    #endregion
}

public enum UploadChequeReceiptResult
{
    Success,
    SellerDosentAccept,
    OrderChequeNotfound
}