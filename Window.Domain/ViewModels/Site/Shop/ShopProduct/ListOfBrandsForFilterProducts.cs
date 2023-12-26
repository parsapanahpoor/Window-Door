namespace Window.Domain.ViewModels.Site.Shop.ShopProduct;

public record ListOfBrandsForFilterProductsDTO
{
    #region properties

    public ulong Id{ get; set; }

    public string BrandTitle { get; set; }

    #endregion
}
