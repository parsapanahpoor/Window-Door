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
            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                #region Is Exist Any Market By This User

                if (await _context.Market.AnyAsync(p=> !p.IsDelete && p.UserId == userId))
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                if (!IsValidMarketName(titleName))
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                #endregion

                AddMarketEntities(userId, titleName.Trim());
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception ex) when (ex.GetBaseException() is Microsoft.Data.SqlClient.SqlException sql && sql.Number == 1205)
            {
                // A concurrent request won the serializable range; report a controlled failure.
                try { await transaction.RollbackAsync(); } catch { /* already rolled back */ }
                return false;
            }
        }

        public bool IsValidMarketName(string? titleName) =>
            !string.IsNullOrWhiteSpace(titleName) && titleName.Trim().Length is >= 3 and <= 100;

        public Task<Market?> GetMarketByUserId(ulong userId) =>
            _context.Market.AsNoTracking().FirstOrDefaultAsync(p => !p.IsDelete && p.UserId == userId);

        public void AddMarketEntities(ulong userId, string titleName)
        {
            #region Add Market

            Market market = new Market()
            {
                UserId = userId,
                IsDelete = false,
                CreateDate = DateTime.Now,
                ActivationTariff = _context.SiteSettings.Where(p=> !p.IsDelete).Select(p=> p.ChargeOfNewMarkets).FirstOrDefault(),
                MarketPersonalsInfoState = MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller,
                MarketName = titleName.Trim()
            };

            _context.Market.Add(market);

            #endregion

            #region Add User To market

            MarketUsers marketUsers = new MarketUsers()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                Market = market,
                UserId = userId,
                RoleId = 4
            };

            _context.MarketUser.Add(marketUsers);

            #endregion

            #region Add User Role

            if (!_context.UserRoles.Any(existing => existing.UserId == userId && existing.RoleId == 4 && !existing.IsDelete))
            {
                UserRole role = new UserRole()
                {
                    UserId = userId,
                    RoleId = 4,
                };

                _context.UserRoles.Add(role);
            }

            #endregion
        }


        #endregion
    }
}
