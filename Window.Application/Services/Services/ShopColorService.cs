using Microsoft.EntityFrameworkCore;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

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

	#region General Methods

	public async Task<Domain.Entities.ShopColors.ShopColor> GetShopColorById(ulong userId, CancellationToken token)
	{
		return await _shopColorsQueryRepository.GetByIdAsync(token, userId);
	}

	#endregion

	#region Admin 

	public async Task<FilterShopColorDTO> FilterShopColor(FilterShopColorDTO filter, CancellationToken cancellation)
	{
		return await _shopColorsQueryRepository.FilterShopColor(filter, cancellation);
	}

	public async Task<CreateShopColorResult> CreateShopColorAdminSide(CreateShopColorDTO incomingShopColor, CancellationToken cancellationToken)
	{
		#region Fill Model 

		var shopColor = new Domain.Entities.ShopColors.ShopColor()
		{
			ColorTitle = incomingShopColor.Title,
			ColorCode = incomingShopColor.ShopColorCode,
			Priority = incomingShopColor.Priority,
		};

		#endregion

		await _shopColorsCommandRepository.AddAsync(shopColor, cancellationToken);
		await _unitOfWork.SaveChangesAsync();


		return CreateShopColorResult.Success;
	}

	public async Task<EditShopColorDTO?> FillEditShopCategoryDTO(ulong shopColorId, CancellationToken cancellation)
	{
		var shopColor = await GetShopColorById(shopColorId, cancellation);
		if (shopColor == null) return null;

		var result = new EditShopColorDTO()
		{
			Id = shopColor.Id,
			Title = shopColor.ColorTitle,
			Priority = shopColor.Priority,
			ColorCode = shopColor.ColorCode
		};

		return result;
	}

	public async Task<EditShopColorResult> EditShopColor(EditShopColorDTO shopColorViewModel, CancellationToken cancellation)
	{
		Domain.Entities.ShopColors.ShopColor? shopColor = await GetShopColorById(shopColorViewModel.Id, cancellation);
		if (shopColor == null) return EditShopColorResult.Fail;

		shopColor.ColorTitle = shopColorViewModel.Title;
		shopColor.Priority = shopColorViewModel.Priority;
		shopColor.ColorCode = shopColorViewModel.ColorCode;

		_shopColorsCommandRepository.Update(shopColor);
		await _unitOfWork.SaveChangesAsync();

		return EditShopColorResult.Success;
	}

	public async Task<bool> DeleteShopColor(ulong shopColorId, CancellationToken cancellation)
	{
		Domain.Entities.ShopColors.ShopColor? shopColor = await GetShopColorById(shopColorId, cancellation);
		if (shopColor == null) return false;

		shopColor.IsDelete = true;

		_shopColorsCommandRepository.Update(shopColor);
		await _unitOfWork.SaveChangesAsync();

		return true;
	}

    #endregion

    #region Site Side

    public async Task<List<ListOfColorsForFilterProductsDTO>> FillListOfColorsForFilterProductsDTO(CancellationToken cancellationToken)
    {
		return await _shopColorsQueryRepository.FillListOfColorsForFilterProductsDTO(cancellationToken);
    }

    #endregion
}
