using Window.Domain.Entities;
using Window.Domain.ViewModels.Admin.ShopCategory;

namespace Window.Domain.Interfaces.ShopCategory;

public interface IShopCategoryQueryRepository
{
	#region Admin Side 

	Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter);

    Task<Domain.Entities.ShopCategory> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion
}
