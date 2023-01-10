using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Window.Domain.ViewModels.User.Account;

public class EditProfileViewModel
{
    #region Properties

    [DisplayName("Username")]
    [Required(ErrorMessage = "Please Enter {0}")]
    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    public string Username { get; set; }

    [DisplayName("Mobile")]
    [Required(ErrorMessage = "Please Enter {0}")]
    [MaxLength(200, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    public string Mobile { get; set; }

    [AllowNull]
    public string? AvatarName { get; set; }

    [DisplayName("Avatar")]
    [AllowNull]
    public IFormFile? Avatar { get; set; }

    public string ShopName { get; set; }

    #endregion
}

public enum EditProfileResponse
{
    Success, UserNotFound, ImageIsNotValid
}