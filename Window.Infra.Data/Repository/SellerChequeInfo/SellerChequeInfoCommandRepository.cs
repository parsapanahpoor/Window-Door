using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.Market;
using Window.Domain.Interfaces.SellerChequeInfo;

namespace Window.Infra.Data.Repository.SellerChequeInfo;

public class SellerChequeInfoCommandRepository : CommandGenericRepository<Domain.Entities.Market.SellerChequeInfo> , ISellerChequeInfoCommandRepository
{
	#region ctor 

	private readonly WindowDbContext _context;

    public SellerChequeInfoCommandRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}
