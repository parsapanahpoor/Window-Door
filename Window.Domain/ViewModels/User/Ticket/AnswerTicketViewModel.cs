using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.User.Ticket
{
    public class AnswerTicketViewModel
    {
        public ulong TicketId { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Message { get; set; }

        public IFormFile? MessageFile { get; set; }
    }
}
