using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.Market;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Infra.Data.Repository.OrderCheque;

public class OrderChequeCommandRepository : CommandGenericRepository<Domain.Entities.Market.SellerChequeInfo> , IOrderChequeCommandRepository
{
	#region ctor 

	private readonly WindowDbContext _context;

    public OrderChequeCommandRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Seller Side 

    public async Task AddOrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque , CancellationToken cancellationToken)
    {
        await _context.orderCheques.AddAsync(orderCheque);
    }

    public async Task AddSellerChequeInfo(Domain.Entities.Market.SellerChequeInfo sellerChequeInfo, 
                                          CancellationToken cancellationToken)
    {
        await _context.SellerChequeInfos.AddAsync(sellerChequeInfo);
    }

    #endregion

    public void Update_OrderCheque(Domain.Entities.ShopOrder.OrderCheque orderCheque)
    {
        _context.orderCheques.Update(orderCheque);
    }

    public void Update_SellerChequeInfo(Domain.Entities.Market.SellerChequeInfo sellerChequeInfo)
    {
        _context.SellerChequeInfos.Update(sellerChequeInfo);
    }
}
