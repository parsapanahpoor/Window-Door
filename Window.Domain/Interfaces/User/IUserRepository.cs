﻿using Window.Domain.Entities.Account;
using Window.Domain.Entities.Wallet;
using Window.Domain.ViewModels.Seller.ShopOrder;
using Window.Domain.ViewModels.User;
using Window.Domain.ViewModels.User.Account;

namespace Window.Domain.Interfaces
{
    public interface IUserRepository
    {
        #region user

        //Create Wallet Without Calculate
        Task CreateWalletWithoutCalculate(Wallet wallet);

        Task<EditProfileViewModel?> GetUserProfileForEditAsync(ulong userId);

        Task<bool> IsUserActive(string email);

        Task<bool> IsUserExist(ulong userId);

        Task<bool> IsEmailExist(string email);

        Task<bool> IsMobileExist(string mobile);

        Task<bool> IsPasswordValid(string email, string password);

        Task CreateUser(User user);

        Task EditUser(User user, ulong userId);

        Task<bool> RemoveUserById(ulong userId);

        Task<User?> GetUserById(ulong userId);

        Task<User?> GetUserByEmail(string email);

        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);

        List<User> GetAllUsers();

        Task<FilterUserViewModel> FilterUsersForSellerPanel(FilterUserViewModel filter, ulong masterId);

        #endregion

        #region General Methods 

        Task<string?> Get_UserMobile_ByUserId(ulong userId, CancellationToken cancellation);

        #endregion

        #region Site Side 

        Task<Window.Domain.ViewModels.Site.Shop.ShopProduct.Seller?> FillSeller(ulong userId,
                                                                                     CancellationToken cancellation);

        #endregion

        #region Seller Side

        Task<SellerInformations?> Fill_SellerInformations(ulong sellerUserId,
                                                          CancellationToken cancellation);

        #endregion

        #region save changees
        Task SaveChanges();

        #endregion
    }
}
