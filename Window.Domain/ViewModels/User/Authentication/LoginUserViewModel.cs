using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Window.Domain.ViewModels.User.Authentication;

public class LoginUserViewModel
{
    #region Properties

    [Required(ErrorMessage = "Please Enter {0}")]
    [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
    [DisplayName("Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please Enter {0}")]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    #endregion
}

public enum LoginUserResponse
{
    Success, UserNotActive, WrongPassword, EmailNotFound
}