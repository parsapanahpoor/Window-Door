#region Usings

using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.BulkSMS;
using Window.Domain.ViewModels.Admin.BulkSMS;

namespace Window.Application.Services.Services;

#endregion

public class BulkSMSService : IBulkSMSService
{
    #region Ctor

    private readonly WindowDbContext _context;

    public BulkSMSService(WindowDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Admin Side 

    //List OF Customer Sent SMS
    public async Task<List<BulkSMS>> ListOFCustomerSentSMS()
    {
        return await _context.BulkSMS
                             .AsNoTracking()
                             .Where(p => !p.IsDelete && p.BulkSMSTargetPersonType == BulkSMSTargetPersonType.Customer && p.IsSent)
                             .ToListAsync();
    }

    //List OF Seller Sent SMS
    public async Task<List<BulkSMS>> ListOFSellerSentSMS()
    {
        return await _context.BulkSMS
                             .AsNoTracking()
                             .Where(p => !p.IsDelete && p.BulkSMSTargetPersonType == BulkSMSTargetPersonType.Seller && p.IsSent)
                             .ToListAsync();
    }

    //Upload Sellers Excel File And Send SMS
    public async Task<bool> UploadSellersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model)
    {
        List<BulkSMS> bulkSMS = new List<BulkSMS>();

        using (var stream = new MemoryStream())
        {
            await model.ExcelFile.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                var rowCount = workSheet.Dimension.Rows;

                if (rowCount >= 2)
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        bulkSMS.Add(new BulkSMS()
                        {
                            CreateDate = DateTime.Now,
                            IsDelete = false,
                            IsSent = true,
                            BulkSMSTargetPersonType = BulkSMSTargetPersonType.Seller,
                            Username = workSheet.Cells[row, 1].Value.ToString().Trim(),
                            Mobile= workSheet.Cells[row, 2].Value.ToString().Trim(),
                        }); 
                    }

                    #region Add List To the Data Base

                    await _context.BulkSMS.AddRangeAsync(bulkSMS);
                    await _context.SaveChangesAsync();

                    #endregion
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    //Upload Customer Excel File And Send SMS
    public async Task<bool> UploadCustomersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model)
    {
        List<BulkSMS> bulkSMS = new List<BulkSMS>();

        using (var stream = new MemoryStream())
        {
            await model.ExcelFile.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                var rowCount = workSheet.Dimension.Rows;

                if (rowCount >= 2)
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        bulkSMS.Add(new BulkSMS()
                        {
                            CreateDate = DateTime.Now,
                            IsDelete = false,
                            IsSent = true,
                            BulkSMSTargetPersonType = BulkSMSTargetPersonType.Customer,
                            Username = workSheet.Cells[row, 1].Value.ToString().Trim(),
                            Mobile = workSheet.Cells[row, 2].Value.ToString().Trim(),
                        });
                    }

                    #region Add List To the Data Base

                    await _context.BulkSMS.AddRangeAsync(bulkSMS);
                    await _context.SaveChangesAsync();

                    #endregion
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    #endregion
}
