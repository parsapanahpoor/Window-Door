using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities;
using Window.Domain.Interfaces.ShopCategory;
using Window.Domain.ViewModels.Admin.ShopCategory;

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

	public async Task<ShopCategory> GetShopCategoryById(ulong userId , CancellationToken token)
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

        var shopCategory = new ShopCategory()
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

    #endregion
}
