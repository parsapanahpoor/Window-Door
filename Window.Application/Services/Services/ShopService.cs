using Microsoft.EntityFrameworkCore;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Services;

public class ShopProductService : IShopProductService
{
	#region Ctor

	private readonly IShopProductCommandRepository _shopProductCommandRepository;
	private readonly IShopProductQueryRepository _shopProductQueryRepository;
	private readonly IUnitOfWork _unitOfWork;

    public ShopProductService(IShopProductCommandRepository shopProductCommandRepository ,
                       IShopProductQueryRepository shopProductQueryRepository ,
                       IUnitOfWork unitOfWork) 
    {
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Seller Side 

    public async Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation)
    {
        return await _shopProductQueryRepository.FilterShopProductSellerSide(filter , cancellation);
    }

    #endregion
}
