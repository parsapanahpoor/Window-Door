using Window.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Contact;
using Window.Domain.Entities.Market;

namespace Window.Domain.Entities.Account
{
    public  class User : BaseEntity
    {
        #region properties

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(300, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Username { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(20, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "موبایل وارد شده معتبر نمی باشد")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        public string Mobile { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(150, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست .")]
        public string? Email { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? Avatar { get; set; }

        public bool IsBan { get; set; } = false;

        public bool IsAdmin { get; set; } = false;

        public bool BanForTicket { get; set; }

        [Required]
        public int WalletBalance { get; set; }
        #endregion

        #region Relations

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<TicketMessage> TicketMessages { get; set; }

        public List<Wallet.Wallet> Wallets { get; set; }

        public List<MarketPersonalInfo> SellersPersonalInfos { get; set; }

        public List<MarketWorkSamle> SellerWorkSamles { get; set; }

        public List<MarketLinks> SellerLinks { get; set; }

        public Market.Market Market { get; set; }

        public ICollection<Article.Article> Articles { get; set; }

        public ICollection<MarketChargeInfo> MarketChargeInfos { get; set; }

        public ICollection<Product.Product> Product { get; set; }

        #endregion
    }
}
