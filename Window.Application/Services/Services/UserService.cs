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
using Microsoft.Data.SqlClient;
using Window.Domain.ViewModels.Common;
using Window.Domain.Entities.Market;
using Window.Application.Services.Interfaces;
using Window.Application.Services.Implementation;
using Window.Domain.ViewModels.Site.Account;
using Window.Domain.ViewModels.Account;
using Window.Application.StaticTools;
using Window.Application.Convertors;

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
        private static readonly HttpClient client = new HttpClient();
        private readonly ISMSService _smsservice;

        public UserService(IConfiguration configuration, IUserRepository userRepository, IWalletService walletService, WindowDbContext context
                                , IEmailSender emailSender, IViewRenderService viewRenderService, ISMSService smsservice)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _walletService = walletService;
            _context = context;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
            _smsservice = smsservice;
        }

        #endregion

        #region Authentication

        public async Task<ResetPasswordResult> ResetUserPassword(ResetPasswordViewModel pass, string mobile)
        {
            #region Get User By Mobile

            var user = await GetMobileCodeAccountAsync(mobile);
            if (user is null || user.IsDelete || user.IsBan)
                return ResetPasswordResult.NotFound;

            var code = MobileNumber.ToAsciiDigits(pass.ActiveCode);
            if (code.Length is < 5 or > 6 || !code.All(char.IsAsciiDigit))
                return ResetPasswordResult.WrongActiveCode;

            #endregion

            var issuedAfter = DateTime.Now.AddMinutes(-2);
            var password = PasswordHelper.EncodePasswordMd5(pass.Password.SanitizeText());
            var inactiveCode = Guid.NewGuid().ToString("N");
            var normalized = MobileNumber.Normalize(mobile);

            var changed = await _context.Users
                .Where(u => u.Id == user.Id && !u.IsDelete && !u.IsBan
                    && EF.Property<string>(u, MobileLoginKey.Name) == normalized
                    && !EF.Property<bool>(u, MobileLoginKey.Excluded)
                    && !_context.Users.Any(other => other.Id != u.Id && EF.Property<string>(other, MobileLoginKey.Name) == normalized)
                    && u.MobileActivationCode == code && u.ExpireMobileSMSDateTime > issuedAfter)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Password, password)
                    .SetProperty(u => u.IsMobileConfirm, true)
                    .SetProperty(u => u.MobileActivationCode, inactiveCode)
                    .SetProperty(u => u.ExpireMobileSMSDateTime, (DateTime?)null));

            return changed == 1 ? ResetPasswordResult.Success : ResetPasswordResult.WrongActiveCode;
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

            if (await _context.Users.AnyAsync(u => EF.Property<string>(u, MobileLoginKey.Name) == model.Mobile))
            {
                return RegisterUserResponse.MobileExist;
            }

            var user = new User
            {
                Mobile = model.Mobile.Trim().ToLower(),
                Username = model.Mobile,
                Password = PasswordHelper.EncodePasswordMd5(model.Password),
                EmailActivationCode = CodeGenerator.GenerateUniqCode(),
                CreateDate = DateTime.Now,
                IsAdmin = false,
                MobileActivationCode = new Random().Next(10000, 999999).ToString(),
                ExpireMobileSMSDateTime = DateTime.Now
            };

            var campaign = await _context.AllBulkSMSs.FirstOrDefaultAsync(p => !p.IsDelete && p.Mobile == user.Mobile);
            if (campaign is not null)
                campaign.IsUserRegistered = true;

            try
            {
                await _userRepository.CreateUser(user);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
            {
                return RegisterUserResponse.MobileExist;
            }

            #region Send Verification Code SMS

            var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={user.Mobile}&token={user.MobileActivationCode}&template=Register";
            var results = client.GetStringAsync(result);

            //var message = Window.Application.StaticTools.Messages.SendActivationRegisterSms(user.MobileActivationCode);

            //await _smsservice.SendSimpleSMS(user.Mobile, message);

            #endregion

            return RegisterUserResponse.Success;
        }

        public async Task<User?> GetUserByEmailActivationCode(string emailActivationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(s =>
                s.EmailActivationCode == emailActivationCode.SanitizeText());
        }

        public async Task<(LoginUserResponse Status, User? Account)> LoginUserAsync(
            LoginUserViewModel model, CancellationToken cancellationToken)
        {
            var mobile = model.Mobile.Trim();
            var normalized = MobileNumber.Normalize(mobile);
            var password = PasswordHelper.EncodePasswordMd5(model.Password);
            var candidates = _context.Users.AsNoTracking().Where(u => normalized == null
                ? u.Mobile == mobile
                : EF.Property<string>(u, MobileLoginKey.Name) == normalized);
            var matches = await candidates.Where(u => u.Password == password)
                .Select(u => new { Account = u, Excluded = EF.Property<bool>(u, MobileLoginKey.Excluded) })
                .Take(2)
                .ToListAsync(cancellationToken);

            if (matches.Count != 1)
                return (LoginUserResponse.InvalidCredentials, null);

            var account = matches[0].Account;
            if (account.IsBan || account.IsDelete)
                return (LoginUserResponse.Unavailable, null);

            if (!account.IsMobileConfirm)
            {
                if (matches[0].Excluded || await candidates.AnyAsync(u => u.Id != account.Id, cancellationToken))
                    return (LoginUserResponse.Unavailable, null);

                return (LoginUserResponse.UserNotActive, account);
            }

            _ = _smsservice.SendLookupSMS(normalized ?? account.Mobile, DateTime.Now.ToShamsi(), null, null, "Wellcoming");
            return (LoginUserResponse.Success, account);
        }

        //Check That Is User Admin Or Not 
        public async Task<bool> CheckThatIsUserAdminOrNot(ulong userId)
        {
            return await _context.Users.AnyAsync(p => !p.IsDelete && p.Id == userId && p.IsAdmin);
        }

        #endregion

        #region User

        public async Task<UserPanelEditUserInfoViewModel> FillUserPanelEditUserInfoViewModel(ulong userId)
        {
            #region Get User By Id

            var user = await GetUserById(userId);

            if (user == null) return null;

            #endregion

            #region Fill View Model

            UserPanelEditUserInfoViewModel model = new UserPanelEditUserInfoViewModel()
            {
                Mobile = user.Mobile,
                UserId = user.Id,
                AvatarName = user.Avatar,
                username = user.Username,
            };

            #endregion

            return model;
        }

        public async Task<UserPanelEditUserInfoResult> EditUserInfoInUserPanel(UserPanelEditUserInfoViewModel edit, IFormFile? UserAvatar)
        {
            #region Data Valdiation

            var user = await GetUserById(edit.UserId);

            if (user == null)
            {
                return UserPanelEditUserInfoResult.UserNotFound;
            }

            if (UserAvatar != null && !UserAvatar.IsImage())
            {
                return UserPanelEditUserInfoResult.NotValidImage;
            }

            if (UserAvatar != null)
            {
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    user.Avatar.DeleteImage(FilePaths.UserAvatarPathServer, FilePaths.UserAvatarPathThumbServer);
                }

                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(UserAvatar.FileName);
                UserAvatar.AddImageToServer(imageName, FilePaths.UserAvatarPathServer, 270, 270, FilePaths.UserAvatarPathThumbServer);
                user.Avatar = imageName;
            }

            #endregion

            #region Update User Field

            user.Username = edit.username.SanitizeText();

            _context.Update(user);
            await _context.SaveChangesAsync();

            #endregion

            return UserPanelEditUserInfoResult.Success;
        }

        public async Task<ActiveMobileByActivationCodeResult> ActiveUserMobile(ActiveMobileByActivationCodeViewModel activeMobileByActivationCodeViewModel)
        {
            #region Get User By Mobile

            var user = await GetMobileCodeAccountAsync(activeMobileByActivationCodeViewModel.Mobile);
            if (user is null || user.IsDelete || user.IsBan)
                return ActiveMobileByActivationCodeResult.AccountNotFound;

            #endregion

            #region Validation Activation Code

            if (user.MobileActivationCode != activeMobileByActivationCodeViewModel.MobileActiveCode)
            {
                return ActiveMobileByActivationCodeResult.AccountNotFound;
            }

            #endregion

            #region Update User 

            user.IsMobileConfirm = true;
            user.MobileActivationCode = new Random().Next(10000, 999999).ToString();

            await _context.SaveChangesAsync();

            #endregion

            return ActiveMobileByActivationCodeResult.Success;
        }

        public async Task ResendActivationCodeSMS(string Mobile)
        {
            var user = await GetMobileCodeAccountAsync(Mobile);
            if (user is null || user.IsDelete || user.IsBan)
                return;

            user.MobileActivationCode = new Random().Next(10000, 999999).ToString();
            user.ExpireMobileSMSDateTime = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            #region Send Verification Code SMS

            var result = $"https://api.kavenegar.com/v1/6A427559367558527A76485753667A5779587337736735753945747946474F347A346A65356E7A567A51413D/verify/lookup.json?receptor={user.Mobile}&token={user.MobileActivationCode}&template=Register";
            var results = client.GetStringAsync(result);

            //var message = Window.Application.StaticTools.Messages.SendActivationRegisterSms(user.MobileActivationCode);

            //await _smsservice.SendSimpleSMS(user.Mobile, message);

            #endregion
        }

        public Task<User?> GetMobileCodeAccountAsync(string mobile) =>
            MobileNumber.Normalize(mobile) is { } normalized
                ? _userRepository.GetUniqueMobileCodeAccountAsync(normalized)
                : Task.FromResult<User?>(null);

        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgetPasswordViewModel forgot)
        {
            var user = await GetMobileCodeAccountAsync(forgot.Mobile);
            if (user == null || user.IsDelete)
            {
                return ForgotPasswordResult.NotFound;
            }

            if (user.IsBan)
            {
                return ForgotPasswordResult.UserIsBlocked;
            }

            var code = System.Security.Cryptography.RandomNumberGenerator.GetInt32(10000, 999999).ToString();
            var message = await _smsservice.SendLookupSMS(MobileNumber.Normalize(user.Mobile)!, code, null, null, "Register");
            if (message == null)
            {
                return ForgotPasswordResult.VerificationSmsFaildFromParsGreen;
            }

            user.MobileActivationCode = code;
            user.ExpireMobileSMSDateTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return ForgotPasswordResult.Success;
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