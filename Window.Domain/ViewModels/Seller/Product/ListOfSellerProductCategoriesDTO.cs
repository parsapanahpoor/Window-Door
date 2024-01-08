using Window.Domain.Entities;

namespace Window.Domain.ViewModels.Seller.Product;

public record ListOfSellerProductCategoriesDTO
{
	#region properties

	public ShopCategory ShopCategory { get; set; }

    public bool IsSelectedBySellerProduct { get; set; }

    #endregion
}
