using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.Ticket
{
    public enum TicketType
    {
        [Display(Name = " مسائل فنی")]
        MasaEleFani,
        [Display(Name = " مشاوره ی رایگان")]
        MoshavereRaygan,
        [Display(Name = " تضمین در خرید")]
        TazminDarKharid,
    }
}
