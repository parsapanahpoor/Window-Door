using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Common;
using Window.Domain.Entities.Location;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Enums.SellerType;

namespace Window.Domain.Entities.Market
{
    public class MarketPersonalInfo : BaseEntity
    {
        #region properties

        public ulong UserId { get; set; }

        public ulong MarketId { get; set; }

        public int NationalCode { get; set; }

        public string PhotoOfBusinessLicense { get; set; }

        public string PhotoOfNationalCode { get; set; }

        public string CompanyName { get; set; }

        public ulong? CountryId { get; set; }

        public ulong? StateId { get; set; }

        public ulong? CityId { get; set; }

        public string CompanyLogo { get; set; }

        public string Resume { get; set; }

        public string PeriodTimesOfWork { get; set; }

        public string WorkAddress { get; set; }

        public string? RejectDescription { get; set; }

        public MarketPersonalsInfoState MarketPersonalsInfoState { get; set; }

        public SellerType SellerType { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        public Market Market { get; set; }

        [InverseProperty("UserCountries")]
        [ForeignKey("CountryId")]
        public State? Country { get; set; }

        [InverseProperty("UserStates")]
        [ForeignKey("StateId")]
        public State? State { get; set; }

        [InverseProperty("UserCities")]
        [ForeignKey("CityId")]
        public State? City { get; set; }

        #endregion
    }

    public enum MarketPersonalsInfoState
    {
        [Display(Name = "اطلاعات تایید شده ولی مبلغ شارژ پرداخت نشده است .")]
        AcceptedPersonalInformation,
        [Display(Name = "تایید نشده")]
        Rejected,
        [Display(Name = "در انتظار تایید اطلاعات شخصی")]
        WaitingForConfirmPersonalInformations,
        [Display(Name = "در انتظار پرداخت مبالغ")]
        WaitingForPyedFromSeller,
        [Display(Name = "تایید شده و فعال")]
        ActiveMarketAccount,
        [Display(Name = "غیر فعال")]
        DisAcctiveMarketAccount,
        [Display(Name = "در انتظار تکمیل اطلاعات فروشنده")]
        WaitingForCompleteInfoFromSeller
    }
}
