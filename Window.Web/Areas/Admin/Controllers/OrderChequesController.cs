using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.AdminPanel.OrderCheques.Query;
using Window.Domain.ViewModels.Admin.OrderCheque;
namespace Window.Web.Areas.Admin.Controllers;

public class OrderChequesController : AdminBaseController
{
	#region Filter Order Cheques

	public async Task<IActionResult> FilterOrderCheques(FilterOrderChequesDTO filter , 
														CancellationToken cancellationToken = default)
	{
		return View(await Mediator.Send(new FilterOrderChequesQuery()
		{
			FilterOrderChequesDTO = filter ,
		},
		cancellationToken)) ;
	}

	#endregion

	#region MyRegion

	#endregion
}
