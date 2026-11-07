using System.ComponentModel.DataAnnotations;

namespace Window.Domain.ViewModels.User.Authentication;

public sealed class SmsLoginRequest
{
    [Required(ErrorMessage = "شمارهٔ موبایل را وارد کن.")]
    [StringLength(32, ErrorMessage = "شمارهٔ موبایل معتبر نیست.")]
    public string Mobile { get; set; } = "";
}

public sealed class SmsLoginCodeInput
{
    [Required(ErrorMessage = "کد ورود را وارد کن.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "کد ورود باید شش‌رقمی باشد.")]
    public string Code { get; set; } = "";
}

public sealed record SmsLoginCodeViewModel(string MobileHint, int SecondsUntilResend, string? ReturnUrl);
