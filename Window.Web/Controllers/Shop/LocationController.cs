using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.Location.Query;
using Window.Application.Extensions;
using Window.Domain.ViewModels.Site.Shop.Location;

namespace Window.Web.Controllers.Shop;

[Authorize]
public class LocationController : SiteBaseController
{
	#region List Of User Location

	public async Task<IActionResult> ListOfUserLocations(CancellationToken cancellation = default)
	{
		return View(await Mediator.Send(new ListOfUserLocationsQuery()
		{
			UserId = User.GetUserId()
		} , 
		cancellation));
	}

	#endregion
}
