using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Wallet;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Admin.Wallet;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.User;
using Window.Domain.ViewModels.User.Account;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        #region constructor

        private readonly WindowDbContext _context;

        public UserRepository(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region user

        public Task<EditProfileViewModel?> GetUserProfileForEditAsync(ulong userId)
        {
            return Task.FromResult(_context.Users
                .Where(u => u.Id == userId)
                .Select(u => new EditProfileViewModel
                {
                    Username = u.Username,
                    Mobile = u.Mobile,
                    AvatarName = u.Avatar
                }).FirstOrDefault());
        }

        public Task<bool> IsUserActive(string email)
        {
            return Task.FromResult(_context.Users.Any(u => u.Email == email));
        }

        public Task<bool> IsUserExist(ulong userId)
        {
            return Task.FromResult(_context.Users.Any(u => u.Id == userId));
        }

        public Task<bool> IsEmailExist(string email)
        {
            return Task.FromResult(_context.Users.Any(u => u.Email == email));
        }

        public Task<bool> IsMobileExist(string mobile)
        {
            return Task.FromResult(_context.Users.Any(u => u.Mobile == mobile));
        }

        public Task<bool> IsPasswordValid(string email, string password)
        {
            return Task.FromResult(_context.Users.Any(u => u.Email == email && u.Password == password));
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveChanges();

        }

        public async Task EditUser(User user, ulong userId)
        {
            var editedUser = await GetUserById(userId);
            _context.Users.Update(editedUser);
            await SaveChanges();

        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter)
        {
            var query = _context.Users
                .Include(u => u.UserRoles)
                .OrderByDescending(p=> p.CreateDate)
                .AsQueryable();

            #region order

            switch (filter.OrderType)
            {
                case FilterUserViewModel.FilterUserOrderType.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;
                case FilterUserViewModel.FilterUserOrderType.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
            }

            #endregion

            #region filter

            if ((!string.IsNullOrEmpty(filter.Email)))
            {
                query = query.Where(u => u.Email.Contains(filter.Email));
            }
            if ((!string.IsNullOrEmpty(filter.Mobile)))
            {
                query = query.Where(u => u.Mobile.Contains(filter.Mobile));
            }
            if ((!string.IsNullOrEmpty(filter.Username)))
            {
                query = query.Where(u => u.Username.Contains(filter.Username));
            }

            #endregion

            #region paging

            await filter.Paging(query);

            #endregion

            return filter;
        }

        public async Task<FilterUserViewModel> FilterUsersForSellerPanel(FilterUserViewModel filter , ulong masterId)
        {
            #region Get Market

            var market = await _context.Market.FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == masterId);

            #endregion

            var query = _context.MarketUser.Include(p=>p.User)
                .Where(p=> !p.IsDelete && p.MarketId == market.Id)
                .Select(p=> p.User)
                .AsQueryable();

            #region order

            switch (filter.OrderType)
            {
                case FilterUserViewModel.FilterUserOrderType.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;
                case FilterUserViewModel.FilterUserOrderType.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
            }

            #endregion

            #region filter

            if ((!string.IsNullOrEmpty(filter.Email)))
            {
                query = query.Where(u => u.Email.Contains(filter.Email));
            }
            if ((!string.IsNullOrEmpty(filter.Mobile)))
            {
                query = query.Where(u => u.Mobile.Contains(filter.Mobile));
            }
            if ((!string.IsNullOrEmpty(filter.Username)))
            {
                query = query.Where(u => u.Username.Contains(filter.Username));
            }

            #endregion

            #region paging

            await filter.Paging(query);

            #endregion

            return filter;
        }

        public async Task<User?> GetUserById(ulong userId)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> RemoveUserById(ulong userId)
        {
            var removedUser = await GetUserById(userId);

            if (removedUser == null)
            {
                return false;
            }

            removedUser.IsDelete = true;
            _context.Update(removedUser);
            await SaveChanges();
            return true;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.Where(u => !u.IsDelete).ToList();
        }

        #endregion

        #region save changes

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
