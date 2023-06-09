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
    Task<bool> UploadSellersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model);

    //List OF Customer Sent SMS
    Task<List<BulkSMS>> ListOFCustomerSentSMS();

    //Upload Customer Excel File And Send SMS
    Task<bool> UploadCustomersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model);

    #endregion
}
