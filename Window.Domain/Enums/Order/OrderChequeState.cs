using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.Order;

public enum OrderChequeAdminState
{
    [Display(Name = "درانتظار بررسی")] WaitingForCheck,
    [Display(Name = "رد شده")] Reject,
    [Display(Name = "تایید شده")] Accept
}

public enum OrderChequeSellerState
{
    [Display(Name = "درانتظار بررسی")] WaitingForCheck,
    [Display(Name = "رد شده")] Reject,
    [Display(Name = "تایید شده")] Accept
}