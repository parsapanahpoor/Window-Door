using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.ViewModels;
using Window.Domain.ViewModels.Admin.Wallet;
using Window.Domain.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Window.Domain.ViewModels.User.Authentication;
using Window.Domain.ViewModels.User.Account;
using Window.Domain.ViewModels.Common;
using Window.Application.Generators;
using Window.Application.Security;
using Window.Domain.ViewModels.Site.Account;
using Window.Domain.ViewModels.Account;

namespace Window.Application.Interfaces
{
    public interface IUserService
    {
        #region Authentication

        //Check That Is User Admin Or Not 
        Task<bool> CheckThatIsUserAdminOrNot(ulong userId);

        Task<ResetPasswordResult> ResetUserPassword(ResetPasswordViewModel pass, string mobile);

        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserViewModel model);

        Task<LoginUserResponse> LoginUserAsync(LoginUserViewModel model);

        Task<List<SelectListViewModel>> GetSelectRolesList();

        #endregion

        #region user account

        Task<UserPanelEditUserInfoResult> EditUserInfoInUserPanel(UserPanelEditUserInfoViewModel edit, IFormFile? UserAvatar);

        Task<ForgotPasswordResult> RecoverUserPassword(ForgetPasswordViewModel forgot);

        Task<ActiveMobileByActivationCodeResult> ActiveUserMobile(ActiveMobileByActivationCodeViewModel activeMobileByActivationCodeViewModel);

        Task<UserPanelEditUserInfoViewModel> FillUserPanelEditUserInfoViewModel(ulong userId);

        Task ResendActivationCodeSMS(string Mobile);

        Task<bool> IsExistUserByMobile(string mobile);

        Task<User?> GetUserByEmailActivationCode(string emailActivationCode);

        Task<EditProfileViewModel?> GetUserProfileForEditAsync(ulong userId);

        Task<ChangeUserPasswordResponse> ChangeUserPasswordAsync(ulong userId, ChangeUserPasswordViewModel model);

        Task<AddNewUserResult> CreateUser(AddUserViewModel user, IFormFile avatar);

        Task<EditUserResult> EditUser(EditUserViewModel user, IFormFile avatar);

        Task<EditProfileResponse> EditProfileAsync(ulong userId, EditProfileViewModel model);

        Task<EditUserViewModel> GetUserForEdit(ulong userId);

        Task<bool> RemoveUserById(ulong userId);

        Task<User> GetUserById(ulong userId);

        Task<User> GetUserByEmail(string email);

        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);

        List<User> GetAllUsers();

        Task<FilterUserViewModel> FilterUsersForSellerPanel(FilterUserViewModel filter, ulong masterId);

        Task<AddNewUserResult> CreateUserFromSellerPanel(AddUserViewModel user, IFormFile avatar, ulong masterId);

        #endregion
    }
}
