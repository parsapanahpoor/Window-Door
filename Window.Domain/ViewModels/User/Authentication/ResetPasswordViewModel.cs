using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Window.Domain.ViewModels.User.Authentication;

public class ResetPasswordViewModel
{
    #region Properties

    public string? EmailActivationCode { get; set; }

    [DisplayName("New Password")]
    [Required(ErrorMessage = "Please Enter {0}")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DisplayName("Re New Password")]
    [Required(ErrorMessage = "Please Enter {0}")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Password And Re Password Does Not Match")]
    public string ReNewPassword { get; set; }

    #endregion
}

public enum ResetPasswordResponse
{
    Success, UserNotFound, UserIsNotActive
}