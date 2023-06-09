#region Usings

using Microsoft.AspNetCore.Mvc;
using Window.Application.Services.Interfaces;
using Window.Domain.ViewModels.Admin.BulkSMS;

namespace Window.Web.Areas.Admin.Controllers;

#endregion

public class BulkSMSController : AdminBaseController
{
	#region Ctor

	private readonly IBulkSMSService _bulkSmsService;

	public BulkSMSController(IBulkSMSService bulkSMSService)
	{
		_bulkSmsService= bulkSMSService;
	}

    #endregion

    #region Sellers

    #region List OF Employee Sent SMS

    #endregion

    #region Upload Excel File 

    [HttpGet]
	public async Task<IActionResult> UploadExcelFile()
	{
		return View();
	}

	[HttpPost]
    public async Task<IActionResult> UploadExcelFile(UploadExcelFileAdminSideViewModel model)
    {
        return View();
    }

    #endregion

    #endregion

    #region Customers

    #endregion
}
