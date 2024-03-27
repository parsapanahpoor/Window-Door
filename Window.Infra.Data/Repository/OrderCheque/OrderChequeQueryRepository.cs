using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.Market;
using Window.Domain.Interfaces.OrderCheque;
using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.OrderCheque;
using Window.Domain.ViewModels.Seller.SellerChequeInfo;
using Window.Infra.Data.Migrations;

namespace Window.Infra.Data.Repository.OrderCheque;

public class OrderChequeQueryRepository : QueryGenericRepository<Domain.Entities.Market.SellerChequeInfo>, IOrderChequeQueryRepository
{
    #region ctor 

    private readonly WindowDbContext _context;

    public OrderChequeQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region General Methods 

    public async Task<Domain.Entities.ShopOrder.OrderCheque?> Get_OrderCheque_ByIdAsync(ulong chequeId , 
                                                                                        CancellationToken cancellation)
    {
        return await _context.orderCheques
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.Id == chequeId);
    }

    #endregion

    #region Admin 

    public async Task<ChequeDetailDTO?> Fill_ChequeDetailDTO_ByOrderChequeId(ulong orderChequeId , 
                                                                             CancellationToken cancellationToken)
    {
        return await _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderChequeId)
                             .Select(p => new ChequeDetailDTO()
                             {
                                 ChequeDateTime = p.ChequeDateTime,
                                 ChequeId = orderChequeId,
                                 ChequeImage = p.ChequeImage,
                                 ChequePrice = p.ChequePrice,
                                 CustomerNationalId = p.CustomerNationalId,
                                 OrderChequeSellerState = p.OrderChequeSellerState,
                                 SellerRejectDescription = p.SellerRejectDescription,
                                 AdminRejectDescription = p.AdminRejectDescription,
                                 OrderChequeAdminState = p.OrderChequeAdminState,
                                 OrderId = p.OrderId
                             })
                             .FirstOrDefaultAsync();
    }

    #endregion

    #region Seller Side

    public async Task<SellerChequeInfoSellerSideDTO?> Fill_SellerChequeInfoSellerSide_DTO(ulong sellerUserId,
                                                                                          CancellationToken cancellationToken)
    {
        return await _context.SellerChequeInfos
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete && 
                                    p.SellerUserId == sellerUserId)
                             .Select(p=> new SellerChequeInfoSellerSideDTO()
                             {
                                 CountOfCheque = p.CountOfCheque,
                                 HasLimitation = p.HasLimitation,
                                 SellerMaximumDays = p.SellerMaximumDays,
                                 SellerUserId = p.SellerUserId
                             })
                             .FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.Market.SellerChequeInfo?> Get_SellerChequeInfo_BySellerUserId(ulong sellerUserId,
                                                                                                   CancellationToken cancellationToken)
    {
        return await _context.SellerChequeInfos
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.SellerUserId == sellerUserId)
                             .FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.Market.SellerChequeInfo?> Get_SellerChequeInfo_BySellerUserId_Sync(ulong sellerUserId)
    {
        return await _context.SellerChequeInfos
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.SellerUserId == sellerUserId)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<Domain.Entities.ShopOrder.OrderCheque>> Get_ListOfCustomerOrderCheques_ByOrderAndUserId(ulong userId,
                                                                                                                   ulong orderId,
                                                                                                                   CancellationToken cancellationToken)
    {
        return await _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId &&
                                    p.CustomerUserId == userId)
                             .OrderByDescending(p => p.ChequeDateTime)
                             .ToListAsync();
    }

    public async Task<FilterOrderChequesDTO> FillFilterOrderChequesDTO(FilterOrderChequesDTO filter,
                                                                       CancellationToken cancellation)
    {
        var chques = _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete)
                             .OrderByDescending(p => p.CreateDate)
                             .AsQueryable();

        var order = from c in chques
                    join o in _context.Orders
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete)
                                      .AsQueryable()
                    on c.OrderId equals o.Id
                    select new OrderChequesDTO()
                    {
                        CustomerUsername = null,
                        ChequeDateTime = c.ChequeDateTime,
                        OrderId = o.Id,
                        OrderIsFinally = o.IsFinally,
                        ChequePrice = c.ChequePrice,
                        CustomerUserId = c.CustomerUserId,
                        OrderChequeAdminState = c.OrderChequeAdminState,
                        OrderChequeSellerState = c.OrderChequeSellerState,
                    };

        var query = from o in order
                    join u in _context.Users
                                      .AsNoTracking()
                                      .Where(p=> !p.IsDelete)
                                      .AsQueryable()
                    on o.CustomerUserId equals u.Id
                    select new OrderChequesDTO()
                    {
                        CustomerUsername = u.Username,
                        ChequeDateTime = o.ChequeDateTime,
                        OrderId = o.OrderId,
                        OrderIsFinally = o.OrderIsFinally,
                        ChequePrice = o.ChequePrice,
                        CustomerUserId = o.CustomerUserId,
                        OrderChequeAdminState = o.OrderChequeAdminState,
                        OrderChequeSellerState = o.OrderChequeSellerState,
                    };

        #region Filter

        if (!string.IsNullOrEmpty(filter.StartDate))
        {
            var spliteDate = filter.StartDate.Split('/');
            int year = int.Parse(spliteDate[0]);
            int month = int.Parse(spliteDate[1]);
            int day = int.Parse(spliteDate[2]);
            DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

            query = query.Where(s => s.ChequeDateTime >= fromDate);
        }

        if (!string.IsNullOrEmpty(filter.EndDate))
        {
            var spliteDate = filter.EndDate.Split('/');
            int year = int.Parse(spliteDate[0]);
            int month = int.Parse(spliteDate[1]);
            int day = int.Parse(spliteDate[2]);
            DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

            query = query.Where(s => s.ChequeDateTime <= toDate);
        }

        switch (filter.OrderChequeAdminState)
        {
            case OrderChequeAdminStateDTO.All: 
                break;

            case OrderChequeAdminStateDTO.Accept:
                query = query.Where(p=> p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Accept);
                break;

            case OrderChequeAdminStateDTO.WaitingForCheck:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.WaitingForCheck);
                break;

            case OrderChequeAdminStateDTO.Reject:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Reject);
                break;
        }

        switch (filter.OrderChequeSellerState)
        {
            case OrderChequeSellerStateDTO.All:
                break;

            case OrderChequeSellerStateDTO.Accept:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Accept);
                break;

            case OrderChequeSellerStateDTO.WaitingForCheck:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.WaitingForCheck);
                break;

            case OrderChequeSellerStateDTO.Reject:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Reject);
                break;
        }

        switch (filter.OrderFinallyState)
        {
            case OrderFinallyStateDTO.All:
                break;

            case OrderFinallyStateDTO.Finally:
                query = query.Where(p => p.OrderIsFinally);
                break;

            case OrderFinallyStateDTO.NotFinally:
                query = query.Where(p => !p.OrderIsFinally);
                break;
        }

        if (!string.IsNullOrEmpty(filter.CustomerUsername))
        {
            query = query.Where(p => p.CustomerUsername.Contains(filter.CustomerUsername));
        }

        #endregion

        await filter.Paging(query);

        return filter;
    }

    public async Task<FilterReciveOrderChequesSellerSideDTO> FillFilterReciveOrderChequesDTO(FilterReciveOrderChequesSellerSideDTO filter,
                                                                                             CancellationToken cancellation)
    {
        var chques = _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.SellerUserId == filter.UserId)
                             .OrderByDescending(p => p.CreateDate)
                             .AsQueryable();

        var order = from c in chques
                    join o in _context.Orders
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete)
                                      .AsQueryable()
                    on c.OrderId equals o.Id
                    select new OrderChequesDTO()
                    {
                        CustomerUsername = null,
                        ChequeDateTime = c.ChequeDateTime,
                        OrderId = o.Id,
                        OrderIsFinally = o.IsFinally,
                        ChequePrice = c.ChequePrice,
                        CustomerUserId = c.CustomerUserId,
                        OrderChequeAdminState = c.OrderChequeAdminState,
                        OrderChequeSellerState = c.OrderChequeSellerState,
                    };

        var query = from o in order
                    join u in _context.Users
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete)
                                      .AsQueryable()
                    on o.CustomerUserId equals u.Id
                    select new OrderChequesDTO()
                    {
                        CustomerUsername = u.Username,
                        ChequeDateTime = o.ChequeDateTime,
                        OrderId = o.OrderId,
                        OrderIsFinally = o.OrderIsFinally,
                        ChequePrice = o.ChequePrice,
                        CustomerUserId = o.CustomerUserId,
                        OrderChequeAdminState = o.OrderChequeAdminState,
                        OrderChequeSellerState = o.OrderChequeSellerState,
                    };

        #region Filter

        if (!string.IsNullOrEmpty(filter.StartDate))
        {
            var spliteDate = filter.StartDate.Split('/');
            int year = int.Parse(spliteDate[0]);
            int month = int.Parse(spliteDate[1]);
            int day = int.Parse(spliteDate[2]);
            DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

            query = query.Where(s => s.ChequeDateTime >= fromDate);
        }

        if (!string.IsNullOrEmpty(filter.EndDate))
        {
            var spliteDate = filter.EndDate.Split('/');
            int year = int.Parse(spliteDate[0]);
            int month = int.Parse(spliteDate[1]);
            int day = int.Parse(spliteDate[2]);
            DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

            query = query.Where(s => s.ChequeDateTime <= toDate);
        }

        switch (filter.OrderChequeAdminState)
        {
            case OrderChequeAdminStateDTO.All:
                break;

            case OrderChequeAdminStateDTO.Accept:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Accept);
                break;

            case OrderChequeAdminStateDTO.WaitingForCheck:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.WaitingForCheck);
                break;

            case OrderChequeAdminStateDTO.Reject:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Reject);
                break;
        }

        switch (filter.OrderChequeSellerState)
        {
            case OrderChequeSellerStateDTO.All:
                break;

            case OrderChequeSellerStateDTO.Accept:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Accept);
                break;

            case OrderChequeSellerStateDTO.WaitingForCheck:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.WaitingForCheck);
                break;

            case OrderChequeSellerStateDTO.Reject:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Reject);
                break;
        }

        switch (filter.OrderFinallyState)
        {
            case OrderFinallyStateDTO.All:
                break;

            case OrderFinallyStateDTO.Finally:
                query = query.Where(p => p.OrderIsFinally);
                break;

            case OrderFinallyStateDTO.NotFinally:
                query = query.Where(p => !p.OrderIsFinally);
                break;
        }

        if (!string.IsNullOrEmpty(filter.CustomerUsername))
        {
            query = query.Where(p => p.CustomerUsername.Contains(filter.CustomerUsername));
        }

        #endregion

        await filter.Paging(query);

        return filter;
    }

    public async Task<FilterReciveOrderChequesSellerSideDTO> FillFilterDepositOrderChequesDTO(FilterReciveOrderChequesSellerSideDTO filter,
                                                                                              CancellationToken cancellation)
    {
        var chques = _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.CustomerUserId == filter.UserId)
                             .OrderByDescending(p => p.CreateDate)
                             .AsQueryable();

        var order = from c in chques
                    join o in _context.Orders
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete)
                                      .AsQueryable()
                    on c.OrderId equals o.Id
                    select new OrderChequesDTO()
                    {
                        CustomerUsername = null,
                        ChequeDateTime = c.ChequeDateTime,
                        OrderId = o.Id,
                        OrderIsFinally = o.IsFinally,
                        ChequePrice = c.ChequePrice,
                        CustomerUserId = c.CustomerUserId,
                        OrderChequeAdminState = c.OrderChequeAdminState,
                        OrderChequeSellerState = c.OrderChequeSellerState,
                    };

        var query = from o in order
                    join u in _context.Users
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete)
                                      .AsQueryable()
                    on o.CustomerUserId equals u.Id
                    select new OrderChequesDTO()
                    {
                        CustomerUsername = u.Username,
                        ChequeDateTime = o.ChequeDateTime,
                        OrderId = o.OrderId,
                        OrderIsFinally = o.OrderIsFinally,
                        ChequePrice = o.ChequePrice,
                        CustomerUserId = o.CustomerUserId,
                        OrderChequeAdminState = o.OrderChequeAdminState,
                        OrderChequeSellerState = o.OrderChequeSellerState,
                    };

        #region Filter

        if (!string.IsNullOrEmpty(filter.StartDate))
        {
            var spliteDate = filter.StartDate.Split('/');
            int year = int.Parse(spliteDate[0]);
            int month = int.Parse(spliteDate[1]);
            int day = int.Parse(spliteDate[2]);
            DateTime fromDate = new DateTime(year, month, day, new PersianCalendar());

            query = query.Where(s => s.ChequeDateTime >= fromDate);
        }

        if (!string.IsNullOrEmpty(filter.EndDate))
        {
            var spliteDate = filter.EndDate.Split('/');
            int year = int.Parse(spliteDate[0]);
            int month = int.Parse(spliteDate[1]);
            int day = int.Parse(spliteDate[2]);
            DateTime toDate = new DateTime(year, month, day, new PersianCalendar());

            query = query.Where(s => s.ChequeDateTime <= toDate);
        }

        switch (filter.OrderChequeAdminState)
        {
            case OrderChequeAdminStateDTO.All:
                break;

            case OrderChequeAdminStateDTO.Accept:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Accept);
                break;

            case OrderChequeAdminStateDTO.WaitingForCheck:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.WaitingForCheck);
                break;

            case OrderChequeAdminStateDTO.Reject:
                query = query.Where(p => p.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Reject);
                break;
        }

        switch (filter.OrderChequeSellerState)
        {
            case OrderChequeSellerStateDTO.All:
                break;

            case OrderChequeSellerStateDTO.Accept:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Accept);
                break;

            case OrderChequeSellerStateDTO.WaitingForCheck:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.WaitingForCheck);
                break;

            case OrderChequeSellerStateDTO.Reject:
                query = query.Where(p => p.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Reject);
                break;
        }

        switch (filter.OrderFinallyState)
        {
            case OrderFinallyStateDTO.All:
                break;

            case OrderFinallyStateDTO.Finally:
                query = query.Where(p => p.OrderIsFinally);
                break;

            case OrderFinallyStateDTO.NotFinally:
                query = query.Where(p => !p.OrderIsFinally);
                break;
        }

        if (!string.IsNullOrEmpty(filter.CustomerUsername))
        {
            query = query.Where(p => p.CustomerUsername.Contains(filter.CustomerUsername));
        }

        #endregion

        await filter.Paging(query);

        return filter;
    }

    public async Task<ChequeDetailSellerSideDTO?> Fill_SellerSideChequeDetailDTO_ByOrderChequeId(ulong orderChequeId,
                                                                                       ulong sellerUserId ,
                                                                                       CancellationToken cancellationToken)
    {   
        return await _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.SellerUserId == sellerUserId && 
                                    p.Id == orderChequeId)
                             .Select(p => new ChequeDetailSellerSideDTO()
                             {
                                 ChequeDateTime = p.ChequeDateTime,
                                 ChequeId = orderChequeId,
                                 ChequeImage = p.ChequeImage,
                                 ChequePrice = p.ChequePrice,
                                 CustomerNationalId = p.CustomerNationalId,
                                 OrderChequeSellerState = p.OrderChequeSellerState,
                                 SellerRejectDescription = p.SellerRejectDescription,
                                 AdminRejectDescription = p.AdminRejectDescription,
                                 OrderChequeAdminState = p.OrderChequeAdminState,
                                 OrderId = p.OrderId,
                                 ChequeReceiptImage = p.ChequeReceiptFileName,
                                 ChequeReceiptDescription = p.CustomerChequeReceiptDescription
                             })
                             .FirstOrDefaultAsync();
    }

    #endregion
}
