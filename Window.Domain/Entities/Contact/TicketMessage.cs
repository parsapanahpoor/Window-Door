using Window.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Contact
{
    public class TicketMessage : BaseEntity
    {
        #region Properties

        public ulong TicketId { get; set; }

        public ulong SenderId { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public Ticket Ticket { get; set; }

        public User Sender { get; set; }

        #endregion
    }

}
