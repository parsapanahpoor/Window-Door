﻿using System.ComponentModel.DataAnnotations;

namespace Window.Domain.Entities.Wallet;

public enum PaymentType
{
    [Display(Name = "Charge Wallet")]
    ChargeWallet = 0,

    [Display(Name = "Buy")]
    Buy = 1,

   
}