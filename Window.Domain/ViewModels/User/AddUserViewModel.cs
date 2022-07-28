using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels
{
    public class AddUserViewModel
    {
        #region properties

        [DisplayName("Username")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(200)]
        public string Username { get; set; }

        [MaxLength(200)]
        [DisplayName("Email")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Email { get; set; }

        [MaxLength(200)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Password { get; set; }

        [MaxLength(200)]
        [DisplayName("Mobile")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Mobile { get; set; }

        [DisplayName("Avatar")]
        public string? Avatar { get; set; }

        #endregion

    }


}
public enum AddNewUserResult
{
    DuplicateEmail,
    DuplicateMobileNumber,
    Success,
}
public enum EditUserResult
{
    DuplicateEmail,
    DuplicateMobileNumber,
    Success,
    Error
}