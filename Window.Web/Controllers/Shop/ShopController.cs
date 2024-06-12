#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.ShopLanding;
namespace Window.Web.Controllers;

#endregion

[Authorize]
public class ShopController : SiteBaseController
{
    #region Ctor



    #endregion

    #region Shop Landing

    public async Task<IActionResult> ShopLanding(CancellationToken cancellation = default)
    {
        var model = await Mediator.Send(new ShopLandingQuery() , cancellation);

        return View(model);
    }

    #endregion
}
