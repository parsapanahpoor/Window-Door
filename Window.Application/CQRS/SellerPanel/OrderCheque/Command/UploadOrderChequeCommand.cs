using Microsoft.AspNetCore.Http;
using Window.Domain.ViewModels.Seller.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Command;

public class UploadOrderChequeCommand : IRequest<UploadOrderChequeResult>
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong CustomerUserId { get; set; }

    public IFormFile ChequeImage { get; set; }

    public decimal ChequePrice { get; set; }

    public string CustomerNationalId { get; set; }

    public string ChequeDateTime { get; set; }

    #endregion
}

public enum UploadOrderChequeResult
{
    SellerDosentSupportCheque,
    SellerHasAnLimitationOFDay,
    SellerHasAnLimitationOfChequeCount,
    ThisIsNotYourOrder,
    Success,
    Faild
}