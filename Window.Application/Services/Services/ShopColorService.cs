using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopColors;

namespace Window.Application.Services.Services;

public class ShopColorService : IShopColorService
{
	#region Ctor

	private readonly IShopColorsCommandRepository _shopColorsCommandRepository;
	private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public ShopColorService(IShopColorsCommandRepository shopColorsCommandRepository,
							IShopColorsQueryRepository shopColorsQueryRepository,
							IUnitOfWork unitOfWork)
	{
		_shopColorsCommandRepository = shopColorsCommandRepository;
		_shopColorsQueryRepository = shopColorsQueryRepository;
		_unitOfWork = unitOfWork;
	}

	#endregion
}
