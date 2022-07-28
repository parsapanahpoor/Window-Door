using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.Types
{
    public enum ProductType
    {
        [Display(Name = "کشویی")]
        Keshoie = 0,
        [Display(Name = "لولایی")]
        Lolaie = 1
    }
}
