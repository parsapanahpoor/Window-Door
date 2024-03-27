namespace Window.Domain.Interfaces.OrderCheque;

public interface IOrderChequeCommandRepository
{
    #region Seller Side 

    Task AddOrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque, CancellationToken cancellationToken);

    void Update_OrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque);

    Task AddSellerChequeInfo(Domain.Entities.Market.SellerChequeInfo sellerChequeInfo,
                             CancellationToken cancellationToken);

    void Update_SellerChequeInfo(Domain.Entities.Market.SellerChequeInfo sellerChequeInfo);

    #endregion
}
