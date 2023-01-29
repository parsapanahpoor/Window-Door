using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Contract;

namespace Window.Application.Services.Services
{
    public class ContractService : IContractService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public ContractService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Site Side

        //Can User Insert Comment For Seller
        public async Task<RequestForContract?> CanUserInsertCommentForSeller(ulong userId , ulong sellerId)
        {
            #region Get user 

            var user = await _context.Users.FirstOrDefaultAsync(p=> !p.IsDelete && p.Id == sellerId);
            if (user == null) return null;

            #endregion

            #region Get Seller

            var seller = await _context.Users.FirstOrDefaultAsync(p=> !p.IsDelete && p.Id == sellerId);
            if(seller == null) return null;

            #endregion

            #region Get Contract

            var contract = await _context.RequestForContracts.FirstOrDefaultAsync(p => !p.IsDelete && p.SellerId == sellerId && p.UserId == userId); 
            if(contract == null) return null;

            #endregion

            return contract;
        }

        //Create Contract Request From User
        public async Task<bool> CreateContractRequestFromUser(ulong userId , ulong sellerId)
        {
            #region Get user 

            var user = await _context.Users.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sellerId);
            if (user == null) return false;

            #endregion

            #region Get Seller

            var seller = await _context.Users.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sellerId);
            if (seller == null) return false;

            #endregion

            #region Get Contract

            var contract = await _context.RequestForContracts.FirstOrDefaultAsync(p => !p.IsDelete && p.SellerId == sellerId && p.UserId == userId);
            if (contract != null) return false;

            #endregion

            #region Create Contract Request

            RequestForContract request = new RequestForContract()
            {
                UserId = userId,
                SellerId = sellerId,
                RequestForContractStatus = null,
                RequestForContractType = Domain.Enums.RequestForContract.RequestForContractType.Waiting
            };

            //Add To The Data Base 
            await _context.RequestForContracts.AddAsync(request);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        #endregion
    }
}
