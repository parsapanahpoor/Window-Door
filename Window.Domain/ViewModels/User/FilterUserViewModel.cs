using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.User
{
    public class FilterUserViewModel : BasePaging<Entities.Account.User>
    {
        #region properties

        public string Username { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public bool IsDelete  { get; set; }

        [RegularExpression(@"^\d{4}\/(0?[1-9]|1[012])\/(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "The entered date is not valid")]
        public string? FromDate { get; set; }

        [RegularExpression(@"^\d{4}\/(0?[1-9]|1[012])\/(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "The entered date is not valid")]
        public string? ToDate { get; set; }
        public FilterUserOrderType OrderType { get; set; }

        #endregion
        public enum FilterUserOrderType
        {
            [Display(Name = "Descending")]
            CreateDate_DES,
            [Display(Name = "Ascending")]
            CreateDate_ASC,
        }

    }
}
