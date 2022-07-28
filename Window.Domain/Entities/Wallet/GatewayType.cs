using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Window.Domain.Entities.Wallet;

public enum GatewayType
{
    [Display(Name = "ZarinPal")]
    Zarinpal = 0,

    [Display(Name = "System")]
    System = 1,
    
    [Display(Name = "PayPal")]
    PayPal = 2,

    [Display(Name = "Stripe")]
    Stripe = 3 
}