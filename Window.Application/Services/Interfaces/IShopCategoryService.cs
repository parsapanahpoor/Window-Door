using Window.Domain.Entities;
using Window.Domain.ViewModels.Admin.ShopCategory;

namespace Window.Application.Services.Interfaces;

public interface IShopCategoryService
{
	#region Admin Panel

	Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter);

    Task<ShopCategory> GetShopCategoryById(ulong userId, CancellationToken token);

    Task<CreateShopCategoryResult> CreateShopCategoryAdminSide(CreateShopCategoriesDTO shopCategoriesDTO, CancellationToken cancellationToken);

    Task<EditShopCartDTO?> FillEditShopCategoryDTO(ulong shopCategoryId, CancellationToken cancellation);

    Task<EditShopCartResult> EditShopCart(EditShopCartDTO shopCategoryViewModel, CancellationToken cancellation);

    Task<bool> DeleteShopCategory(ulong shopCategoryId, CancellationToken cancellation);

    #endregion
}
