using Microsoft.AspNetCore.Http;
using Window.Domain.Enums.Order;
namespace Window.Domain.ViewModels.Seller.OrderCheque;

public record UploadOrderChequeDTO
{
    #region properties

    public ulong OrderId { get; set; }

    public IFormFile ChequeImage { get; set; }

    public decimal ChequePrice { get; set; }

    public string CustomerNationalId { get; set; }

    public string ChequeDateTime { get; set; }

    #endregion
}
