#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

    #region List OF Seller Sent SMS

    [HttpGet]
    public async Task<IActionResult> ListOFSellerSentSMS()
    {
        return View(await _bulkSmsService.ListOFSellerSentSMS());    
    }

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
        #region Model State Validation 

        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion

        var res = await _bulkSmsService.UploadSellersExcelFileAndSendSMS(model);
        if (res)
        {
            TempData[SuccessMessage] = "پیامک برای لیست فروشندگان ارسال گردید.";
            return RedirectToAction(nameof(ListOFSellerSentSMS));
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(model);
    }

    #endregion

    #endregion

    #region Customers

    #region List OF Customer Sent SMS

    [HttpGet]
    public async Task<IActionResult> ListOFCustomerSentSMS()
    {
        return View(await _bulkSmsService.ListOFCustomerSentSMS());
    }

    #endregion

    #region Upload Excel File 

    [HttpGet]
    public async Task<IActionResult> UploadExcelFileForCustomers()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadExcelFileForCustomers(UploadExcelFileAdminSideViewModel model)
    {
        #region Model State Validation 

        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion

        var res = await _bulkSmsService.UploadCustomersExcelFileAndSendSMS(model);
        if (res)
        {
            TempData[SuccessMessage] = "پیامک برای لیست فروشندگان ارسال گردید.";
            return RedirectToAction(nameof(ListOFCustomerSentSMS));
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(model);
    }

    #endregion

    #endregion
}
