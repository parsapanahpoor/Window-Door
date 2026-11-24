namespace Window.Domain.ViewModels.User.Authentication;

public sealed class SmsRegisterViewModel
{
    public string Mobile { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public string? MarketName { get; set; }

    public string? ReturnUrl { get; set; }
}
