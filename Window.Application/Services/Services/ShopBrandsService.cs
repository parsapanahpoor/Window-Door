using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopBrands;

namespace Window.Application.Services.Services;

public class ShopBrandsService : IShopBrandsService
{
	#region Ctor

	private readonly IShopBrandsCommandRepository _shopBrandsCommandRepository;
	private readonly IShopBrandsQueryRepository _shopBrandsQueryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public ShopBrandsService(IShopBrandsCommandRepository shopBrandsCommandRepository,
							 IShopBrandsQueryRepository shopBrandsQueryRepository , 
							 IUnitOfWork unitOfWork)
	{
		_shopBrandsCommandRepository = shopBrandsCommandRepository;
		_shopBrandsQueryRepository = shopBrandsQueryRepository;
		_unitOfWork = unitOfWork;
	}

	#endregion
}
