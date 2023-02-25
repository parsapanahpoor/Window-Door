using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Market;

namespace Window.Application.Services.Services
{
    public class MarketService : IMarketService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public MarketService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Site Side 

        public async Task<bool> AddMarket(ulong userId , string titleName)
        {
            #region Is Exist Any Market By This User

            if (await _context.Market.AnyAsync(p=> !p.IsDelete && p.UserId == userId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(titleName))
            {
                return false;
            }

            #endregion

            #region Add Market 

            Market market = new Market()
            {
                UserId = userId,
                IsDelete = false,
                CreateDate = DateTime.Now,
                ActivationTariff = await _context.SiteSettings.Where(p=> !p.IsDelete).Select(p=> p.ChargeOfNewMarkets).FirstOrDefaultAsync(),
                MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller,
                MarketName = titleName
            };

            await _context.Market.AddAsync(market);
            await _context.SaveChangesAsync();

            #endregion

            #region Add User To market

            MarketUsers marketUsers = new MarketUsers()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                MarketId = market.Id,
                UserId = userId,
                RoleId = 4
            };

            await  _context.MarketUser.AddAsync(marketUsers);
            await _context.SaveChangesAsync();

            #endregion

            #region Add User Role

            UserRole role = new UserRole()
            {
                UserId =userId,
                RoleId = 4,
            };

            await _context.UserRoles.AddAsync(role);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }


        #endregion
    }
}
