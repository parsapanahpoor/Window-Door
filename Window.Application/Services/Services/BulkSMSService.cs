#region Usings

using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Interfaces;
using Window.Application.Services.Implementation;
using Window.Application.Services.Interfaces;
using Window.Application.StaticTools;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.BulkSMS;
using Window.Domain.ViewModels.Admin.BulkSMS;

namespace Window.Application.Services.Services;

#endregion

public class BulkSMSService : IBulkSMSService
{
    #region Ctor

    private readonly WindowDbContext _context;
    private static readonly HttpClient client = new HttpClient();

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
    public async Task<List<BulkSMSResultViewModel>?> UploadSellersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model)
    {
        List<BulkSMS> bulkSMS = new List<BulkSMS>();
        List<BulkSMSResultViewModel> returnModel = new List<BulkSMSResultViewModel>();

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
                            Mobile = workSheet.Cells[row, 2].Value.ToString().Trim(),
                            SMSText = model.SMSText
                        });

                        BulkSMSResultViewModel modelChild = new BulkSMSResultViewModel()
                        {
                            Usernames = workSheet.Cells[row, 1].Value.ToString().Trim(),
                            UserMobiles = workSheet.Cells[row, 2].Value.ToString().Trim(),
                        };

                        returnModel.Add(modelChild);
                    }

                    #region Add List To the Data Base

                    await _context.BulkSMS.AddRangeAsync(bulkSMS);
                    await _context.SaveChangesAsync();

                    #endregion

                    return returnModel;

                }
                else
                {
                    return null;
                }
            }
        }

        return null;
    }

    //Upload Customer Excel File And Send SMS
    public async Task<List<BulkSMSResultViewModel>?> UploadCustomersExcelFileAndSendSMS(UploadExcelFileAdminSideViewModel model)
    {
        List<BulkSMS> bulkSMS = new List<BulkSMS>();
        List<BulkSMSResultViewModel> returnModel = new List<BulkSMSResultViewModel>();

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
                            SMSText = model.SMSText
                        });

                        BulkSMSResultViewModel modelChild = new BulkSMSResultViewModel()
                        {
                            Usernames = workSheet.Cells[row, 1].Value.ToString().Trim(),
                            UserMobiles = workSheet.Cells[row, 2].Value.ToString().Trim(),
                        };

                        returnModel.Add(modelChild);
                    }

                    #region Add List To the Data Base

                    await _context.BulkSMS.AddRangeAsync(bulkSMS);
                    await _context.SaveChangesAsync();

                    #endregion

                    return returnModel;
                }
                else
                {
                    return null;
                }
            }
        }

        return null;
    }

    //Upload Excel File Just For Add Users Records
    public async Task<bool> UploadExcelFileJustForAddUsersRecords(UploadExcelFileForBulkSMSAdminSideViewModel model)
    {
        List<AllBulkSMS> bulkSMS = new List<AllBulkSMS>();

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
                        var username = workSheet.Cells[row, 1].Value.ToString().Trim();
                        var mobile = workSheet.Cells[row, 2].Value.ToString().Trim();

                        bulkSMS.Add(new AllBulkSMS()
                        {
                            CreateDate = DateTime.Now,
                            IsDelete = false,
                            Username = username,
                            Mobile = mobile,
                            CountOfSentSMS = 0,
                            IsUserRegistered = ((await _context.Users.AnyAsync(p => !p.IsDelete && p.Mobile == mobile)) ? true : false),
                            LastestSMSSent = null,
                        });

                    }

                    #region Add List To the Data Base

                    await _context.AllBulkSMSs.AddRangeAsync(bulkSMS);
                    await _context.SaveChangesAsync();

                    #endregion

                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    //List Of All Bulk SMS Records
    public async Task<List<AllBulkSMS>> ListOfAllBulkSMSRecords()
    {
        return await _context.AllBulkSMSs
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .ToListAsync();
    }

    //Send SMS For All Bulk SMS
    public async Task<bool> SendSMSForAllBulkSMS(ulong bulkSMSRecordeId)
    {
        #region Get Bulk SMS Record 

        var bulkSMS = await _context.AllBulkSMSs
                                    .FirstOrDefaultAsync(p => !p.IsDelete && p.Id == bulkSMSRecordeId);
        if (bulkSMS == null) return false;

        #endregion

        #region Update Record 

        bulkSMS.CountOfSentSMS = bulkSMS.CountOfSentSMS + 1;
        bulkSMS.LastestSMSSent = DateTime.Now;

        _context.AllBulkSMSs.Update(bulkSMS);
        await _context.SaveChangesAsync();

        #endregion

        #region Send Verification Code SMS

        var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={bulkSMS.Mobile}&token={bulkSMS.Username}&template=UserTanks";
        var results = client.GetStringAsync(result);

        #endregion

        return true;
    }

    //Delete Bulk SMS Record
    public async Task<bool> DeleteBulkSMSRecord(ulong id)
    {
        #region Get Bulk SMS Record 

        var bulkSMS = await _context.AllBulkSMSs
                                    .FirstOrDefaultAsync(p => !p.IsDelete && p.Id == id);
        if (bulkSMS == null) return false;

        #endregion

        #region Delete Record

        bulkSMS.IsDelete = true;

        _context.AllBulkSMSs.Update(bulkSMS);
        await _context.SaveChangesAsync();

        #endregion

        return true;
    }

    #endregion
}
