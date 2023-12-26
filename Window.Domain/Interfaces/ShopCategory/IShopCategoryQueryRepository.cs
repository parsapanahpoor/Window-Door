#region Usings

using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Domain.Interfaces.ShopCategory;

#endregion

public interface IShopCategoryQueryRepository
{
    #region General Methods

    Task<List<SelectListViewModel>> GetAllMainShopCategoriesCategories(CancellationToken cancellationToken);

    Task<List<SelectListViewModel>> GetCategoriesChildrent(ulong parentId, CancellationToken cancellationToken);

    #endregion

    #region Admin Side 

    Task<FilterShopCategoryDTO> FilterShopCategory(FilterShopCategoryDTO filter);

    Task<Domain.Entities.ShopCategory> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    #endregion

    #region Site Side 

    Task<List<ShopCategoriesDTO>?> FillLargSideShopCategoriesDTO(CancellationToken cancellationToken);

    Task<List<ShopCategoriesDTO>?> FillShopCategoriesDTO(CancellationToken cancellationToken);

    Task<List<ShopCategoriesForShowInFilterShopProduct>> FillShopCategoriesForShowInFilterShopProduct(ulong shopCategoryParentId, CancellationToken cancellationToken);

    Task<string?> GetShopCategoryTitle(ulong shopCategoryId, CancellationToken cancellationToken);

    #endregion
}
