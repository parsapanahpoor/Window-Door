﻿#region Using

using Window.Domain.Entities;
using Window.Domain.ViewModels.Admin.ShopCategory;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.Services.Interfaces;

#endregion

public interface IShopCategoryService
{
    #region General Methods

    Task<List<SelectListViewModel>> GetAllMainShopCategoriesCategories(CancellationToken cancellationToken);

    Task<List<SelectListViewModel>> GetCategoriesChildrent(ulong parentId, CancellationToken cancellationToken);

    #endregion

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

    Task<List<ShopCategoriesForShowInFilterShopProduct>> FillShopCategoriesForShowInFilterShopProduct(ulong shopCategoryParentId, CancellationToken cancellationToken);

    Task<string?> GetShopCategoryTitle(ulong shopCategoryId, CancellationToken cancellationToken);

    #endregion
}
