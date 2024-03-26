using Microsoft.AspNetCore.Http;

namespace Window.Domain.ViewModels.Seller.ChequeReceipt;

public record ShowOrderChequeReceiptDTO
{
    #region properties

    public ulong OrderChequeId { get; set; }

    public ulong OrderId { get; set; }

    public string? ChequeReceiptImageName { get; set; }

    public IFormFile? ChequeReceiptImageFile { get; set; }

    public string? ChequeReceiptDescription { get; set; }

    #endregion
}
