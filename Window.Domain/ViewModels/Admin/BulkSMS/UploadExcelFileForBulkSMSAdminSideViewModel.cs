#region Usings

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Window.Domain.ViewModels.Admin.BulkSMS;

#endregion

public class UploadExcelFileForBulkSMSAdminSideViewModel
{
    #region properties

    [Display(Name = "فایل اکسل")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public IFormFile ExcelFile { get; set; }

    #endregion
}
