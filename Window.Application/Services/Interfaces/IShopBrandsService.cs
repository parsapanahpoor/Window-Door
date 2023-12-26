﻿using Window.Domain.ViewModels.Admin.ShopBrand;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.Services.Interfaces;

public interface IShopBrandsService
{
    #region General Methods

    Task<Domain.Entities.ShopBrands.ShopBrand> GetShopBrandById(ulong userId, CancellationToken token);

    #endregion

    #region Admin 

    Task<FilterShopBrandDTO> FilterShopBrand(FilterShopBrandDTO filter, CancellationToken cancellation);

    Task<CreateShopBrandResult> CreateShopBrandAdminSide(CreateShopBrandDTO incomingShopBrand, CancellationToken cancellationToken);

    Task<EditShopBrandDTO?> FillEditShopCategoryDTO(ulong shopBrandId, CancellationToken cancellation);

    Task<EditShopBrandResult> EditShopBrand(EditShopBrandDTO shopBrandViewModel, CancellationToken cancellation);

    Task<bool> DeleteShopBrand(ulong shopBrandId, CancellationToken cancellation);

    #endregion

    #region Site Side

    Task<List<ListOfBrandsForFilterProductsDTO>> FillListOfBrandsForFilterProductsDTO(CancellationToken cancellationToken);

    #endregion
}