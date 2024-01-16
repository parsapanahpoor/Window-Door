using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopProductGallery;
using Window.Domain.Interfaces.ShopProductGallery;
namespace Window.Infra.Data.Repository.ShopProductGallery;

public class ShopProductGalleryCommandRepository : CommandGenericRepository<Domain.Entities.ShopProductGallery.ShopProductGallery> , IShopProductGalleryCommandRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

	public ShopProductGalleryCommandRepository(WindowDbContext context) : base(context)
	{
		_context = context;
	}

    #endregion
}
