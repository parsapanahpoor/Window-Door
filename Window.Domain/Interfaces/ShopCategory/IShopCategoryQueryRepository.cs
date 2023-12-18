#region Usings

using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Site.Shop.Landing;
namespace Window.Domain.Interfaces.ShopCategory;

#endregion

public interface IShopCategoryQueryRepository
{
	#region Admin Side 

	Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter);

    Task<Domain.Entities.ShopCategory> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side 

    Task<List<ShopCategoriesDTO>?> FillShopCategoriesDTO(CancellationToken cancellationToken);

    #endregion
}
