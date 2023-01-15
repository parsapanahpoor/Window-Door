using Window.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Enums.Ticket;

namespace Window.Domain.ViewModels.Admin.Ticket
{
    public class AdminFilterTicketViewModel : BasePaging<Entities.Contact.Ticket>
    {
        public AdminFilterTicketViewModel()
        {
            AdminTicketFilterSeenByAdminStatus = AdminTicketFilterSeenByAdminStatus.All;
            AdminTicketFilterSeenByUserStatus = AdminTicketFilterSeenByUserStatus.All;
            AdminTicketFilterStatus = AdminTicketFilterStatus.All;
            AdminTicketFilterOnWorkingStatus = AdminTicketFilterOnWorkingStatus.All;
        }

        public string? TicketTitle { get; set; }

        public string? OwnerName { get; set; }

        public AdminTicketFilterSeenByAdminStatus AdminTicketFilterSeenByAdminStatus { get; set; }

        public AdminTicketFilterSeenByUserStatus AdminTicketFilterSeenByUserStatus { get; set; }

        public AdminTicketFilterStatus AdminTicketFilterStatus { get; set; }

        public AdminTicketFilterOnWorkingStatus AdminTicketFilterOnWorkingStatus { get; set; }

        public AdminTicketType? AdminTicketType { get; set; }

        public string? mobile { get; set; }
    }

    public enum AdminTicketFilterSeenByAdminStatus
    {
        [Display(Name = "All")] All,
        [Display(Name = "Seen By Admin")] SeenByAdmin,
        [Display(Name = "Admin Doesnt See")] NotSeenByAdmin
    }

    public enum AdminTicketFilterSeenByUserStatus
    {
        [Display(Name = "All")] All,
        [Display(Name = "Seen By User")] SeenByUser,
        [Display(Name = "User Doesnt See")] NotSeenByUser
    }

    public enum AdminTicketFilterStatus
    {
        [Display(Name = "All")] All,
        [Display(Name = "Answered")] Answered,
        [Display(Name = "On Working")] Pending,
        [Display(Name = "Closed")] Closed
    }

    public enum AdminTicketFilterOnWorkingStatus
    {
        [Display(Name = "All")] All,
        [Display(Name = "On Working")] OnWorking,
        [Display(Name = "NotWorking")] NotWorking
    }

    public enum AdminTicketType
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = " مسائل فنی")]
        MasaEleFani,
        [Display(Name = " مشاوره ی رایگان")]
        MoshavereRaygan,
        [Display(Name = " تضمین در خرید")]
        TazminDarKharid,
    }
}
