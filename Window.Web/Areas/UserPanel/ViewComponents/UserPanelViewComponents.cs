
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Window.Application.Extensions;
using Window.Application.Interfaces;

namespace DoctorType.Web.Areas.UserPanel.ViewComponents
{

    #region UserPanel SideBar ViewComponent

    public class UserPanelSideBarViewComponent : ViewComponent
    {
        #region Ctor
        
        private readonly IUserService _userService;

        public UserPanelSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserPanelSideBar" , await _userService.GetUserById(User.GetUserId()));
        }
    }

    #endregion

    #region UserPanel Header ViewComponent

    public class UserPanelHeaderViewComponent : ViewComponent
    {
        #region Ctor 

        private readonly IUserService _userService;

        public UserPanelHeaderViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserPanelHeader", await _userService.GetUserById(User.GetUserId())) ;
        }
    }

    #endregion

    #region UserPanel Chatbox ViewComponent

    public class UserPanelChatboxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserPanelChatbox");
        }
    }

    #endregion

    #region UserPanel Footer ViewComponent

    public class UserPanelFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserPanelFooter");
        }
    }

    #endregion

    #region Notification Tickets ViewComponent

    public class UserPanelNotificationTicketsViewComponent : ViewComponent
    {
        //#region Constructor

        //private readonly IContactUsService _contactUsService;
        //public NotificationTicketsViewComponent(IContactUsService contactUsService)
        //{
        //    this._contactUsService = contactUsService;
        //}

        //#endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserPanelNotificationTickets");
        }
    }

    #endregion

    #region Incoming Email Messages

    public class UserPanelUnReadEmailsViewComponent : ViewComponent
    {
        //private readonly IContactUsService _contactUsService;

        //public UnReadEmailsViewComponent(IContactUsService contactUsService)
        //{
        //    _contactUsService = contactUsService;
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //List<Domain.Entities.ContactUs.ContactUs> Messages = await _contactUsService.GetLastestEmailMessages(5);

            return await Task.FromResult((IViewComponentResult)View("UserPanelUnReadEmails"));
        }
    }

    #endregion
}
