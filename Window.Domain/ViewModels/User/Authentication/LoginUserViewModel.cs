using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Window.Domain.ViewModels.User.Authentication;

public class LoginUserViewModel
{
    #region Properties

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DisplayName("موبایل")]
    public string Mobile { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
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