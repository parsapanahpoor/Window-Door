﻿using System;
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

        public string? ShopName { get; set; }

        public string? SellerPersonalVideo { get; set; }

        public string? SellerPersonalBanner { get; set; }
    }

    public class brandInquiryViewModel
    {
        public string brandName { get; set; }

        public string BreandLogo { get; set; }
    }

    public class UserInquiryViewModel
    {
        public string UserAvatar { get; set; }

        public string Username { get; set; }
    }
}
