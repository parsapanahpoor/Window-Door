using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.Order;

namespace Window.Infra.Data.Repository.Order;

public class OrderCommandRepository : CommandGenericRepository<Domain.Entities.ShopOrder.Order>,  IOrderCommandRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public OrderCommandRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    public async Task AddOrderDetailAsync(Domain.Entities.ShopOrder.OrderDetail orderDetail, CancellationToken cancellationToken)
    {
        await _context.OrderDetails.AddAsync(orderDetail, cancellationToken);
    }

    public void UpdateOrderDetail(Domain.Entities.ShopOrder.OrderDetail orderDetail)
    {
        _context.OrderDetails.Update(orderDetail);
    }
}
