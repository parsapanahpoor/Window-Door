using Microsoft.EntityFrameworkCore;
using System.Data;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Site.Shop.Order;
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

    public async Task<List<ulong>> GetOrderDetailIds_OrderDetails_ByOrderId(ulong orderId , CancellationToken cancellation)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p=> !p.IsDelete && 
                                    p.OrderId == orderId)
                             .Select(p=> p.Id)
                             .ToListAsync();
    }

    public async Task<ShopCartOrderDetailItems?> FillShopCartOrderDetailItems(ulong orderDetailId , CancellationToken cancellationToken)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderDetailId)
                             .Select(p => new ShopCartOrderDetailItems()
                             {
                                 ProductId = p.ProductId,
                                 Products = _context.ShopProducts
                                                    .AsNoTracking()
                                                    .Where(s => !s.IsDelete &&
                                                           s.Id == p.ProductId)
                                                    .Select(s => new Product()
                                                    {
                                                        ProductEntity = p.Count,
                                                        ProductId = s.Id,
                                                        ProductTitle = s.ProductName,
                                                        ProductImage = s.ProductImage,
                                                        SellerInfo = _context.Users
                                                                             .AsNoTracking()
                                                                             .Where(u => u.IsDelete &&
                                                                                    u.Id == s.SellerUserId)
                                                                             .Select(u => new ShopCartOrderProductSellerInfo()
                                                                             {
                                                                                 SellerUserId = s.SellerUserId,
                                                                                 SellerUsername = u.Username
                                                                             })
                                                                             .FirstOrDefault(),
                                                        Color = _context.ShopColors
                                                                        .AsNoTracking()
                                                                        .Where(c => !c.IsDelete &&
                                                                               c.Id == s.ProductColorId)
                                                                        .Select(c => new Color()
                                                                        {
                                                                            ColorCode = c.ColorCode,
                                                                            ColorId = c.Id,
                                                                            ColorTitle = c.ColorTitle
                                                                        })
                                                                        .FirstOrDefault()
                                                    })
                                                    .FirstOrDefault()
                             })
                             .FirstOrDefaultAsync();
    }

    #endregion
}
