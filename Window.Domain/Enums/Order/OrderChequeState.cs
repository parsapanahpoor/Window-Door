using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.Order;

public enum OrderChequeAdminState
{
    WaitingForCheck,
    Reject,
    Accept
}

public enum OrderChequeSellerState
{
    WaitingForCheck,
    Reject,
    Accept
}