using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Infra.Data.Repository.OrderCheque;

public class OrderChequeQueryRepository : QueryGenericRepository<Domain.Entities.Market.SellerChequeInfo> , IOrderChequeQueryRepository
{
    #region ctor 

    private readonly WindowDbContext _context;

    public OrderChequeQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Seller Side

    public async Task<Domain.Entities.Market.SellerChequeInfo?> Get_SellerChequeInfo_BySellerUserId(ulong sellerUserId , 
                                                                                                   CancellationToken cancellationToken)
    {
        return await _context.SellerChequeInfos
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.SellerUserId == sellerUserId)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<Domain.Entities.ShopOrder.OrderCheque>> Get_ListOfCustomerOrderCheques_ByOrderAndUserId(ulong userId , 
                                                                                                                   ulong orderId , 
                                                                                                                   CancellationToken cancellationToken)
    {
        return await _context.orderCheques
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId &&
                                    p.CustomerUserId == userId)
                             .OrderByDescending(p=> p.ChequeDateTime)
                             .ToListAsync();
    }

    #endregion
}
