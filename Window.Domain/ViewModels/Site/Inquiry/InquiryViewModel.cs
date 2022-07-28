using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class InquiryViewModel
    {
        public Domain.Entities.Account.User User { get; set; }

        public MainBrand MainBrand { get; set; }

        public int? Price { get; set; }

        public int? Score { get; set; }
    }
}
