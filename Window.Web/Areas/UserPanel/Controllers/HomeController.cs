using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Window.Web.Areas.UserPanel.Controllers
{
    public class HomeController : UserPanelBaseController
    {
        #region Ctor

        

        #endregion

        #region User Panel Dashboard

        public async Task<IActionResult> Index()
        {
            return View();
        }

        #endregion
    }
}
