using Microsoft.EntityFrameworkCore;
using Window.Domain.ViewModels.Admin.ShopColor;

namespace Window.Domain.Interfaces.ShopColors;

public interface IShopColorsQueryRepository
{

	#region Admin 

	Task<FilterShopColorDTO> FilterShopColor(FilterShopColorDTO filter, CancellationToken cancellation);

	Task<Domain.Entities.ShopColors.ShopColor> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

	#endregion
}
