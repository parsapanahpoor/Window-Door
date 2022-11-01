using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.BrandType
{
    public enum BrandType
    {
        [Display(Name ="همه")]
        All,

        [Display(Name = "UPVC")]
        UPVC,

        [Display(Name = "آلمینیوم")]
        Alminum,

        [Display(Name = "یراق UPVC و آلمینیوم")]
        Yaragh
    }
}
