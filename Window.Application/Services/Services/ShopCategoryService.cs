﻿using Microsoft.EntityFrameworkCore;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities;
using Window.Domain.Enums.ShopCategory;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Admin.State;

namespace Window.Application.Services.Services;

public class ShopCategoryService : IShopCategoryService
{
	#region Ctor

	private readonly IShopCategoryCommandRepository _shopCategoryCommandRepository;
	private IShopCategoryQueryRepository  _shopCategoryQueryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public ShopCategoryService(IShopCategoryCommandRepository shopCategoryCommandRepository,
							   IShopCategoryQueryRepository shopCategoryQueryRepository,
							   IUnitOfWork unitOfWork)
	{
		_shopCategoryCommandRepository = shopCategoryCommandRepository;
		_shopCategoryQueryRepository = shopCategoryQueryRepository;
		_unitOfWork = unitOfWork;
	}

	#endregion

	#region General Methods

	public async Task<Domain.Entities.ShopCategory> GetShopCategoryById(ulong userId , CancellationToken token)
	{
		return await _shopCategoryQueryRepository.GetByIdAsync(token , userId);
	}

	#endregion

	#region Admin Panel 

	public async Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter)
	{
		return await _shopCategoryQueryRepository.FilterShopCategory(filter);
	}

    public async Task<CreateShopCategoryResult> CreateShopCategoryAdminSide(CreateShopCategoriesDTO shopCategoriesDTO , CancellationToken cancellationToken)
    {
		//Validation For Parent Id
        if (shopCategoriesDTO.ParentId.HasValue && shopCategoriesDTO.ParentId.Value != 0)
        {
            if(await GetShopCategoryById(shopCategoriesDTO.ParentId.Value , cancellationToken) == null)
																			return CreateShopCategoryResult.Fail;
        }

        #region Fill Model 

        var shopCategory = new Domain.Entities.ShopCategory()
        {
            Title = shopCategoriesDTO.Title,
            ShopCategoryType = shopCategoriesDTO.ShopCategoryType
        };

        if (shopCategoriesDTO.ParentId != null && shopCategoriesDTO.ParentId != 0)
        {
            shopCategory.ParentId = shopCategoriesDTO.ParentId;
        }

        #endregion

        await _shopCategoryCommandRepository.AddAsync( shopCategory , cancellationToken) ;
        await _unitOfWork.SaveChangesAsync() ;


        return CreateShopCategoryResult.Success;
    }

    public async Task<EditShopCartDTO?> FillEditShopCategoryDTO(ulong shopCategoryId, CancellationToken cancellation )
    {
        var shopCategory = await GetShopCategoryById(shopCategoryId, cancellation);
        if (shopCategory == null) return null;

        var result = new EditShopCartDTO()
        {
            Title = shopCategory.Title,
            ShopCategoryId = shopCategory.Id,
            ParentId = shopCategory.ParentId,
            ShopCategory = shopCategory.ShopCategoryType
        };

        return result;
    }

    public async Task<EditShopCartResult> EditShopCart(EditShopCartDTO shopCategoryViewModel, CancellationToken cancellation)
    {
        Domain.Entities.ShopCategory? shopCategory = await GetShopCategoryById(shopCategoryViewModel.ShopCategoryId, cancellation);
        if (shopCategory == null)return EditShopCartResult.Fail;

        if (shopCategoryViewModel.ParentId.HasValue && shopCategoryViewModel.ParentId.Value != 0)
        {
            if (await _shopCategoryQueryRepository.GetByIdAsync(cancellation , shopCategoryViewModel.ParentId.Value ) == null)
            {
                return EditShopCartResult.Fail;
            }
        }

        shopCategory.Title = shopCategoryViewModel.Title;
        shopCategory.ShopCategoryType = shopCategoryViewModel.ShopCategory;

        _shopCategoryCommandRepository.Update(shopCategory);
        await _unitOfWork.SaveChangesAsync();

        return EditShopCartResult.Success;
    }

    #endregion
}