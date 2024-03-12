using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Window.Domain.Entities.Common;
using Window.Domain.Enums.Order;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.OrderCheque;

public class FilterOrderChequesDTO : BasePaging<OrderChequesDTO>
{
    #region properties

    public OrderChequeAdminStateDTO? OrderChequeAdminState { get; set; }

    public OrderChequeSellerStateDTO? OrderChequeSellerState { get; set; }

    public string? CustomerUsername { get; set; }

    public OrderFinallyStateDTO? OrderFinallyState { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    #endregion
}

public class OrderChequesDTO
{
    #region properties

    public ulong OrderId { get; set; }

    public ulong CustomerUserId { get; set; }

    public string CustomerUsername { get; set; }

    public decimal ChequePrice { get; set; }

    public OrderChequeAdminState OrderChequeAdminState { get; set; }

    public OrderChequeSellerState OrderChequeSellerState { get; set; }

    public DateTime ChequeDateTime { get; set; }

    public bool OrderIsFinally { get; set; }

    #endregion
}

public enum OrderChequeAdminStateDTO
{
    [Display(Name = "همه")] All,
    [Display(Name = "درانتظار بررسی پنجره یاب")] WaitingForCheck,
    [Display(Name = "رد شده توسط پنجره یاب")] Reject,
    [Display(Name = "تایید شده توسط پنجره یاب")] Accept
}

public enum OrderChequeSellerStateDTO
{
    [Display(Name = "همه")] All,
    [Display(Name = "درانتظار بررسی فروشنده")] WaitingForCheck,
    [Display(Name = "رد شده توسط فروشنده")] Reject,
    [Display(Name = "تایید شده توسط فروشنده")] Accept
}

public enum OrderFinallyStateDTO
{
    [Display(Name = "همه")] All,
    [Display(Name = "نهایی نشده")] NotFinally,
    [Display(Name = "نهایی شده")] Finally
}
