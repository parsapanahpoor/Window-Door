using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Enums.Types
{
    public enum ProductKind
    {
        [Display(Name = "درب")]
        Door,
        [Display(Name = "پنجره")]
        Window
    }
}
