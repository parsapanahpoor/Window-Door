using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.ShopColor;

public class FilterShopColorDTO : BasePaging<Entities.ShopColors.ShopColor>
{
	#region propreties

	public string? Title { get; set; }

	#endregion
}
