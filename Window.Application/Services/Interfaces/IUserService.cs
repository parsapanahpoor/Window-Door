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

namespace Window.Application.Interfaces
{
    public interface IUserService
    {
        #region Authentication

        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserViewModel model);

        Task<LoginUserResponse> LoginUserAsync(LoginUserViewModel model);

        Task<List<SelectListViewModel>> GetSelectRolesList();

        #endregion

        #region user account

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
