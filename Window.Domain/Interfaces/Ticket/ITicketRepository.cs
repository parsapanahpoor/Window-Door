using Window.Domain.Entities.Contact;
using Window.Domain.ViewModels.Admin.Ticket;
using Window.Domain.ViewModels.User.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Interfaces
{
    public interface ITicketRepository
    {
        #region Admin Side

        Task<AdminFilterTicketViewModel> FilterAdminTicketViewModel(AdminFilterTicketViewModel filter);

        Task AddTicket(Ticket ticket);

        Task AddMessage(TicketMessage message);

        Task<Ticket?> GetTicketById(ulong ticketId);

        Task ReadTicketByAdmin(Ticket ticket);

        Task<List<TicketMessage>> GetTicketMessages(ulong ticketId);

        Task UpdateTicket(Ticket ticket);

        Task<TicketMessage?> GetTicketMessageById(ulong messageId);

        Task UpdateTicketMessage(TicketMessage ticketMessage);

        Task<List<Ticket>> GetLastestTicketForAdminDashboard();

        #endregion

        #region User Side

        Task<FilterSiteTicketViewModel> FilterSiteTicket(FilterSiteTicketViewModel filter, ulong userId);

        #endregion
    }
}
