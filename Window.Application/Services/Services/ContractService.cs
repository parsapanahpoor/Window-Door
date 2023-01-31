using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Comment;
using Window.Domain.Entities.Contract;
using Window.Domain.ViewModels.Seller.Contract;
using Window.Domain.ViewModels.Seller.Product;
using Window.Domain.ViewModels.Site.Inquiry;

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

        //Check That Is Exist Any Market By This Seller Id
        public async Task<bool> CheckThatIsExistAnyMarketByThisSellerId(ulong sellerId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == sellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            return true;
        }

        //List Of seller Comments For Show
        public async Task<List<Comment>?> ListOfSellerCommentsForShow(ulong sellerId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == sellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion

            return await _context.Comments.Include(p=> p.User)
                                    .Where(p => !p.IsDelete && p.SellerId == sellerId).ToListAsync();
        }

        //Add Comment From User
        public async Task<bool> AddCommentFromUser(AddCommentSiteSideViewModel comment , ulong userId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == comment.SellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            #region Get User By User Id 

            var user = await _context.Users.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == userId);
            if (user == null) return false;

            #endregion

            #region Add Comment From User 

            Comment model = new Comment()
            {
                UserId = userId,
                SellerId = comment.SellerId,
                Description = comment.Description.SanitizeText()
            };

            //Add To The Data Base
            await _context.Comments.AddAsync(model);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

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
                RequestForContractType = Domain.Enums.RequestForContract.RequestForContractType.Waiting
            };

            //Add To The Data Base 
            await _context.RequestForContracts.AddAsync(request);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        #endregion

        #region Seller Side 

        //Count Of User Waiting Contract Request 
        public async Task<int> CountOfUserWaitingContractRequest(ulong sellerId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == sellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return 0;

            #endregion

            return await _context.RequestForContracts.Where(p => !p.IsDelete && p.SellerId == market.UserId
                                                                && p.RequestForContractType == Domain.Enums.RequestForContract.RequestForContractType.Waiting)
                                                                    .CountAsync();
        }

        //List Of Seller Contracts Request 
        public async Task<FilterContractRequestSellerSideViewModel> FilterContractRequestSellerSide(FilterContractRequestSellerSideViewModel filter)
        { 
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == filter.SellerUserId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return null;

            #endregion
            
            var query = _context.RequestForContracts
                .Include(p => p.User)
                .Where(a => !a.IsDelete && a.SellerId == market.UserId)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            await filter.Paging(query);

            return filter;
        }

        //Decline Request From Seller
        public async Task<bool> DeclineRequestFromSeller(ulong requestId , ulong sellerId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == sellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            #region Get Request

            var request = await _context.RequestForContracts.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == requestId);
            if (request == null || request.SellerId != market.UserId) return false;

            #endregion

            #region Decline request Type 

            request.RequestForContractType = Domain.Enums.RequestForContract.RequestForContractType.Decline;

            //Update Request
            _context.RequestForContracts.Update(request);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        //Accept Request From Seller
        public async Task<bool> AcceptRequestFromSeller(ulong requestId, ulong sellerId)
        {
            #region Get Market By Id 

            var market = await _context.MarketUser.Where(p => !p.IsDelete && p.UserId == sellerId).Select(p => p.Market).FirstOrDefaultAsync();
            if (market == null) return false;

            #endregion

            #region Get Request

            var request = await _context.RequestForContracts.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == requestId);
            if (request == null || request.SellerId != market.UserId) return false;

            #endregion

            #region Accept request Type 

            request.RequestForContractType = Domain.Enums.RequestForContract.RequestForContractType.Accepted;

            //Update Request
            _context.RequestForContracts.Update(request);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        #endregion
    }
}
