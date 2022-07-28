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

        public FilterUserOrderType OrderType { get; set; }

        #endregion
        public enum FilterUserOrderType
        {
            [Display(Name = "Ascending")]
            CreateDate_ASC,
            [Display(Name = "Descending")]
            CreateDate_DES,
        }

    }
}
