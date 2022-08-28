using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.Services.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string to, string subject, string body);
        Task<EmailSetting> GetDefaultEmailSetting();
    }
}
