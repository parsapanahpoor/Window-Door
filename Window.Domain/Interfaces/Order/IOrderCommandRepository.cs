namespace Window.Domain.Interfaces.Order;

public interface IOrderCommandRepository
{
    Task AddAsync(Domain.Entities.ShopOrder.Order order, CancellationToken cancellationToken);

    Task AddOrderDetailAsync(Domain.Entities.ShopOrder.OrderDetail orderDetail, CancellationToken cancellationToken);

    void UpdateOrderDetail(Domain.Entities.ShopOrder.OrderDetail orderDetail);

    void Update(Entities.ShopOrder.Order order);
}
