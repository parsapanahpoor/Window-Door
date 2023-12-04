#region Usings

using Microsoft.AspNetCore.Mvc;
namespace Window.Web.Controllers;

#endregion

public class ShopController : SiteBaseController
{
    #region Ctor



    #endregion

    #region Shop Landing

    public async Task<IActionResult> ShopLanding()
    {
        return View();
    }

    #endregion
}
