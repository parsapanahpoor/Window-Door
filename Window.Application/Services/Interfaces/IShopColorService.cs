﻿using Window.Domain.ViewModels.Admin.ShopColor;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.Services.Interfaces;

public interface IShopColorService
{
	#region General Methods

	Task<Domain.Entities.ShopColors.ShopColor> GetShopColorById(ulong userId, CancellationToken token);

	#endregion

	#region Admin 

	Task<FilterShopColorDTO> FilterShopColor(FilterShopColorDTO filter, CancellationToken cancellation);

	Task<CreateShopColorResult> CreateShopColorAdminSide(CreateShopColorDTO incomingShopColor, CancellationToken cancellationToken);

	Task<EditShopColorDTO?> FillEditShopCategoryDTO(ulong shopColorId, CancellationToken cancellation);

	Task<EditShopColorResult> EditShopColor(EditShopColorDTO shopColorViewModel, CancellationToken cancellation);

	Task<bool> DeleteShopColor(ulong shopColorId, CancellationToken cancellation);

	#endregion

	#region Site Side

	Task<List<ListOfColorsForFilterProductsDTO>> FillListOfColorsForFilterProductsDTO(CancellationToken cancellationToken);

    #endregion
}
