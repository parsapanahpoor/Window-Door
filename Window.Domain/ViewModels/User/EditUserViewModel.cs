using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.User
{
    public class EditUserViewModel 
    {
        #region properties
        public ulong Id { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(200)]
        public string Username { get; set; }

        [MaxLength(200)]
        [DisplayName("Password")]
        public string? Password { get; set; }

        [MaxLength(200)]
        [DisplayName("Mobile")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Mobile { get; set; }

        [DisplayName("Avatar")]
        public string? Avatar { get; set; }

        [Display(Name = "انتخاب نقش های کاربر")]
        public List<ulong>? UserRoles { get; set; }

        #endregion

        #region relations



        #endregion
    }
}