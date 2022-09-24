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
        public string UserAvatar { get; set; }

        public string UserName { get; set; }

        public ulong UserId { get; set; }

        public string BrandImage { get; set; }

        public string BrandName { get; set; }

        public double? Price { get; set; }

        public int? Score { get; set; }
    }
}
