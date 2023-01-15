using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Domain.Entities.Account;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels;
using Window.Domain.ViewModels.User;
using Window.Domain.ViewModels.User.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Window.Domain.ViewModels.User.Account;
using Microsoft.Extensions.Configuration;
using Window.Domain.Entities.Wallet;
using Window.Application.Generators;
using Window.Application.StticTools;
using Window.Data.Context;
using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Common;
using Window.Domain.Entities.Market;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Implementation;

namespace Window.Application.Services
{
    public class UserService : IUserService
    {
        #region Ctor

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IWalletService _walletService;
        private readonly WindowDbContext _context;
        private readonly IViewRenderService _viewRenderService;
        private IEmailSender _emailSender;

        public UserService(IConfiguration configuration, IUserRepository userRepository, IWalletService walletService, WindowDbContext context, IEmailSender emailSender , IViewRenderService viewRenderService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _walletService = walletService;
            _context = context;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
        }

        #endregion

        #region Authentication

        public async Task<bool> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            // get user by activation code
            var user = await GetUserByEmailActivationCode(resetPassword.EmailActivationCode);

            // check user exists
            if (user == null) return false;

            // hash password
            var password = PasswordHelper.EncodePasswordMd5(resetPassword.NewPassword.SanitizeText());

            // update user
            user.Password = password;
            user.EmailActivationCode = CodeGenerator.GenerateUniqCode();

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<SelectListViewModel>> GetSelectRolesList()
        {
            return await _context.Roles.Where(s => !s.IsDelete).Select(s => new SelectListViewModel
            {
                Id = s.Id,
                Title = s.Title
            }).ToListAsync();
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserViewModel model)
        {
            if (await _userRepository.IsEmailExist(model.Email.Trim().ToLower()))
            {
                return RegisterUserResponse.EmailExist;
            }

            if (await _userRepository.IsMobileExist(model.Mobile.Trim().ToLower()))
            {
                return RegisterUserResponse.MobileExist;
            }

            var user = new User
            {
                Mobile = model.Mobile.Trim().ToLower(),
                Username = model.Username,
                Password = PasswordHelper.EncodePasswordMd5(model.Password),
                EmailActivationCode = CodeGenerator.GenerateUniqCode(),
                CreateDate = DateTime.Now,
                IsAdmin = false,
            };

            await _userRepository.CreateUser(user);

            return RegisterUserResponse.Success;
        }

        public async Task<ResetPasswordViewModel> GetResetPasswordViewModel(string emailActivationCode)
        {
            // get user by activation code
            var user = await GetUserByEmailActivationCode(emailActivationCode.SanitizeText());

            // check user exists
            if (user == null) return null;

            return new ResetPasswordViewModel()
            {
                EmailActivationCode = user.EmailActivationCode
            };
        }

        public async Task<User?> GetUserByEmailActivationCode(string emailActivationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(s =>
                s.EmailActivationCode == emailActivationCode.SanitizeText());
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserViewModel model)
        {
            if (!await _userRepository.IsEmailExist(model.Email.Trim().ToLower()))
            {
                return LoginUserResponse.EmailNotFound;
            }

            if (!await _userRepository.IsUserActive(model.Email.Trim().ToLower()))
            {
                return LoginUserResponse.UserNotActive;
            }

            if (!await _userRepository.IsPasswordValid(model.Email.Trim().ToLower(),
                    PasswordHelper.EncodePasswordMd5(model.Password)))
            {
                return LoginUserResponse.WrongPassword;
            }

            return LoginUserResponse.Success;
        }

        #endregion

        #region User

        public async Task<bool> ForgotPasswordUser(ForgotPasswordViewModel forgotPassword)
        {
            // get user by email
            var user = await GetUserByEmail(forgotPassword.Email.SanitizeText());

            // check user exists
            if (user == null) return false;

            #region Send Email

            var emailViewModel = new EmailViewModel
            {
                EmailActivationCode = user.EmailActivationCode,
                ButtonName = "فعالسازی حساب کاربری",
                FullName = user.Username,
                Description = $"{user.Username} عزیز لطفا جهت بازیابی کلمه عبور روی لینک زیر کلیک کنید .",
                ButtonLink = $"{FilePaths.SiteAddress}/ResetPassword/{user.EmailActivationCode}",
                EmailBanner = string.Empty,
            };

            string body = _viewRenderService.RenderToStringAsync("_Email", emailViewModel);

            await _emailSender.SendEmail(forgotPassword.Email.SanitizeText(), "بازیابی کلمه عبور", body);

            #endregion

            return true;
        }

        public Task<EditProfileViewModel?> GetUserProfileForEditAsync(ulong userId)
        {
            return _userRepository.GetUserProfileForEditAsync(userId);
        }

        public async Task<ChangeUserPasswordResponse> ChangeUserPasswordAsync(ulong userId, ChangeUserPasswordViewModel model)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return ChangeUserPasswordResponse.UserNotFound;
            }

            if (user.Password != PasswordHelper.EncodePasswordMd5(model.CurrentPassword))
            {
                return ChangeUserPasswordResponse.WrongPassword;
            }

            user.Password = PasswordHelper.EncodePasswordMd5(model.NewPassword);
            await _userRepository.EditUser(user, userId);

            return ChangeUserPasswordResponse.Success;
        }

        public async Task<AddNewUserResult> CreateUser(AddUserViewModel user, IFormFile avatar)
        {
            if (await _userRepository.IsMobileExist(user.Mobile))
            {
                return AddNewUserResult.DuplicateMobileNumber;

            }

            var newUser = new User()
            {
                Username = user.Username,
                CreateDate = DateAndTime.Now,
                Mobile = user.Mobile.SanitizeText(),
                Password = PasswordHelper.EncodePasswordMd5(user.Password).SanitizeText(),
                IsAdmin = false,
            };

            if (avatar != null && avatar.IsImage())
            {
                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(avatar.FileName);
                avatar.AddImageToServer(imageName, FilePaths.UserAvatarPathServer, 270, 270, FilePaths.UserAvatarPathThumbServer);
                newUser.Avatar = imageName;
            }

            await _userRepository.CreateUser(newUser);
            return AddNewUserResult.Success;
        }

        public async Task<AddNewUserResult> CreateUserFromSellerPanel(AddUserViewModel user, IFormFile avatar, ulong masterId)
        {
            #region Model State Validation 

            if (await _userRepository.IsMobileExist(user.Mobile))
            {
                return AddNewUserResult.DuplicateMobileNumber;
            }

            #endregion

            #region Add New User

            var newUser = new User()
            {
                Username = user.Username,
                CreateDate = DateAndTime.Now,
                Mobile = user.Mobile.SanitizeText(),
                EmailActivationCode = CodeGenerator.GenerateUniqCode(),
                Password = PasswordHelper.EncodePasswordMd5(user.Password).SanitizeText(),
                IsAdmin = false,
            };

            if (avatar != null && avatar.IsImage())
            {
                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(avatar.FileName);
                avatar.AddImageToServer(imageName, FilePaths.UserAvatarPathServer, 270, 270, FilePaths.UserAvatarPathThumbServer);
                newUser.Avatar = imageName;
            }

            await _userRepository.CreateUser(newUser);

            #endregion

            #region Get Market

            var marlet = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == masterId);
            if (marlet == null) return AddNewUserResult.DuplicateEmail;

            #endregion

            #region Add New User Market

            MarketUsers marketUsers = new MarketUsers()
            {
                UserId = newUser.Id,
                MarketId = marlet.Id,
                RoleId = 3
            };

            await _context.MarketUser.AddAsync(marketUsers);
            await _context.SaveChangesAsync();

            #endregion

            #region Add User Role

            UserRole userRole = new UserRole()
            {
                RoleId = 3,
                UserId = newUser.Id
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            #endregion

            return AddNewUserResult.Success;
        }

        public async Task<EditUserResult> EditUser(EditUserViewModel user, IFormFile avatar)
        {
            var userOldInfos = await _userRepository.GetUserById(user.Id);

            if (await _userRepository.IsMobileExist(user.Mobile) && user.Mobile != userOldInfos.Mobile)
            {
                return EditUserResult.DuplicateMobileNumber;
            }

            var editedUser = await _userRepository.GetUserById(user.Id);

            if (editedUser != null)
            {
                editedUser.Username = user.Username;
                editedUser.Mobile = user.Mobile.SanitizeText();

                if (user.Password != null)
                {
                    editedUser.Password = user.Password.SanitizeText();
                }

                if (avatar != null && avatar.IsImage())
                {
                    if (!string.IsNullOrEmpty(editedUser.Avatar))
                    {
                        editedUser.Avatar.DeleteImage(FilePaths.UserAvatarPathServer, FilePaths.UserAvatarPathThumbServer);
                    }

                    var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(avatar.FileName);
                    avatar.AddImageToServer(imageName, FilePaths.UserAvatarPathServer, 270, 270, FilePaths.UserAvatarPathThumbServer);
                    editedUser.Avatar = imageName;
                }

                await _userRepository.EditUser(editedUser, user.Id);


                #region Delete User Rols

                var roles = await _context.UserRoles.Where(s => !s.IsDelete && s.UserId == user.Id).ToListAsync();

                _context.RemoveRange(roles);

                #endregion

                #region Add User Roles

                if (user.UserRoles != null && user.UserRoles.Any())
                {
                    foreach (var roleId in user.UserRoles)
                    {
                        var userRole = new UserRole()
                        {
                            RoleId = roleId,
                            UserId = user.Id
                        };
                        await _context.AddAsync(userRole);
                    }

                    await _context.SaveChangesAsync();
                }

                #endregion

                return EditUserResult.Success;
            }

            return EditUserResult.Error;

        }

        public async Task<EditProfileResponse> EditProfileAsync(ulong userId, EditProfileViewModel model)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return EditProfileResponse.UserNotFound;
            }

            #region Avatar

            if (model.Avatar != null && model.Avatar.IsImage())
            {
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    user.Avatar.DeleteImage(FilePaths.UserAvatarPathServer, FilePaths.UserAvatarPathThumbServer);
                }

                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(model.Avatar.FileName);
                model.Avatar.AddImageToServer(imageName, FilePaths.UserAvatarPathServer, 270, 270, FilePaths.UserAvatarPathThumbServer);
                user.Avatar = imageName;
            }

            #endregion

            #region Edit Properties

            user.Username = model.Username;
            user.Mobile = model.Mobile;
            user.ShopName = model.ShopName;

            #endregion

            await _userRepository.EditUser(user, user.Id);
            return EditProfileResponse.Success;
        }

        public async Task<EditUserViewModel> GetUserForEdit(ulong userId)
        {
            var user = await GetUserById(userId);

            var userRoleIds = await _context.UserRoles
                    .Where(s => !s.IsDelete && s.UserId == userId)
                    .Select(s => s.RoleId)
                    .ToListAsync();

            return new EditUserViewModel()
            {
                Username = user.Username,
                Mobile = user.Mobile,
                Avatar = user.Avatar,
                UserRoles = userRoleIds
            };
        }

        public Task<User?> GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email.Trim().ToLower());
        }

        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter)
        {
            return await _userRepository.FilterUsers(filter);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task<FilterUserViewModel> FilterUsersForSellerPanel(FilterUserViewModel filter, ulong masterId)
        {
            return await _userRepository.FilterUsersForSellerPanel(filter, masterId);
        }

        public async Task<User?> GetUserById(ulong userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<bool> RemoveUserById(ulong userId)
        {
            return await _userRepository.RemoveUserById(userId);
        }

        #endregion

        #region Utils

        private string GenerateUserActivationCode()
        {
            return Guid.NewGuid().ToString();
        }

        #endregion
    }
}