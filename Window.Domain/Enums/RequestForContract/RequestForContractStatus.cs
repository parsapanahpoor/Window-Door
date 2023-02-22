using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Window.Domain.Enums.RequestForContract
{
    public enum RequestForContractStatus
    {
        [Display(Name = "درخواست قرارداد")] Request = 0,
        [Display(Name = "عقد قرارداد")] AcceptedRequest = 1,
        [Display(Name = "اندازه گیری ساخت")] ProcessingMaterial = 2,
        [Display(Name = "تولید و مونتاژ")] ProcessingProduct = 3,
        [Display(Name = "نصب و آبگیری")] Installing = 4,
        [Display(Name = "تحویل پروژه")] SendToCustomer = 5,
    }
}
