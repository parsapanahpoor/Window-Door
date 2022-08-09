using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Window.Domain.ViewModels.User.Authentication;

public class RegisterUserViewModel
{

    #region Properties

    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DisplayName("Username")]
    public string Username { get; set; }

    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
    [DisplayName("Email")]
    public string Email { get; set; }

    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DisplayName("Re Password")]
    [Compare("Password", ErrorMessage = "Password And Re Password Does Not Match")]
    [DataType(DataType.Password)]
    public string RePassword { get; set; }

    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DisplayName("Mobile")]
    public string Mobile { get; set; }

    #endregion

}

public enum RegisterUserResponse
{
    EmailExist, MobileExist, Success
}