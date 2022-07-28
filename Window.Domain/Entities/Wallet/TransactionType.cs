using System.ComponentModel.DataAnnotations;

namespace Window.Domain.Entities.Wallet;

public enum TransactionType
{
    [Display(Name = "Deposit")]
    Deposit = 0,

    [Display(Name = "Withdraw")]
    Withdraw = 1,
}