#region Usings

using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using SixLabors.ImageSharp.ColorSpaces;
using Window.Application.Services.Interfaces;
using Window.Application.StaticTools;
using Window.Domain.ViewModels.Admin.BulkSMS;

namespace Window.Web.Areas.Admin.Controllers;

#endregion

public class BulkSMSController : AdminBaseController
{
    #region Ctor

    private readonly IBulkSMSService _bulkSmsService;
    private readonly ISMSService _smsService;

    public BulkSMSController(IBulkSMSService bulkSMSService, ISMSService smsService)
    {
        _bulkSmsService = bulkSMSService;
        _smsService = smsService;
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
        if (res != null)
        {
            #region Send SMS  

            foreach (var item in res)
            {
                var smstext = $"{item.Usernames} عزیز .{Environment.NewLine} {model.SMSText}";
                await _smsService.SendSimpleSMS(item.UserMobiles, smstext);
            }

            #endregion

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
        if (res != null && res.Any())
        {
            #region Send SMS  

            foreach (var item in res)
            {
                var smstext = $"{item.Usernames} عزیز .{Environment.NewLine} {model.SMSText}";
                await _smsService.SendSimpleSMS(item.UserMobiles, smstext);
            }

            #endregion

            TempData[SuccessMessage] = "پیامک برای لیست فروشندگان ارسال گردید.";
            return RedirectToAction(nameof(ListOFCustomerSentSMS));
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(model);
    }

    #endregion

    #endregion

    #region All Bulk SMS 

    #region List Of All Bulk SMS Records

    //List Of All Bulk SMS Records
    public async Task<IActionResult> ListOfAllBulkSMSRecords()
    {
        return View(await _bulkSmsService.ListOfAllBulkSMSRecords()) ;
    }

    #endregion

    #region Upload Excel File 

    [HttpGet]
    public async Task<IActionResult> UploadExcelFileJustForAddUsersRecords()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadExcelFileJustForAddUsersRecords(UploadExcelFileForBulkSMSAdminSideViewModel model)
    {
        #region Model State Validation 

        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
            return View(model);
        }

        #endregion

        var res = await _bulkSmsService.UploadExcelFileJustForAddUsersRecords(model);
        if (res)
        {

            TempData[SuccessMessage] = "عملیات باموفقیت انجام شده است.";
            return RedirectToAction(nameof(ListOfAllBulkSMSRecords));
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return View(model);
    }


    #endregion

    #region Send SMS Fro All Bulk SMS

    //Send SMS Fro All Bulk SMS
    public async Task<IActionResult> SendSMSForAllBulkSMS(ulong id)
    {
        var res = await _bulkSmsService.SendSMSForAllBulkSMS(id);
        if (res)
        {

            TempData[SuccessMessage] = "پیامک شما با موفقیت ارسال گردید.";
            return RedirectToAction(nameof(ListOfAllBulkSMSRecords));
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction(nameof(ListOfAllBulkSMSRecords));
    }

    #endregion

    #region Delete Bulk SMS Record

    //Delete Bulk SMS Record
    public async Task<IActionResult> DeleteBulkSMSRecord(ulong id)
    {
        var res = await _bulkSmsService.DeleteBulkSMSRecord(id);
        if (res)
        {

            TempData[SuccessMessage] = "پیامک شما با موفقیت ارسال گردید.";
            return RedirectToAction(nameof(ListOfAllBulkSMSRecords));
        }

        TempData[ErrorMessage] = "اطلاعات وارد شده صحیح نمی باشد.";
        return RedirectToAction(nameof(ListOfAllBulkSMSRecords));
    }

    #endregion

    #endregion
}
