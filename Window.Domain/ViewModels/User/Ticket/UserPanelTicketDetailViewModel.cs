using Window.Domain.Entities.Contact;
using Window.Domain.Enums.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.User.Ticket
{
    public class UserPanelTicketDetailViewModel
    {
        public string TicketTitle { get; set; }

        public string CreateDate { get; set; }

        public string Status { get; set; }

        public int MessagesCount { get; set; }

        public ulong OwnerId { get; set; }

        public List<TicketMessage> TicketMessages { get; set; }

        public TicketStatus TicketStatus { get; set; }
    }
}
