using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.UserMacAddress = User.GetUserId().ToString();

            var res = await _inquiryService.GetUserLastestInquiryDetailForChange(User.GetUserId().ToString());
            return View("ChatBar", res);
        }
    }
}
