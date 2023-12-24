#region Using

using Window.Domain.Entities;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Site.Shop.Landing;
namespace Window.Application.Services.Interfaces;

#endregion

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

    #region Site Side 

    Task<List<ShopCategoriesDTO>?> FillShopCategoriesDTO(CancellationToken cancellationToken);

    Task<List<ShopCategoriesDTO>?> FillLargSideShopCategoriesDTO(CancellationToken cancellationToken);

    #endregion
}
