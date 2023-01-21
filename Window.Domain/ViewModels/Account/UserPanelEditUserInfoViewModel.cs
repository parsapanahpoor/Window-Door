using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.User.Account
{
    public class UserPanelEditUserInfoViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(150, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        public string username { get; set; }

        public ulong UserId { get; set; }

        public string? AvatarName { get; set; }

        [Display(Name = "Mobile")]
        [MaxLength(20, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The information entered is not valid.")]
        public string? Mobile { get; set; }

    }

    public enum UserPanelEditUserInfoResult
    {
        NotValidImage,
        UserNotFound,
        Success,
        NotValidEmail,
        NationalId,
        NotValidNationalId
    }
}
