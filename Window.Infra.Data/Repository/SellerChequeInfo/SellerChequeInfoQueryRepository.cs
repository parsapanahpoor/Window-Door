using Window.Data;
using Window.Data.Context;
using Window.Domain.Interfaces.SellerChequeInfo;

namespace Window.Infra.Data.Repository.SellerChequeInfo;

public class SellerChequeInfoQueryRepository : QueryGenericRepository<Domain.Entities.Market.SellerChequeInfo> , ISellerChequeInfoQueryRepository
{
    #region ctor 

    private readonly WindowDbContext _context;

    public SellerChequeInfoQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}
