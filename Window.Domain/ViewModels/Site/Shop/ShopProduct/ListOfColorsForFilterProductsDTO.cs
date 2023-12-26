namespace Window.Domain.ViewModels.Site.Shop.ShopProduct;

public record ListOfColorsForFilterProductsDTO
{
    #region properties

    public ulong Id{ get; set; }

    public string ColorTitle { get; set; }

    public string ColorCode { get; set; }

    #endregion
}
