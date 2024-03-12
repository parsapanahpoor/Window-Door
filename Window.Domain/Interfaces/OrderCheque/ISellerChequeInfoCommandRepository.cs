namespace Window.Domain.Interfaces.OrderCheque;

public interface IOrderChequeCommandRepository
{
    #region Seller Side 

    Task AddOrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque, CancellationToken cancellationToken);

    #endregion
}
