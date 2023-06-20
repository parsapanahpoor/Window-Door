#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.BulkSMS;
using Window.Domain.ViewModels.Admin.BulkSMS;

namespace Window.Application.Services.Interfaces;

#endregion

public interface IBulkSMSService
{
    #region Admin Side

    //List OF Seller Sent SMS
    Task<List<BulkSMS>> ListOFSellerSentSMS();

    //Upload Sellers Excel File And Send SMS
    Task<List<BulkSMSResultViewModel>?> UploadSellersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model);

    //List OF Customer Sent SMS
    Task<List<BulkSMS>> ListOFCustomerSentSMS();

    //Upload Customer Excel File And Send SMS
    Task<List<BulkSMSResultViewModel>?> UploadCustomersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model);

    //Upload Excel File Just For Add Users Records
    Task<bool> UploadExcelFileJustForAddUsersRecords(UploadExcelFileForBulkSMSAdminSideViewModel model);

    //List Of All Bulk SMS Records
    Task<List<AllBulkSMS>> ListOfAllBulkSMSRecords();

    //Send SMS For All Bulk SMS
    Task<bool> SendSMSForAllBulkSMS(ulong bulkSMSRecordeId);

    //Delete Bulk SMS Record
    Task<bool> DeleteBulkSMSRecord(ulong id);

    #endregion
}
