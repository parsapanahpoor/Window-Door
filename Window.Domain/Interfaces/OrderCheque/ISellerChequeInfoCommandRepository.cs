namespace Window.Domain.Interfaces.OrderCheque;

public interface IOrderChequeCommandRepository
{
    #region Seller Side 

    Task AddOrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque, CancellationToken cancellationToken);

    void Update_OrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque);

    #endregion
}
