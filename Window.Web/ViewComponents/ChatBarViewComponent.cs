using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AngleSharp.Io;
using System.Net;

namespace CRM.Web.Areas.Admin.ViewComponents
{
    public class ChatBarViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IInquiryService _inquiryService;

        public ChatBarViewComponent(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            #region Get User Ip Address

            string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            #endregion

            ViewBag.UserMacAddress = Ip;
            

            var res = await _inquiryService.GetUserLastestInquiryDetailForChange(Ip);
            return View("ChatBar", res);
        }
    }

    public class NewChatBarViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IInquiryService _inquiryService;

        public NewChatBarViewComponent(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            #region Get User Ip Address

            string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            #endregion

            ViewBag.UserMacAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            var res = await _inquiryService.GetUserLastestInquiryDetailForChange(Ip);
            return View("NewChatBar", res);
        }
    }
}
