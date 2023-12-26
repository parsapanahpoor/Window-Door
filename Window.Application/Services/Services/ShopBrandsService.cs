using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopBrands;
using Window.Domain.ViewModels.Admin.ShopBrand;

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

    #region General Methods

    public async Task<Domain.Entities.ShopBrands.ShopBrand> GetShopBrandById(ulong userId, CancellationToken token)
    {
        return await _shopBrandsQueryRepository.GetByIdAsync(token, userId);
    }

    #endregion

    #region Admin 

    public async Task<FilterShopBrandDTO> FilterShopBrand(FilterShopBrandDTO filter, CancellationToken cancellation)
    {
        return await _shopBrandsQueryRepository.FilterShopBrand(filter, cancellation);
    }

    public async Task<CreateShopBrandResult> CreateShopBrandAdminSide(CreateShopBrandDTO incomingShopBrand, CancellationToken cancellationToken)
    {
        #region Fill Model 

        var shopBrand = new Domain.Entities.ShopBrands.ShopBrand()
        {
            ShopBrandTitle = incomingShopBrand.Title,
            Priority = incomingShopBrand.Priority,
        };

        #endregion

        await _shopBrandsCommandRepository.AddAsync(shopBrand, cancellationToken);
        await _unitOfWork.SaveChangesAsync();


        return CreateShopBrandResult.Success;
    }

    public async Task<EditShopBrandDTO?> FillEditShopCategoryDTO(ulong shopBrandId, CancellationToken cancellation)
    {
        var shopBrand = await GetShopBrandById(shopBrandId, cancellation);
        if (shopBrand == null) return null;

        var result = new EditShopBrandDTO()
        {
            Id = shopBrand.Id,
            Title = shopBrand.ShopBrandTitle,
            Priority = shopBrand.Priority,
        };

        return result;
    }

    public async Task<EditShopBrandResult> EditShopBrand(EditShopBrandDTO shopBrandViewModel, CancellationToken cancellation)
    {
        Domain.Entities.ShopBrands.ShopBrand? shopBrand = await GetShopBrandById(shopBrandViewModel.Id, cancellation);
        if (shopBrand == null) return EditShopBrandResult.Fail;

        shopBrand.ShopBrandTitle = shopBrandViewModel.Title;
        shopBrand.Priority = shopBrandViewModel.Priority;

        _shopBrandsCommandRepository.Update(shopBrand);
        await _unitOfWork.SaveChangesAsync();

        return EditShopBrandResult.Success;
    }

    public async Task<bool> DeleteShopBrand(ulong shopBrandId, CancellationToken cancellation)
    {
        Domain.Entities.ShopBrands.ShopBrand? shopBrand = await GetShopBrandById(shopBrandId, cancellation);
        if (shopBrand == null) return false;

        shopBrand.IsDelete = true;

        _shopBrandsCommandRepository.Update(shopBrand);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}
