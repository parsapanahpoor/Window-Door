using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.SellerType
{
    public enum SellerType
    {
        [Display(Name = "UPVC")]
        UPC,
        [Display(Name = " آلمینیوم")]
        Aluminium,
        [Display(Name = " UPVC و آلمینیوم")]
        UPVCAlminium
    }
}
