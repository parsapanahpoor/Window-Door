using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Window.Domain.ViewModels.User.Authentication;

public class ForgotPasswordViewModel
{
    #region Properties

    [Required(ErrorMessage = "Please Enter {0}")]
    [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
    [DisplayName("Email")]
    public string Email { get; set; }

    #endregion
}

public enum ForgotPasswordResponse
{
    Success, UserNotFound, UserIsNotActive
}