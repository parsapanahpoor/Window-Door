using Microsoft.EntityFrameworkCore;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces.Order;
namespace Window.Infra.Data.Repository.Order;

public class OrderQueryRepository : QueryGenericRepository<Domain.Entities.ShopOrder.Order> , IOrderQueryRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

    public OrderQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Site Side 

    public async Task<Domain.Entities.ShopOrder.Order?> IsExistAnyWaitingOrder(ulong userId , 
                                                                              CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete && 
                                    p.UserId == userId && 
                                    !p.IsFinally)
                             .FirstOrDefaultAsync();
    }

    public async Task<OrderDetail?> Get_OrderDetail_ByOrderIdAndProductId(ulong orderId , 
                                                                         ulong productId , 
                                                                         CancellationToken cancellation)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId &&
                                    p.ProductId == productId)
                             .FirstOrDefaultAsync();
    }

    #endregion
}
