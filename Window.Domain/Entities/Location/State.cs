using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Log;
using Window.Domain.Entities.Market;

namespace Window.Domain.Entities.Location
{
    public class State : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "نام یکتا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string UniqueName { get; set; }

        public ulong? ParentId { get; set; }

        #endregion

        #region Relations

        public State? Parent { get; set; }

        public ICollection<State> Children { get; set; }

        [InverseProperty("Country")]
        public ICollection<MarketPersonalInfo> UserCountries { get; set; }

        [InverseProperty("State")]
        public ICollection<MarketPersonalInfo> UserStates { get; set; }

        [InverseProperty("City")]
        public ICollection<MarketPersonalInfo> UserCities { get; set; }

        [InverseProperty("Country")]
        public ICollection<LogForInquiry> LogCountries { get; set; }

        [InverseProperty("State")]
        public ICollection<LogForInquiry> LogStates { get; set; }

        [InverseProperty("City")]
        public ICollection<LogForInquiry> LogCities { get; set; }

        #endregion
    }
}
