using Window.Domain.ViewModels.Common;

namespace Window.Domain.ViewModels.Admin.ShopCategory;

public class FilterShopCategoryDTO : BasePaging<Domain.Entities.ShopCategory>
{
	#region propreties

	public string? Title { get; set; }

	public ulong? ParentId { get; set; }

	#endregion
}
